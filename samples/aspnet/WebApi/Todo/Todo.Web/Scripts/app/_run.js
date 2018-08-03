$(function () {
    // Instance of AppViewModel
    app.initialize();

    // Activate Knockout
    ko.validation.init({ grouping: { observable: false } });
    ko.applyBindings(app);
});
