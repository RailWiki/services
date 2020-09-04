using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using RailWiki.Shared.Configuration;

namespace RailWiki.Shared.Services.FileStorage
{
    public class AmazonS3FileService : IFileService
    {
        const int objectMaxAge = 2592000;

        private readonly IAmazonS3 _amazonS3;
        private readonly FileStorageConfig _fileStoreConfig;

        public AmazonS3FileService(IAmazonS3 amazonS3,
            IOptions<FileStorageConfig> fileStoreConfig)
        {
            _amazonS3 = amazonS3;
            _fileStoreConfig = fileStoreConfig.Value;
        }

        public async Task<bool> FileExistsAsync(string path)
        {
            try
            {
                var response = await _amazonS3.GetObjectMetadataAsync(_fileStoreConfig.S3.BucketName, path);
                return true; // If this succeeds then it was found
            }
            catch(AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw;
            }
        }

        public string ResolveFilePath(string path) => throw new NotImplementedException();

        public string ResolveFileUrl(string path)
        {
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            return $"https://s3.amazonaws.com/{_fileStoreConfig.S3.BucketName}/{path}";
        }

        public async Task SaveFileAsync(string path, string contentType, Stream stream)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _fileStoreConfig.S3.BucketName,
                InputStream = stream,
                Key = path,
                AutoCloseStream = false,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead
            };
            putRequest.Headers.CacheControl = $"max-age={objectMaxAge}";

            await _amazonS3.PutObjectAsync(putRequest);
        }

        public async Task DeleteFileAsync(string path)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _fileStoreConfig.S3.BucketName,
                Key = path
            };

            await _amazonS3.DeleteObjectAsync(deleteRequest);
        }
    }
}
