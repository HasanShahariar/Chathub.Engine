using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ChatHub.Web.Controllers.Helper
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileStreamController : ControllerBase
    {

        private readonly IConfiguration _config;

        public FileStreamController(IConfiguration config)
        {
            _config = config;
        }



        [HttpGet]
        [Route("streaming")]
        public IActionResult GetVideoContent(string fileName)
        {
            if (fileName != null && fileName != "")
            {
                string fullPath = Path.Combine(_config.GetValue<string>("FileUploads:RootPath"), fileName);

                //string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

                FileInfo fileInfo = new FileInfo(fullPath);
                var contentType = GetContentType(fullPath);
                if (fileInfo.Exists)
                {
                    FileStream fRead = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    FileStreamResult result = File(
                        fileStream: fRead,
                        contentType: new System.Net.Http.Headers.MediaTypeHeaderValue(contentType).MediaType,
                        enableRangeProcessing: true //<-- enable range requests processing
                    );
                    return result;
                }

                return NotFound();
            }

            return new NoContentResult();
        }

        private string GetContentType(string fileName)
        {
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            return contentType ?? "application/octet-stream";
        }

    }
}