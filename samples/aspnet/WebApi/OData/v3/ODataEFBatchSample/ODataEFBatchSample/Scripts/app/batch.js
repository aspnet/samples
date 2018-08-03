/// <reference path="../q.min.js" />
/// <reference path="../datajs-1.1.3.min.js" />

// This is a helper module that contains functions to make building and sending requests to our service easier.
var ODataEFBatchSample = (function (odata, $) {
    return {

        // Posts a batch request to the service endpoint.
        postBatch: function (requests) {
            // Create a "TaskCompletionSource" to wrap the result of the callbacks to datajs into a promise interface.
            var deferred = $.Deferred();

            // Fulfills the promise when the call to datajs is successful.
            var success = function (data, response) {
                deferred.resolve(data);
            };

            // Rejects the promise when the call to datajs throws an error.
            var error = function (error, response) {
                deferred.reject(error);
            };

            // Creates a batch request with the requests passed as argument.
            var batchRequest = {
                requestUri: "/odata/$batch",
                method: "POST",
                data: requests
            };

            // Sends the request to the service asynchonously.
            odata.request(batchRequest, success, error, odata.batchHandler);

            // Returns a promise that will be fulfilled or rejected when the call to the service returns.
            return deferred.promise();
        },

        // Creates a batch builder to make building batch requests easier.
        createBatchBuilder: function () {
            var data = {
                __batchRequests: []
            };

            builder = {};

            // Adds a changeset to the batch request and returns a changeSetBuilder in order to add requests
            // to the changeset.
            builder.addChangeSet = function () {
                var changeset = { __changeRequests: [] };
                data.__batchRequests.push(changeset);

                var changeSetBuilder = {};

                // Adds a request at the end of the current changeset and returns the changeSetBuilder to allow adding
                // more requests to the changeSet.
                changeSetBuilder.addRequest = function (request) {
                    var requests = changeset.__changeRequests;
                    requests.push(request);
                    return changeSetBuilder;
                };

                return changeSetBuilder;
            };

            // Returns a batch request containing the changesets added using the builder.
            builder.getBatchRequest = function () {
                return data;
            };

            return builder;
        },

        // Queries the OData service for orders whose CustomerId is equal to the one indicated by key.
        getOrderLines: function (key) {
            // Create a "TaskCompletionSource" to wrap the result of the callbacks to datajs into a promise interface.
            var deferred = $.Deferred();

            // Fulfills the promise when the call to datajs is successful.
            var success = function (data) {
                deferred.resolve(data);
            };

            // Rejects the promise when the call to datajs throws an error.
            var error = function (error) {
                deferred.reject(error);
            };

            // Sends the request to the service asynchonously.
            OData.read('/odata/Orders?$filter=CustomerId eq ' + key, success, error);

            // Returns a promise that will be fulfilled or rejected when the call to the service returns.
            return deferred.promise();
        }
    };
})(OData, jQuery);