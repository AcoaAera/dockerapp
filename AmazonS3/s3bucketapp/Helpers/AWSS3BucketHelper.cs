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
        Task<bool> UploadFile(System.IO.Stream inputStream, string bucketName, string fileName);
        Task<ListVersionsResponse> FilesList(string bucketName);
        Task<ListBucketsResponse> BucketList();
        Task<Stream> GetFile(string bucketName, string key);
        Task<bool> DeleteFile(string bucketName, string key);
        Task<bool> DeleteBucket(string key);
        Task<bool> AddBucket(string key);
        Task<DetectLabelsResponse> RecognizeImage(string fileName);

    }

    public class AWSS3BucketHelper : IAWSS3BucketHelper
    {
        private readonly IAmazonS3 _amazonS3;

        public AWSS3BucketHelper(IAmazonS3 s3Client)
        {
            this._amazonS3 = s3Client;
        }

        public async Task<bool> UploadFile(System.IO.Stream inputStream, string bucketName, string fileName)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = inputStream,
                    BucketName = bucketName,
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

        public async Task<DetectLabelsResponse> RecognizeImage(string fileName)
        {
            Image image = new Image();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] data = null;
                data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
                image.Bytes = new MemoryStream(data);
            }


            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient();

            DetectLabelsRequest detectlabelsRequest = new DetectLabelsRequest()
            {
                Image = image,
                MaxLabels = 10,
                MinConfidence = 77F
            };

            DetectLabelsResponse detectLabelsResponse = await rekognitionClient.DetectLabelsAsync(detectlabelsRequest);
            return detectLabelsResponse;

        }

        public async Task<Stream> GetFile(string bucketName, string key)
        {

            GetObjectResponse response = await _amazonS3.GetObjectAsync(bucketName, key);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return response.ResponseStream;
            else
                return null;
        }

        public async Task<bool> DeleteFile(string bucketName, string key)
        {
            try
            {
                DeleteObjectResponse response = await _amazonS3.DeleteObjectAsync(bucketName, key);
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
