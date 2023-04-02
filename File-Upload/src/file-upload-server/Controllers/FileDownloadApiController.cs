namespace GraphQL.Aspnet.Examples.FileUpload.Controllers
{
    using GraphQL.Aspnet.Examples.FileUpload.Model;
    using GraphQL.AspNet.Common;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.StaticFiles;
    using System.IO;

    [Route("downloads")]
    public class FileDownloadApiController : Controller
    {
        private readonly UploadConfiguration _config;

        public FileDownloadApiController(UploadConfiguration config)
        {
            _config = Validation.ThrowIfNullOrReturn(config, nameof(config));
        }
        private string CreateExpectedLocalPath(string fileName)
        {
            // *******************************
            // Warning: this is just a sample project
            //          In production do not directly combine the received filename
            //          with local paths without some sort of checks.
            //          Doing this can allow an attacker to access folders beyond your
            //          original intent.
            // *******************************
            return Path.Combine(_config.UploadFolderPath, fileName);
        }

        [HttpGet("{fileName}")]
        public IActionResult Index(string fileName)
        {
            var path = this.CreateExpectedLocalPath(fileName);
            if (!System.IO.File.Exists(path))
                return this.NotFound();

            // return the file contents
            var stream = new FileStream(path, FileMode.Open);
            return new FileStreamResult(stream, this.GetContentType(path))
            {
                FileDownloadName = fileName,
            };
        }

        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(fileName, out string contentType))
            {
                return contentType;
            }

            return "application/octet-stream";
        }
    }
}
