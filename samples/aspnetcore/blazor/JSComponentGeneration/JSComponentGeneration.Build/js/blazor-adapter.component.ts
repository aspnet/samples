import { Component, ElementRef, EventEmitter, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';

declare const Blazor: any

@Component({
  template: '',
})

export abstract class BlazorAdapterComponent implements OnDestroy, OnChanges {
  private _addComponentPromise: Promise<any> | null = null;
  private _parameterValues: any = {};
  private _isDisposed = false;
  private _hasPendingSetParameters = false;

  constructor(private _parentElementReference: ElementRef) { }

  ngOnChanges(changes: SimpleChanges): void {
    for (const propName in changes) {
      this._parameterValues[propName] = changes[propName].currentValue;
    }

    if (this._addComponentPromise === null) {
      this._addRootComponent();
    } else if (!this._hasPendingSetParameters) {
      this._supplyUpdatedParameters();
    }
  }

  ngOnDestroy(): void {
    setTimeout(() => async () => {
      this._isDisposed = true;
      if (this._addComponentPromise !== null) {
        const rootComponent = await this._addComponentPromise;
        rootComponent.dispose();
      }
    }, 1000);
  }

  private _addRootComponent() {
    const nativeElement = this._parentElementReference.nativeElement;
    const componentIdentifier = `${nativeElement.tagName.toLowerCase()}-angular`;

    const parameters = {
      ...this._parameterValues,
    };

    for (const [key, value] of Object.entries(this)) {
      if (value instanceof EventEmitter) {
        parameters[key] = (...args: any[]) => value.emit(...args);
      }
    }

    this._hasPendingSetParameters = true;
    this._addComponentPromise = Blazor.rootComponents.add(nativeElement, componentIdentifier, parameters).then((rootComponent: any) => {
      this._hasPendingSetParameters = false;
      return rootComponent;
    });
  }

  private async _supplyUpdatedParameters() {
    if (this._hasPendingSetParameters) {
      return;
    }

    this._hasPendingSetParameters = true;
    const rootComponent = await this._addComponentPromise;
    if (!this._isDisposed) {
      await rootComponent.setParameters(this._parameterValues);
    }
    this._hasPendingSetParameters = false;
  }
}
