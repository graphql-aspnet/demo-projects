# Bakery Subscriptions Demo

## Technology Used
```
* GraphQL Asp.Net Subscriptions
* ReactJS (w/ vite)
* Apollo Client
```

This project demonstrates the capabilities of the GraphQL ASP.NET subscription server running in process. 

### Launching the demo

**Open and Start the API**
1. In visual studio, Rider or other IDE of choice open the solution: '`Subscriptions-ReactApolloClient.slnx`
2. Start the solution which should launch the single `Bakery.API` project
4. Run the solution
    * Server runs at `localhost:5000`

**Open and Start the UI**
1. Open `src/Bakery.UI` in your favorite javascript IDE (VS Code, Web Storm etc.)
2. Run `npm install` from the command line
3. Run `npm start` from the command line
    * Client runs at `localhost:3000` and already points to the default server port. 