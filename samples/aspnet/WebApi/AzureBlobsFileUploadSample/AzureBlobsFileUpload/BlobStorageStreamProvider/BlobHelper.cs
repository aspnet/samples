using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureBlobsFileUpload
{
    internal static class BlobHelper
    {
        public static async Task<CloudBlobContainer> GetBlobContainer(string containerName)
        {
            if (String.IsNullOrEmpty(containerName))
            {
                throw new ArgumentException("containerName");
            }

            // Retrieve storage account from connection-string
            string connectionString = CloudConfigurationManager.GetSetting("CloudStorageConnectionString");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the blob client 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. Note that container name must use lower case
            CloudBlobContainer container = blobClient.GetContainerReference(containerName.ToLowerInvariant());

            // Create options for communicating with the blob container.
            BlobRequestOptions options = new BlobRequestOptions();

            // Create the container if it doesn't already exist
            bool result = await Task.Factory.FromAsync<BlobRequestOptions, bool>(container.BeginCreateIfNotExist, container.EndCreateIfNotExist, options, state: null);

            // Enable public access to blob
            BlobContainerPermissions permissions = container.GetPermissions();
            if (permissions.PublicAccess == BlobContainerPublicAccessType.Off)
            {
                permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                container.SetPermissions(permissions);
            }

            return container;
        }
    }
}