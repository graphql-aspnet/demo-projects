namespace GraphQL.AspNet.Examples.ReactApollo.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GraphQL.AspNet.Common.Generics;
    using Microsoft.JSInterop.Infrastructure;

    public class CupcakeService
    {
        private static ConcurrentList<Cupcake> _cupCakeRepo;

        static CupcakeService()
        {
            _cupCakeRepo = new ConcurrentList<Cupcake>();

            _cupCakeRepo.Add(new Cupcake("Frosted Delight", PastryFlavor.Strawberry, 15));
            _cupCakeRepo.Add(new Cupcake("Plain Jane", PastryFlavor.Plain, 5));
            _cupCakeRepo.Add(new Cupcake("Chocolate Coma", PastryFlavor.Chocolate, 10));
        }

        public Cupcake RetrieveCupCake(int id)
        {
            return _cupCakeRepo.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Cupcake> SearchCupCakes(string nameLike = null, PastryFlavor? flavor = null)
        {
            IEnumerable<Cupcake> cakes = null;
            if (nameLike != null && nameLike != "*")
                cakes = _cupCakeRepo.Where(x => x.Name.StartsWith(nameLike, StringComparison.OrdinalIgnoreCase));

            if (flavor.HasValue)
                cakes = (cakes ?? _cupCakeRepo).Where(x => x.Flavor == flavor);

            return (cakes ?? _cupCakeRepo).ToList();
        }

        public Cupcake CreateCupcake(string name, PastryFlavor flavor, int quantity)
        {
            var newCake = new Cupcake(name, flavor, quantity);
            _cupCakeRepo.Add(newCake);
            return newCake;
        }

        public Cupcake UpdateCupcake(int id, string name, PastryFlavor flavor)
        {
            var foundCake = _cupCakeRepo.Single(x => x.Id == id);
            if (foundCake == null)
                return null;

            foundCake.Name = name;
            foundCake.Flavor = flavor;
            return foundCake;
        }

        public bool SellSingleCupcake(int id)
        {
            var foundCake = _cupCakeRepo.Single(x => x.Id == id);
            if (foundCake == null)
                return false;

            if (foundCake.Quantity == 0)
                return false;

            foundCake.Quantity -= 1;
            return true;
        }
    }
}
