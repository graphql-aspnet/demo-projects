namespace GraphQL.AspNet.Examples.Subscriptions.DataModel
{
    using System.ComponentModel.DataAnnotations;

    public class Widget
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
    }
}
