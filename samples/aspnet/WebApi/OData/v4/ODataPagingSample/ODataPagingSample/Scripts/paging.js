// paging.js

// registers a knockout binding handler to change the visiblity of the previous page
// and next page buttons depending on whether the page exists
ko.bindingHandlers.hidden = {
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        element.style.visibility = value ? "hidden" : "visible";
    }
}

// the URL of the feed
var feedUrl = "api/Movies";

var viewModel = new Object();
viewModel.movies = ko.observable();
viewModel.currentPage = ko.observable();
viewModel.previousPages = ko.observableArray();
viewModel.nextPage = ko.observable();
viewModel.currentPageNumber = ko.observable();
viewModel.totalPageNumber = ko.observable();

$(document).ready(function () {
    ko.applyBindings(viewModel);
});

// When the run query button is clicked, make a request to get the feed with the specified query string
var runQueryClicked = function () {
    var queryUrl = feedUrl + "?" + document.getElementById("query").value;
    $.get(queryUrl, function (data) {
        viewModel.currentPage(queryUrl);
        viewModel.currentPageNumber(1);
        viewModel.totalPageNumber(data["odata.count"] == undefined ? null : Math.ceil(data["odata.count"] / data.value.length));
        viewModel.previousPages.removeAll();
        viewModel.nextPage(data["odata.nextLink"]);
        viewModel.movies(data.value);
    });
}

// When the previous button is clicked, make a request to get the previous page viewed
var previousButtonClicked = function () {
    var previousPageLink = viewModel.previousPages.pop();
    $.get(previousPageLink, function (data) {
        viewModel.nextPage(viewModel.currentPage());
        viewModel.currentPage(previousPageLink);
        viewModel.currentPageNumber(viewModel.currentPageNumber() - 1);
        viewModel.movies(data.value);
    });
}

// When the next button is clicked, make a request to get the next page using the link
// from the current page's request
var nextButtonClicked = function () {
    var nextPageLink = viewModel.nextPage();
    $.get(nextPageLink, function (data) {
        viewModel.previousPages.push(viewModel.currentPage());
        viewModel.currentPage(nextPageLink);
        viewModel.nextPage(data["odata.nextLink"]);
        viewModel.currentPageNumber(viewModel.currentPageNumber() + 1);
        viewModel.movies(data.value);
    });
}