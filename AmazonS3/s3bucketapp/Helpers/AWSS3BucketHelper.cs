using Amazon.Lambda;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using s3bucketapp.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace s3bucketapp.Helpers
{
    public interface IAWSS3BucketHelper
    {
        Task<bool> UploadFile(System.IO.Stream inputStream, string fileName);
        Task<ListVersionsResponse> FilesList(string bucketName);
        Task<ListBucketsResponse> BucketList();
        Task<Stream> GetFile(string key);
        Task<bool> DeleteFile(string key);
        Task<bool> DeleteBucket(string key);
        Task<bool> AddBucket(string key);
        Task<DetectLabelsResponse> RecognizeImage(string bucketName, string fileName);

    }

    public class AWSS3BucketHelper : IAWSS3BucketHelper
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly ServiceConfiguration _settings;
        private readonly IAmazonRekognition rekognition;

        public AWSS3BucketHelper(IAmazonS3 s3Client, IOptions<ServiceConfiguration> settings)
        {
            this._amazonS3 = s3Client;
            this._settings = settings.Value;
        }

        public async Task<bool> UploadFile(System.IO.Stream inputStream, string fileName)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = inputStream,
                    BucketName = _settings.AWSS3.BucketName,
                    Key = fileName
                };
                PutObjectResponse response = await _amazonS3.PutObjectAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ListVersionsResponse> FilesList(string bucketName)
        {
            return await _amazonS3.ListVersionsAsync(bucketName);
        }

        public async Task<ListBucketsResponse> BucketList()
        {
            return await _amazonS3.ListBucketsAsync();
        }

        public async Task<DetectLabelsResponse> RecognizeImage(string bucketName, string fileName)
        {
            var rekognitionClient = new AmazonRekognitionClient();
            return await rekognitionClient.DetectLabelsAsync(
                new DetectLabelsRequest
                {
                    Image = new Image
                    {
                        S3Object = new Amazon.Rekognition.Model.S3Object
                        {
                            Bucket = bucketName,
                            Name = fileName
                        }
                    }
                });
        }

        public async Task<Stream> GetFile(string key)
        {

            GetObjectResponse response = await _amazonS3.GetObjectAsync(_settings.AWSS3.BucketName, key);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return response.ResponseStream;
            else
                return null;
        }

        public async Task<bool> DeleteFile(string key)
        {
            try
            {
                DeleteObjectResponse response = await _amazonS3.DeleteObjectAsync(_settings.AWSS3.BucketName, key);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<bool> DeleteBucket(string key)
        {
            try
            {
                DeleteBucketResponse response = await _amazonS3.DeleteBucketAsync(key);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<bool> AddBucket(string key)
        {
            try
            {
                PutBucketResponse response = await _amazonS3.PutBucketAsync(key);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
