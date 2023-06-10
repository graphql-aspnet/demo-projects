namespace unit_tests
{
    using GraphQL.AspNet.Examples.TestApi.Controllers;
    using GraphQL.AspNet.Tests.Framework;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;
    using Xunit;

    public class PersonControllerTests
    {
        [Fact]
        public async Task RetrievePerson_ReturnsPersonWithSuppliedNames()
        {
            // ****Arrange****
            var builder = new TestServerBuilder();
            builder.AddGraphQL(o =>
            {
                o.AddController<PersonController>();
            });

            var server = builder.Build();

            var queryBuilder = server.CreateQueryContextBuilder();
            queryBuilder.AddQueryText(@"
                    query {
                        retrievePerson(firstName: ""Jane"" lastName: ""Doe"") {
                            firstName
                            lastName
                        }
                    }");

            var query = queryBuilder.Build();

            // ****Act****
            var result = await server.RenderResult(query);

            /* result contains the json string for:
            {
                "data": {
                "retrievePerson": {
                    "firstName": "Jane",
                    "lastName": "Doe"
                    }
                }
            };
            */

            // ****Assert****

            // deserialize result to an object
            var node = JsonNode.Parse(result);
            Assert.Equal("Jane", node["data"]["retrievePerson"]["firstName"].AsValue().ToString());
            Assert.Equal("Doe", node["data"]["retrievePerson"]["lastName"].AsValue().ToString());
        }

        [Fact]
        public async Task RetrieveVip_ForAuthorizedUser_ReturnsTheVip()
        {
            // ****Arrange****
            var builder = new TestServerBuilder();
            builder.AddGraphQL(o =>
            {
                o.AddController<PersonController>();
            });

            // configure a policy the server will support
            builder.Authorization.AddClaimPolicy("vipAccess", "accessLevel", "vip");

            // configure the user to both authenticate and
            // present the correct claims
            builder.UserContext.Authenticate();
            builder.UserContext.AddUserClaim("accessLevel", "vip");

            var server = builder.Build();

            var queryBuilder = server.CreateQueryContextBuilder();
            queryBuilder.AddQueryText(@"
                    query {
                        retrieveVip {
                            firstName
                            lastName
                        }
                    }");

            var query = queryBuilder.Build();

            // ****Act****
            // the user context configured on the builder is automatically
            // injected in the request
            var result = await server.RenderResult(query);

            /* result contains the json string for:
            {
                "data": {
                "retrieveVip": {
                    "firstName": "Bob",
                    "lastName": "Smith"
                    }
                }
            };
            */

            // ****Assert****

            // deserialize result to an object
            var node = JsonNode.Parse(result);
            Assert.Equal("Bob", node["data"]["retrieveVip"]["firstName"].AsValue().ToString());
            Assert.Equal("Smith", node["data"]["retrieveVip"]["lastName"].AsValue().ToString());
        }

        [Fact]
        public async Task RetrieveVip_ForUnauthorizedUser_ReturnsAccessDenied()
        {
            // ****Arrange****
            var builder = new TestServerBuilder();
            builder.AddGraphQL(o =>
            {
                o.AddController<PersonController>();
            });

            // configure a policy the server will support
            builder.Authorization.AddClaimPolicy("vipAccess", "accessLevel", "vip");

            // DO NOT configure the user to be authenticated
            // builder.UserContext.Authenticate();

            var server = builder.Build();

            var queryBuilder = server.CreateQueryContextBuilder();
            queryBuilder.AddQueryText(@"
                    query {
                        retrieveVip {
                            firstName
                            lastName
                        }
                    }");

            var query = queryBuilder.Build();

            // ****Act****
            // Execute the query and return the full result object
            // so we can inspect it
            await server.ExecuteQuery(query);

            // ****Assert****

            // inspect the query results for any error messages
            // there should be only one in this case
            Assert.Equal(1, query.Messages.Count);
            Assert.Equal("ACCESS_DENIED", query.Messages[0].Code);
        }
    }
}