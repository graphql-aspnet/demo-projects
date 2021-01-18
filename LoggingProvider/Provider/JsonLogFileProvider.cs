namespace GraphQL.AspNet.Examples.LoggingProvider.Provider
{
    using GraphQL.AspNet.Interfaces.Logging;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;

    /// <summary>
    /// <para>A logging provider that will record any <see cref="IGraphLogEntry"/> to
    /// a file in the provided folder. All other log messages are ignored.</para>
    ///
    /// <para>WARNING: This log provider is for demonstration purposes only. DO NOT
    /// use this log provider in a production environment.</para>
    /// </summary>
    public class JsonLogFileProvider : ILoggerProvider
    {
        private string _fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLogFileProvider" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file. If not provided a unique filename will be created.</param>
        public JsonLogFileProvider(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Directory.Exists)
            {
                throw new ArgumentException($"The folder '{fileInfo.Directory.FullName}' does not exist. No log files can be written.");
            }

            _fileName = fileName;
        }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>Microsoft.Extensions.Logging.ILogger.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new JsonLogger(_fileName);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="isDisposing">A value indicating if disposing of resources is indicated.</param>
        protected virtual void Dispose(bool isDisposing)
        {
        }
    }
}