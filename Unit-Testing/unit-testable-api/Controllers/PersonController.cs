namespace GraphQL.AspNet.Examples.TestApi.Controllers
{
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Examples.TestApi.Model;
    using Microsoft.AspNetCore.Authorization;

    public class PersonController : GraphController
    {
        [QueryRoot]
        public Person RetrievePerson(string firstName, string lastName)
        {
            return new Person
            {
                FirstName = firstName,
                LastName = lastName,
            };
        }

        [QueryRoot]
        [Authorize(Policy = "vipAccess")]
        public Person RetrieveVip()
        {
            return new Person()
            {
                FirstName = "Bob",
                LastName = "Smith",
            };
        }
    }
}