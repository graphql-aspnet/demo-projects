namespace GraphQL.AspNet.Examples.Subscriptions.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class WidgetConstants
    {
        /// <summary>
        /// The name of the internal event used to notify any listening subscriptions
        /// that a widget was altered by mutation.
        /// </summary>
        public const string WIDGET_INSERTED_OR_ADDED = "WIDGET_INSERTED_OR_ADDED";
    }
}
