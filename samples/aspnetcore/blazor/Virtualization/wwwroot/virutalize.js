window.VirtualizedComponent = {
    _ticking: false,
    _initialize: function (component, contentElement) {
        // Find closest scrollable container
        let scrollableContainer = contentElement.parentElement;
        while (!(scrollableContainer.style.overflow || scrollableContainer.style.overflowY)) {
            scrollableContainer = scrollableContainer.parentElement;
            if (!scrollableContainer) {
                throw new Error('No scrollable container was found around VirtualizedComponent.');
            }
        }

        // TODO: Also listen for 'scrollableContainer' being resized or 'contentElement' moving
        // within it, and notify.NET side
        scrollableContainer.addEventListener('scroll', e => {
            const lastKnownValues =  {
                containerRect: scrollableContainer.getBoundingClientRect(),
                contentRect: readClientRectWithoutTransform(contentElement)
            };

            if (!this._ticking) {
                requestIdleCallback(() => {
                    component.invokeMethodAsync('OnScroll', lastKnownValues);
                    this._ticking = false;
                });

                this._ticking = true;
            }
        });

        return {
            containerRect: scrollableContainer.getBoundingClientRect(),
            contentRect: readClientRectWithoutTransform(contentElement)
        };
    }
};

function readClientRectWithoutTransform(elem) {
    const rect = elem.getBoundingClientRect();
    const translateY = parseFloat(elem.getAttribute('data-translateY'));
    return { top: rect.top - translateY, bottom: rect.bottom - translateY, left: rect.left, right: rect.right, height: rect.height, width: rect.width };
}
