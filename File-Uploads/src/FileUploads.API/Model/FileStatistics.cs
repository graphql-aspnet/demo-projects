﻿namespace GraphQL.Aspnet.Examples.MultipartRequest.API.Model
{
    using GraphQL.AspNet.Attributes;

    [GraphType("File")]
    public class FileStatistics
    {
        public string Id { get; set; }

        [GraphField("name")]
        public string FileName { get; set; }

        public string Url { get; set; }
    }
}