using Amazon.S3.Model;
using s3bucketapp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using s3bucketapp.Models;
using System.Security.Cryptography.X509Certificates;

namespace s3bucketapp.Services
{
    public interface IAWSS3FileService
    {
        Task<bool> UploadFile(string bucketName, UploadFileName uploadFileName);
        Task<List<string>> FilesList(string bucketName);
        Task<List<string>> BucketList();
        Task<Stream> GetFile(string bucketName, string key);
        Task<bool> UpdateFile(UploadFileName uploadFileName, string bucketName, string key);
        Task<bool> DeleteFile(string bucketName, string key);
        Task<bool> DeleteBucket(string key);
        Task<bool> AddBucket(string key);
        Task<List<string>> RecognizeImage(string fileName);
    }

    public class AWSS3FileService : IAWSS3FileService
    {
        private readonly IAWSS3BucketHelper _AWSS3BucketHelper;

        public AWSS3FileService(IAWSS3BucketHelper AWSS3BucketHelper)
        {
            this._AWSS3BucketHelper = AWSS3BucketHelper;
        }
        public async Task<bool> UploadFile(string bucketName, UploadFileName uploadFileName)
        {
            try
            {
                var path = Path.Combine("Files", uploadFileName.ToString() + ".png");
                using (FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    string fileExtension = Path.GetExtension(path);
                    string fileName = string.Empty;
                    fileName = $"{DateTime.Now.Ticks}{fileExtension}";
                    return await _AWSS3BucketHelper.UploadFile(fsSource, bucketName, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<string>> FilesList(string bucketName)
        {
            try
            {
                ListVersionsResponse listVersions = await _AWSS3BucketHelper.FilesList(bucketName);
                return listVersions.Versions.Select(c => c.Key).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<string>> BucketList()
        {
            try
            {
                ListBucketsResponse listBuckets = await _AWSS3BucketHelper.BucketList();
                return listBuckets.Buckets.Select(c=>c.BucketName).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Stream> GetFile(string bucketName, string key)
        {
            try
            {
                Stream fileStream = await _AWSS3BucketHelper.GetFile(bucketName, key);
                if (fileStream == null)
                {
                    Exception ex = new Exception("File Not Found");
                    throw ex;
                }
                else
                {
                    return fileStream;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateFile(UploadFileName uploadFileName, string bucketName, string key)
        {
            try
            {
                var path = Path.Combine("Files", uploadFileName.ToString() + ".png");
                using (FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return await _AWSS3BucketHelper.UploadFile(fsSource, bucketName, key);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteFile(string bucketName, string key)
        {
            try
            {
                return await _AWSS3BucketHelper.DeleteFile(bucketName, key);
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
                return await _AWSS3BucketHelper.DeleteBucket(key);
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
                return await _AWSS3BucketHelper.AddBucket(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> RecognizeImage(string fileName)
        {
            try
            {
                var detectList = await _AWSS3BucketHelper.RecognizeImage(fileName);
                var list = new List<string>();
                foreach (var item in detectList.Labels)
                    list.Add(item.Name + "," + item.Confidence);

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
