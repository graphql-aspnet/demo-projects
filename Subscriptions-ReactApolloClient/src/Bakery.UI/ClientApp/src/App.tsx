import React from 'react';
import './App.css';

import CupcakeList from './components/cupcake-list';
import { Cupcake, PastryFlavor } from './model';

import { ApolloClient, ApolloProvider } from '@apollo/client';
import { InMemoryCache, HttpLink } from '@apollo/client';
import { split } from '@apollo/client';
import { WebSocketLink } from '@apollo/client/link/ws';
import { getMainDefinition } from '@apollo/client/utilities';
import Bakery from './components/bakery';

const httpLink = new HttpLink({
    uri: 'http://localhost:5001/graphql',
});

const wsLink = new WebSocketLink({
    uri: `ws://localhost:5001/graphql`,
    options: {
        reconnect: true,
    },
});

// create a split network link to use web sockets for subscriptions
// and http for query and mutations
const splitLink = split(
    ({ query }) => {
        const definition = getMainDefinition(query);
        return (
            definition.kind === 'OperationDefinition' &&
            definition.operation === 'subscription'
        );
    },
    wsLink,
    httpLink
);

const client = new ApolloClient({
    cache: new InMemoryCache(),
    link: splitLink,
});

function App() {
    return (
        <ApolloProvider client={client}>
            <Bakery />
        </ApolloProvider>
    );
}

export default App;
