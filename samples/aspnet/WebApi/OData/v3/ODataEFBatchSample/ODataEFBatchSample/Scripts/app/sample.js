var sample = function (testOut) {

    // Helper function to create and send a batch request that inserts a customer, a related order and a related order
    // line using the $Content-ID syntax.
    // Returns a promise that will contain the batch response when fulfilled or an error when rejected.
    var sendBatchRequest = function () {
        var builder = ODataEFBatchSample.createBatchBuilder();
        var changeSet = builder.addChangeSet();

        // Add a Customer.
        changeSet.addRequest({
            requestUri: '/odata/Customers',
            headers: { 'Content-ID': 1 },
            method: 'POST',
            data: { Id: 3 }
        });

        // Add a related order to the customer in the previous request.
        changeSet.addRequest({
            requestUri: '$1/Orders',
            headers: { 'Content-ID': 2 },
            method: 'POST',
            data: { Id: 5 }
        });

        // Add a related order line to the order in the previous request.
        changeSet.addRequest({
            requestUri: '$2/OrderLines',
            method: 'POST',
            data: { Id: 6 }
        });

        var batchRequest = builder.getBatchRequest();
        return ODataEFBatchSample.postBatch(batchRequest)
    };

    // Extracts the responses in the first changeset of the batch response.
    var getResponses = function (data) {
        var batchResponses = data.__batchResponses;
        var changeSet = batchResponses[0];
        return changeSet.__changeResponses;
    };

    return {
        // Send a batch request and check that all the responses are 201 (Created) and that the body isn't empty. (It
        // contains the inserted entity).
        contentIdRequest: function () {

            testOut.write('<h1>Sending a batch request to insert a customer a related order and a related order line '
                + 'using $Content-ID</h1>');

            return sendBatchRequest().then(function (data) {

                responses = getResponses(data);

                // Check that we got back three responses.
                testOut.write('<h3>Number of responses: ' + responses.length + '</h3>');

                // Check each individual response.
                responses.forEach(function (value, index) {
                    testOut.write('<h3>Status code for response ' + index + ': ' + value.statusCode + '</h3>');
                    testOut.writeLine('<span>' + JSON.stringify(value.data, null, '    ') + '</span>');
                });
            });
        },

        // 1) Send a batch request that inserts a customer, a related order and a related order line. 
        // 2) Read the responses, find the inserted customer (it has Content-ID: 1), extract the key from the customer and 
        //    the location url from the response.
        // 3) Send a batch request with a single changeset containing a DELETE request to the location url (The customer we
        //    just inserted) and ensure that the answer is 204.
        // 4) Query the service to retrieve the orders with 'CustomerId' equal to the key of the customer we just deleted 
        //    and check that the result contains 0 entities.
        cascadeDeletes: function () {
            // The key for the inserted customer. Declared here so that it can be set on the first continuation and used
            // later in the promises chain.
            var entityKey;

            testOut.write('<h1>Sending a batch request to delete the customer entity we just inserted, the related ' +
                'order and order line will be deleted at the same time</h1>');

            // Send a batch request that inserts a customer, a related order and a related order line.
            return sendBatchRequest().then(function (data) {
                var responses = getResponses(data);

                // Find the customer we just inserted.
                var response = responses.filter(function (response) {
                    return response.headers['Content-ID'] === '1';
                });

                // Ensure we only got one customer back.
                if (response.length != 1) {
                    throw new Error('We got back more than one response with Content-ID === 1');
                }

                // Extract the customer location url and the customer key from the response for later use.
                var locationUrl = response[0].headers.Location;
                entityKey = response[0].data.Id;

                // Create a batch request with a single changeset and a single Delete operation inside it to remove the
                // customer we just created.
                var builder = ODataEFBatchSample.createBatchBuilder();
                var changeSet = builder.addChangeSet();
                changeSet.addRequest({
                    requestUri: locationUrl,
                    method: 'DELETE'
                });
                var batchRequest = builder.getBatchRequest();

                // Send the batch request to the server and return a promise that will be fullfiled or rejected when the
                // call to the server finishes.
                testOut.write('<h3>Sending the $batch request to delete the existing customer</h3>');
                return ODataEFBatchSample.postBatch(batchRequest);

            }).then(function (data) {
                // Extract the responses from the changeset and pick the first one.
                var result = getResponses(data)[0];
                // Ensure that we deleted the customer correctly.
                testOut.writeLine('The status code of the response for the request to delete the customer is: ' + result.statusCode);
            }).then(function () {
                // Try to get the orders related to the customer we just deleted.
                testOut.write('<h3>Querying for orders with CustomerId = ' + entityKey + '</h3>');
                return ODataEFBatchSample.getOrderLines(entityKey);
            }).then(function (data) {
                // Ensure that all related orders have been deleted.
                testOut.writeLine('Orders returned: ' + data.results.length);
            });
        }
    };
};

