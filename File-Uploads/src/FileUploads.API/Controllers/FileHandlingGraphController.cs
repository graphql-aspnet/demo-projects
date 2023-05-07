namespace GraphQL.Aspnet.Examples.FileUploadMultipartRequest.APIControllers
{
    using GraphQL.Aspnet.Examples.MultipartRequest.API.Model;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Common;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Interfaces.Controllers;
    using GraphQL.AspNet.ServerExtensions.MultipartRequests.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class FileHandlingGraphController : GraphController
    {
        private readonly UploadConfiguration _config;

        public FileHandlingGraphController(UploadConfiguration config)
        {
            _config = Validation.ThrowIfNullOrReturn(config, nameof(config));
        }

        private string CreateExpectedLocalPath(string fileName)
        {
            // *******************************
            // WARNING:
            // This is just a sample project
            // In production do not directly combine the received filename
            // with local paths without some sort of checks.
            // Doing this can allow an attacker to access folders beyond your
            // original intent.
            // *******************************
            return Path.Combine(_config.UploadFolderPath, fileName);
        }

        private string CreateDownloadUrl(string fileName)
        {
            var url = _config.DownloadFolderUrlBase;
            if(!url.EndsWith("/"))
                url += "/";
            url += fileName;
            return url;
        }

        [MutationRoot("singleUpload", typeof(FileStatistics))]
        public async Task<IGraphActionResult> UploadFile(FileUpload file)
        {
            if (file == null)
                return this.Error("No File received.");

            var localPath = this.CreateExpectedLocalPath(file.FileName);
            if (File.Exists(localPath))
                return this.Error($"Operation Failed. A file with the name '{file.FileName}' already exists.");

            await this.SaveFileToDisk(localPath, file);

            var stats = new FileStatistics()
            {
                Id = Guid.NewGuid().ToString(),
                FileName = file.FileName,
                Url = this.CreateDownloadUrl(file.FileName),
            };

            return this.Ok(stats);
        }

        [MutationRoot("multipleUpload", typeof(IEnumerable<FileStatistics>))]
        public async Task<IGraphActionResult> UploadManyFiles(IList<FileUpload> files)
        {
            var results = new List<FileStatistics>();
            if (files == null || files.Any(x => x == null))
                return this.Error($"Operation Failed. All provided file instances must be valid files.");


            // check that all files can be written to disk
            // before writing
            foreach (var file in files)
            {
                var path = this.CreateExpectedLocalPath(file.FileName);
                if (File.Exists(path))
                    return this.Error($"Operation Failed. A file with the name '{file.FileName}' already exists.");
            }

            // write files to disk
            foreach (var file in files)
            {
                var localPath = this.CreateExpectedLocalPath(file.FileName);
                await this.SaveFileToDisk(localPath, file);

                results.Add(new FileStatistics()
                {
                    Id = file.FileName,
                    FileName = file.FileName,
                    Url = this.CreateDownloadUrl(file.FileName),
                });
            }

            return this.Ok(results);
        }

        [QueryRoot("uploads")]
        public IEnumerable<FileStatistics> RetrieveAllFiles()
        {
            var files = new DirectoryInfo(_config.UploadFolderPath);
            var results = new List<FileStatistics>();

            foreach (var file in files.GetFiles())
            {
                results.Add(new FileStatistics()
                {
                    Id = file.Name,
                    FileName = file.Name,
                    Url = this.CreateDownloadUrl(file.Name),
                });
            }

            return results;
        }

        private async Task SaveFileToDisk(string localPath, FileUpload file)
        {
            using var fileInStream = await file.OpenFileAsync();
            using var fileOutStream = File.OpenWrite(localPath);
            await fileInStream.CopyToAsync(fileOutStream);
        }
    }
}