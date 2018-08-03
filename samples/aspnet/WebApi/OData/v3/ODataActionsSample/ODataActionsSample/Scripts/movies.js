// The web page uses Knockout.js for data-binding.

// the URL of the feed
var feedUrl = "odata/Movies"; 

window.viewModel = (function () {
    var self = this;

    self.movies = ko.observableArray();
    self.title = ko.observable();
    self.errorMessage = ko.observable();

    // Function to invoke the non-bindable "CreateMovie" action.
    self.createMovie = function () {
        ajaxRequest("post", "odata/CreateMovie", { Title: self.title() })
            .done(function (result) {
                self.movies.push(new movie(result))
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                self.errorMessage(errorThrown);
            });
    }

    // Function to invoke the "CheckOutMany" action, which is bound to the Movies entity set.
    self.checkoutMany = function () {

        // The action takes a parameter named "Movies" which is a list of movie IDs.
        // Create the list from the movies that the user has selected in the UI.
        // Also create a dictionary by ID so that we can easily update them with the results.

        var keys = {};
        var data = {
            MovieIDs: $.map(self.movies(), function (movie) {
                if (movie.selected()) {
                    keys[movie.id] = movie;
                    return movie.id;
                }
            })
        };
        ajaxRequest("post", feedUrl + "/CheckOutMany", data)
            .done(function (result) {
                $.each(result.value, function (index, m) {
                    keys[m.ID].update(m);
                });
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                self.errorMessage(errorThrown);
            });
    }

    // The client model for the Movie entity.
    function movie(data, parent) {
        var self = this;

        self.id = data.ID;
        self.title = data.Title;
        self.dueDate = ko.observable();
        self.checkoutUrl = ko.observable();     // Action link 
        self.returnMovieUrl = ko.observable();  // Action link
        self.selected = ko.observable();

        self.update = updateMovie;

        self.checkout = function () {
            invokeAction(self.checkoutUrl());
        };

        self.returnMovie = function () {
            invokeAction(self.returnMovieUrl());
        };

        // Invoke "checkout" or "return" action. Both actions take no parameter data.
        function invokeAction(url) {
            ajaxRequest("post", url)
                .done(function (updated) {
                    updateMovie(updated);
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    parent.errorMessage(errorThrown);
                });
        }

        self.update(data);

        // Update the model with new data from the server.
        function updateMovie(data) {
            var dueDate = data.DueDate ? new Date(data.DueDate) : null;
            self.dueDate(dueDate);

            if (data["#Container.CheckOut"]) {
                self.checkoutUrl(data["#Container.CheckOut"].target);
            }
            else {
                self.checkoutUrl(null);
            }
            if (data["#Container.Return"]) {
                self.returnMovieUrl(data["#Container.Return"].target);
            }
            else {
                self.returnMovieUrl(null);
            }
        }
    }

    // Ask for full OData metadata, so we get all the action links. We could infer some links from the
    // service metadata document. The approach here is simpler but requires larger response payloads.
    $.ajaxSetup({
        accepts: { "json": "application/json; odata=fullmetadata" }
    });

    // Get the movie list.
    ajaxRequest("get", feedUrl)
        .done(function (data) {
            var mapped = $.map(data.value, function (element) { return new movie(element, self); });
            self.movies(mapped);
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            self.errorMessage(errorThrown)
        });

    // Ajax helper
    function ajaxRequest(type, url, data) {
        var options = {
            dataType: "json",
            contentType: "application/json",
            type: type,
            data: data ? ko.toJSON(data) : null
        };
        self.errorMessage(null); // clear error message
        return $.ajax(url, options);
    }
})();

$(document).ready(function () {
    ko.applyBindings(viewModel);
});