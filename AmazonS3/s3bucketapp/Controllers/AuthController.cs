using Microsoft.AspNetCore.Mvc;
using s3bucketapp.Models;
using s3bucketapp.Services;
using System.Threading.Tasks;

namespace s3bucketapp.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    [ApiController]
    public class AWSS3FileController : ControllerBase
    {
        private readonly IAWSS3FileService _AWSS3FileService;
        public AWSS3FileController(IAWSS3FileService AWSS3FileService)
        {
            this._AWSS3FileService = AWSS3FileService;
        }
        [Route("uploadFile")]
        [HttpPost]
        public async Task<IActionResult> UploadFileAsync(string bucketName, UploadFileName uploadFileName)
        {
            var result = await _AWSS3FileService.UploadFile(bucketName, uploadFileName);
            return Ok(new { isSucess = result });
        }
        [Route("filesList")]
        [HttpGet]
        public async Task<IActionResult> FilesListAsync(string bucketName)
        {
            var result = await _AWSS3FileService.FilesList(bucketName);
            return Ok(result);
        }

        [Route("getFile/{fileName}")]
        [HttpGet]
        public async Task<IActionResult> GetFile(string bucketName, string fileName)
        {
            try
            {
                var result = await _AWSS3FileService.GetFile(bucketName, fileName);
                return File(result, "image/png");
            }
            catch
            {
                return Ok("NoFile");
            }

        }
        [Route("updateFile")]
        [HttpPut]
        public async Task<IActionResult> UpdateFile(UploadFileName uploadFileName, string bucketName, string fileName)
        {
            var result = await _AWSS3FileService.UpdateFile(uploadFileName, bucketName, fileName);
            return Ok(new { isSucess = result });
        }

        [Route("deleteFile/{fileName}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFile(string bucketName, string fileName)
        {
            var result = await _AWSS3FileService.DeleteFile(bucketName, fileName);
            return Ok(new { isSucess = result });
        }


        [Route("addBucket/{bucketName}")]
        [HttpPost]
        public async Task<IActionResult> AddBucket(string bucketName)
        {
            var result = await _AWSS3FileService.AddBucket(bucketName);
            return Ok(new { isSucess = result });
        }

        [Route("bucketsList")]
        [HttpGet]
        public async Task<IActionResult> BucketsListAsync()
        {
            var result = await _AWSS3FileService.BucketList();
            return Ok(result);
        }

        [Route("deleteBucket/{bucketName}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteBucket(string bucketName)
        {
            var result = await _AWSS3FileService.DeleteBucket(bucketName);
            return Ok(new { isSucess = result });
        }

        [Route("regognizeImage")]
        [HttpGet]
        public async Task<IActionResult> RegognizeImageAsync(string fileName)
        {
            var result = await _AWSS3FileService.RecognizeImage(fileName);
            return Ok(result);
        }
    }
}
