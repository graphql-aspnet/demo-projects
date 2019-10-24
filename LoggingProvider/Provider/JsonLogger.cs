namespace GraphQL.AspNet.Examples.LoggingProvider.Provider
{
    using System;
    using System.IO;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using GraphQL.AspNet.Interfaces.Logging;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// <para>A singleton log instance that will write and recieved <see cref="IGraphLogEntry"/>
    /// to an array stored at the given filename. If the file exists, it will be deleted and replaced.</para>
    ///
    /// <para>WARNING: This logger is for demonstration purposes only. DO NOT
    /// use this logger in a production environment.</para>
    /// </summary>
    public class JsonLogger : ILogger
    {
        private readonly string _fileName;
        private static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLogger"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public JsonLogger(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the t state.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState">The type of the t state.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!(state is IGraphLogEntry logEntry))
                return;

            // This provider is for demo purposes only
            // in production you'd want a more robust way to handle file I/O
            lock (locker)
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                options.Converters.Add(new LogEntryConverterFactory());

                var serializedEntry = JsonSerializer.Serialize(state, options);

                using (var stream = File.Open(_fileName, FileMode.OpenOrCreate))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        if (stream.Length > 0)
                            stream.Seek(0, SeekOrigin.End);

                        // write the opening brace for the array
                        // or if the file already has data, to the point
                        // of the close brace (so it will be overwritten)
                        // and append a commna
                        if (stream.Position == 0)
                        {
                            writer.WriteLine("[");
                        }
                        else
                        {
                            stream.Seek(-1, SeekOrigin.End);
                            writer.WriteLine(",");
                        }

                        writer.Write(serializedEntry);
                        writer.Write("]");
                    }
                }
            }
        }
    }
}