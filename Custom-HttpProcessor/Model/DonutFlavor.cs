namespace GraphQL.AspNet.Examples.CustomHttpProcessor.Model
{
    using System.ComponentModel;

    /// <summary>
    /// The possible flavor types of donuts
    /// </summary>
    [Description("One of the flavors of donut this bakery is capable of making")]
    public enum DonutFlavor
    {
        Sugar,
        Chocolate,
        Plain,
        Glazed
    }
}