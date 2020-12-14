using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Helpers
{
    public class AzureStorageConfig
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string ImageContainer { get; set; }
        public string ThumbnailContainer { get; set; }
    }
    
    public static class StorageHelper
    {
        public static bool IsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = { ".jpg", ".png", ".gif", ".jpeg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

       
        
        public static async Task<Tuple<string, string>> MyUploadFileToStorage(Stream fileStream, string fileName,
            AzureStorageConfig _storageConfig)
        {
            // Create a URI to the blob
            Uri blobImageUri = new Uri("https://" +
                                  _storageConfig.AccountName +
                                  ".blob.core.windows.net/" +
                                  _storageConfig.ImageContainer +
                                  "/" + fileName);
            
            Uri blobThumbnailUri = new Uri("https://" +
                                       _storageConfig.AccountName +
                                       ".blob.core.windows.net/" +
                                       _storageConfig.ThumbnailContainer +
                                       "/" + fileName);

            // Create StorageSharedKeyCredentials object by reading
            // the values from the configuration (appsettings.json)
            StorageSharedKeyCredential storageCredentials =
                new StorageSharedKeyCredential(_storageConfig.AccountName, _storageConfig.AccountKey);

            // Create the blob client.
            BlobClient blobClient = new BlobClient(blobImageUri, storageCredentials);

            // Upload the file
            await blobClient.UploadAsync(fileStream);

            return Tuple.Create(blobImageUri.ToString(), blobThumbnailUri.ToString());
        }

       
    }
}