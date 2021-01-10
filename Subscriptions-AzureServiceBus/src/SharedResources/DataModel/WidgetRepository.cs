namespace GraphQL.AspNet.Examples.Subscriptions.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    /// <summary>
    /// This sample repository is for demo purposes only. In reality, this
    /// data would come from a database.
    /// </summary>
    public class WidgetRepository
    {
        private static List<Widget> _widgetData;

        static WidgetRepository()
        {
            _widgetData = new List<Widget>();
            _widgetData.Add(new Widget()
            {
                Id = 1,
                Name = "First Widget",
                Description = "The first widget ever created",
            });

            _widgetData.Add(new Widget()
            {
                Id = 2,
                Name = "Second Widget",
                Description = "A second widget is better than the first",
            });

            _widgetData.Add(new Widget()
            {
                Id = 3,
                Name = "Third Widget",
                Description = "Still in the top three, third widget is optimistic",
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetRepository"/> class.
        /// </summary>
        public WidgetRepository()
        {
        }

        /// <summary>
        /// Searches for any widgets with a name starting with the given letters.
        /// </summary>
        /// <param name="searchText">The letters to search for.</param>
        /// <returns>IEnumerable&lt;Widget&gt;.</returns>
        public IEnumerable<Widget> SearchByName(string searchText = "*")
        {
            lock (_widgetData)
            {
                if (searchText == "*")
                    return _widgetData.ToList();

                return _widgetData
                    .Where(x => x.Name.StartsWith(searchText))
                    .ToList();
            }
        }

        /// <summary>
        /// Searches for any widgets with a description starting with the given letters.
        /// </summary>
        /// <param name="searchText">The letters to search for.</param>
        /// <returns>IEnumerable&lt;Widget&gt;.</returns>
        public IEnumerable<Widget> SearchByDescription(string searchText = "*")
        {
            lock (_widgetData)
            {
                if (searchText == "*")
                    return _widgetData.ToList();

                return _widgetData
                    .Where(x => x.Description.StartsWith(searchText))
                    .ToList();
            }
        }

        /// <summary>
        /// Finds a single widget by the given identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Widget or null.</returns>
        public Widget FindById(int id)
        {
            lock (_widgetData)
            {
                return _widgetData.SingleOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Inserts the widget if its id does not exist in the master list
        /// or updates the name and description of an existing widget with the same
        /// id.
        /// </summary>
        /// <param name="newData">The new data to add/update.</param>
        public void InsertOrUpdateWidget(Widget newData)
        {
            lock (_widgetData)
            {
                var item = _widgetData.SingleOrDefault(x => x.Id == newData.Id);
                if (item == null)
                {
                    _widgetData.Add(item);
                }
                else
                {
                    item.Name = newData.Name;
                    item.Description = newData.Description;
                }
            }
        }
    }
}
