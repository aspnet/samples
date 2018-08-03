RelaySample
-----------

This sample shows how to relay requests and responses from a backend service through
a relay controller asynchronously and without buffering the contents on the server. 
The sample uses two controllers as follows:

1) The ContentController supports GET and PUT of some content
2) The RelayController relays HTTP requests and responses to the ContentController

In this sample, the two controllers are co-located but they can be anywhere. The 
ContentController is just to illustrate a service which may be relayed. The relay
controller does not care about the shape of the ContentController.

A request and response flows as follows:

  Client <--> RelayController <--> ContentController

The data flow is such that all data is exchanged asynchronously and without buffering
the content in the RelayController. This allows the sample to pipe though many GBytes
without running out of resources.

The "curl" command line utility may be used as a client. For example, a relayed PUT 
request may look like this:

  curl --upload-file <LargeSampleData.random> http://localhost:50231/api/relay -m 20000 -o response.txt

and a relayed GET request may look like this:

  curl http://localhost:50231/api/relay -m 20000 -o response.bin

For more information about curl, please see
http://curl.haxx.se/

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487