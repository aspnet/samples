using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureBlobsFileUpload.Controllers
{
    public class FilesController : ApiController
    {
        // The container name we use for this sample
        private static readonly string _containerName = "webapicontainer";

        // Get or create the blob container once. We reuse the container for all subsequent calls.
        private static readonly Lazy<Task<CloudBlobContainer>> _blobContainer =
            new Lazy<Task<CloudBlobContainer>>(async () => await BlobHelper.GetBlobContainer(_containerName));

        // This action returns the list of blobs that have been stored in the container.
        public async Task<IEnumerable<AzureBlobInfo>> Get()
        {
            // Get or create the blob container
            CloudBlobContainer container = await _blobContainer.Value;

            // Get the list of blobs and convert them into our AzureBlobInfo class
            return container.ListBlobs().OfType<CloudBlockBlob>().Select(blob =>
                new AzureBlobInfo
                {
                    Name = blob.Name,
                    Size = blob.Properties.Length,
                    ContentType = blob.Properties.ContentType,
                    Location = blob.Uri.AbsoluteUri
                });
        }

        /// <summary>
        /// This action takes a MIME multipart file upload and stores the result in 
        /// the Azure cloud store. Both the reading and writing of the blob happens
        /// asynchronously without blocking any threads.
        /// </summary>
        public async Task<Collection<AzureBlobInfo>> Post()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Get or create the blob container
            CloudBlobContainer container = await _blobContainer.Value;

            // Create a AzureBlobStorageMultipartProvider and process the request
            AzureBlobStorageMultipartProvider streamProvider = new AzureBlobStorageMultipartProvider(container, Path.GetTempPath());
            await Request.Content.ReadAsMultipartAsync<AzureBlobStorageMultipartProvider>(streamProvider);

            // Return result from storing content in the blob container
            return streamProvider.AzureBlobs;
        }
    }
}