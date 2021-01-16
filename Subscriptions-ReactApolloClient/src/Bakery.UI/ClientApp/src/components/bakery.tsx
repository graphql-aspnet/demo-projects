import React, { useState } from 'react';
import { Cupcake } from '../model';
import './cupcake-list.css';
import CupcakeList from './cupcake-list';
import { gql, useQuery, useSubscription } from '@apollo/client';

const GET_CUPCAKES = gql`
    query RetrieveCupcakes {
        cupcakes {
            search(nameLike: "*") {
                id
                name
                flavor
                quantity
            }
        }
    }
`;

const ON_CUPCAKE_PURCHASED = gql`
    subscription {
        onCupcakeSold {
            id
            name
            flavor
            quantity
        }
    }
`;

const ON_CUPCAKE_CREATED = gql`
    subscription {
        onCupcakeCreated {
            id
            name
            flavor
            quantity
        }
    }
`;

const Bakery = function () {
    const queryResult = useQuery(GET_CUPCAKES);
    const purchaseResult = useSubscription(ON_CUPCAKE_PURCHASED);
    const createResult = useSubscription(ON_CUPCAKE_CREATED);

    if (queryResult.loading) {
        return <p>loading...</p>;
    } else if (queryResult.error) {
        console.log(queryResult.error);
        return <p>Failed: {queryResult.error.message}</p>;
    }

    return (
        <div>
            <h2>Sample Bakery</h2>
            <span>
                Open the website on another browser window then purchase a
                cupcake. Watch the available quantity change automatically via
                the registered subscriptions...
            </span>
            <CupcakeList cupcakes={queryResult.data.cupcakes.search} />
        </div>
    );
};

export default Bakery;
