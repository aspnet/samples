import React, { useCallback, useEffect, useRef, useState } from "react";

function useForceUpdate() {
  const [, setState] = useState();
  return () => setState({});
}

export function useBlazor(identifier, props) {
  const forceUpdate = useForceUpdate();

  // We prefer useRef over useState because we don't want changes to internal housekeeping
  // state to cause the component to re-render.
  const previousPropsRef = useRef({});
  const addRootComponentPromiseRef = useRef(null);
  const hasPendingSetParametersRef = useRef(true);
  const isDisposedRef = useRef(false);

  // After the initial render, we use the template element ref to find its parent node so we
  // can attach the dynamic root component to it.
  const onTemplateRender = useCallback((node) => {
    if (!node) {
      return;
    }

    const parentElement = node.parentElement;

    // We defer adding the root component until after this component re-renders.
    // If Blazor removes the template element from the DOM before React does,
    // it can throw off React's DOM management.
    addRootComponentPromiseRef.current = Promise.resolve().then(() => {
      // eslint-disable-next-line no-undef
      return Blazor.rootComponents.add(parentElement, `${identifier}-react`, props);
    }).then((rootComponent) => {
      hasPendingSetParametersRef.current = false;
      return rootComponent;
    });

    // We want to cause a re-render here so the template element gets removed by
    // React rather than by Blazor.
    forceUpdate();
  }, [forceUpdate, identifier, props]);

  // Supply .NET with updated parameters.
  useEffect(() => {
    if (hasPendingSetParametersRef.current) {
      return;
    }

    const parameters = {};
    let parametersDidChange = false;

    // Only send changed parameters to .NET.
    for (const [key, value] of Object.entries(props)) {
      if (previousPropsRef.current[key] !== value) {
        parameters[key] = value;
        parametersDidChange = true;
      }
    }

    if (!parametersDidChange) {
      return;
    }

    hasPendingSetParametersRef.current = true;
    addRootComponentPromiseRef.current.then((rootComponent) => {
      if (!isDisposedRef.current) {
        return rootComponent.setParameters(parameters);
      }
    }).then(() => {
      hasPendingSetParametersRef.current = false;
    });
  }, [props]);

  // This effect will run when the component is about to unmount.
  useEffect(() => () => {
    setTimeout(() => {
      isDisposedRef.current = true;
      if (addRootComponentPromiseRef.current) {
        addRootComponentPromiseRef.current.then((rootComponent) => rootComponent.dispose());
      }
    }, 1000);
  }, []);

  // Update the previous props with the current props after each render.
  useEffect(() => {
    previousPropsRef.current = props;
  });

  return addRootComponentPromiseRef.current === null
    ? <template ref={onTemplateRender} />
    : null;
}
