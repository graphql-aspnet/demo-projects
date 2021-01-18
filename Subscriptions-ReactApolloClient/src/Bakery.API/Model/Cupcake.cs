namespace GraphQL.AspNet.Examples.ReactApollo.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using GraphQL.AspNet.Attributes;

    [GraphType("Cupcake", InputName = "Cupcake_Input")]
    public class Cupcake
    {
        private static object _syncLock = new object();
        private static int _nextId = 1;

        public Cupcake()
        {
            this.Id = -1;
        }

        public Cupcake(string name, PastryFlavor flavor, int quantity)
        {
            lock(_syncLock)
                this.Id = _nextId++;

            this.Name = name;
            this.Flavor = flavor;
            this.Quantity = quantity;
        }

        public int? Id { get; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public PastryFlavor Flavor { get; set; }

        public int Quantity { get; set; }
    }
}
