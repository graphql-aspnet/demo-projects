namespace GraphQL.AspNet.Examples.StrawberryShakeServer.Controllers
{
    using System;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;

    /// <summary>
    /// A query controller allowing anyone to retrieve the current second value from the server.
    /// </summary>
    public class QueryController : GraphController
    {
        /// <summary>
        /// Returns the current time on the server. Provides a nice, ever changing 
        /// value to see repeated in the demo client.
        /// </summary>
        [QueryRoot("CurrentTime", typeof(TimeOnly))]
        public TimeOnly CurrentTime()
        {
            // round to the last second
            return TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(Math.Round(DateTime.UtcNow.TimeOfDay.TotalSeconds)));
        }
    }
}
