HostedClientCertificateSample
-------------

This ASP.NET Web API sample illustrates how to create a hosted web API using client certificate for client authentication purpose.

In this sample, the Sample controller has two actions: 

1. GetItems will return a list of sample Items, and this action does not require client to send the client certificate;

2. PostItem shows how to add one sample item with an HTTP POST message, and this action only allows authenticated client with administrator privilege;

For a detailed description of this sample, please see
http://blogs.msdn.com/b/hongmeig1/archive/2012/05/11/how-to-access-clientcertificate-in-a-host-agnostic-manner.aspx
