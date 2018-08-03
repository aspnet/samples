AzureBlobsFileUploadSample
--------------------------

This ASP.NET Web API sample illustrates how to use the Azure Blob Store as back-end 
store for files uploaded to a sample controller using MIME multipart file upload. It 
also demonstrates how to get a list of files already in the store. 

The sample requires the Windows Azure SDK for .NET which you can find here
https://www.windowsazure.com/en-us/develop/net/

The functionality is implemented as two actions in the FilesController sample class: 
a Post and a Get action. The Post action is for uploading MIME multipart and then 
save the contents asynchronously in Azure Blob Store. The Get action is for getting 
a list of blobs already in the store. Both operations happen asynchronously without 
blocking any threads in the process using the .NET 4.5 async/await keywords.

The sample is set up using the local Azure Storage Emulator which doesn't communicate with 
the actual Azure Blob Store. In order to use the real Azure Blob Store you need to get an 
Azure Storage Account and add the configuration to the Web.config file by updating the 
CloudStorageConnectionString key as follows:

<appSettings>
  <add key="CloudStorageConnectionString" value="DefaultEndpointsProtocol=http;AccountName=[your account name];AccountKey=[your account key]"/> 
</appSettings>

For details, please see the blog below.

For a detailed description of this sample, please see 
http://blogs.msdn.com/b/yaohuang1/archive/2012/07/02/asp-net-web-api-and-azure-blob-storage.aspx

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487