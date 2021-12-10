namespace Firebase.AuthTest.Controllers

{
    using Firebase.AuthTest.Models;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [GraphRoute("bakery")]
    public class BakeryController : GraphController
    {
        private static readonly Random _rando = new Random();
        private List<Donut> _donuts;
        private List<Cookie> _cookies;

        public BakeryController()
        {
            _donuts = new List<Donut>();
            _donuts.Add(new Donut("Sabrina's Dream", "Vanilla"));
            _donuts.Add(new Donut("Death By Chocolate", "Chocolate"));
            _donuts.Add(new Donut("Strawberry Shortcake", "Strawberry"));

            _cookies = new List<Cookie>();
            _cookies.Add(new Cookie("Chocoholics", "Chocolate Chip"));
            _cookies.Add(new Cookie("Big Nuts", "Walnuts & Pecans"));
        }

        [QueryRoot("makeDonut")]
        [Authorize]
        public Donut CreateANewDonut()
        {
            var userId = this.User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
            var newDonut = _donuts[_rando.Next(0, 2)].MadeFor(userId);
            return newDonut;
        }

        [QueryRoot("makeCookie")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public Cookie RetrieveUserIdWithSchemeRestriction()
        {
            // Authorization to this method requires use of a "Cookie" authentication scheme
            // but the user was authenticated with a JWT token, the "Bearer" scheme, so they are denied
            //
            // Note that you may get an exception thrown and a 500 result indicating that Cookie authentication
            // is not even registered to the application.
            var userId = this.User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
            var newCookie = _cookies[_rando.Next(0, 1)].MadeFor(userId);
            return newCookie;
        }
    }
}