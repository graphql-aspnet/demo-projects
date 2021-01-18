import React, { useEffect, useState } from 'react';
import { Cupcake } from '../model';
import CupcakeList from './cupcake-list';
import { gql, useMutation, useQuery, useSubscription } from '@apollo/client';

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

const ADD_CUPCAKE = gql`
    mutation AddCupcake($cupcake: Cupcake_Input) {
        addCupcake(cupcake: $cupcake)
    }
`;
const PURCHASE_CUPCAKE = gql`
    mutation BuyCupcake($id: Int!) {
        purchaseCupcake(id: $id)
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

type CupcakeState = {
    cupcakeList: Cupcake[];
};

const Bakery = () => {
    // Initially load the local bakery with no cupcakes
    const initialState: CupcakeState = {
        cupcakeList: [],
    };

    // keep track of the current bakery inventory
    const [state, setCupcakes] = useState(initialState);

    // setup a query to fetch the current cupcake list from the server
    // when complete, update the local inventory with the new list
    const queryRequest = useQuery(GET_CUPCAKES, {
        onCompleted: (data) => {
            const newState: CupcakeState = {
                cupcakeList: [...state.cupcakeList, ...data.cupcakes.search],
            };
            setCupcakes(newState);
        },
    });

    // setup two mutation's to handle when someone purchases a cupcake
    // or creates a new one on the remote bakery inventory
    const [purchaseCupcake] = useMutation(PURCHASE_CUPCAKE);
    const [addNewCupcake] = useMutation(ADD_CUPCAKE);

    // setup two subscriptions to listen for newly created cupcakes
    // and recently purchased cupcakes on the remote bakery inventory
    const purchaseSub = useSubscription(ON_CUPCAKE_PURCHASED, {
        onSubscriptionData: (options: any) => {
            const updatedCupcake =
                options.subscriptionData?.data?.onCupcakeSold;
            if (updatedCupcake != null) {
                // update the state held cupcake with the new details from the server
                const newList = state.cupcakeList.map((x) => {
                    if (x.id == updatedCupcake.id) return updatedCupcake;
                    return x;
                });

                const newState: CupcakeState = {
                    cupcakeList: newList,
                };
                setCupcakes(newState);
            }
        },
    });

    const addSub = useSubscription(ON_CUPCAKE_CREATED, {
        onSubscriptionData: (options: any) => {
            const newCupcake = options.subscriptionData?.data?.onCupcakeCreated;
            if (newCupcake != null) {
                // add new the cupcake to the end of the list
                const newList = [...state.cupcakeList, newCupcake];
                const newState: CupcakeState = {
                    cupcakeList: newList,
                };
                setCupcakes(newState);
            }
        },
    });

    // two local functions (based to child components)
    // to handle purhcases and creation of new cupcakes
    const onCupcakeCreated = (cupcake: Cupcake) => {
        addNewCupcake({ variables: { cupcake: cupcake } });
    };

    const onCupcakePurchased = (id: number) => {
        purchaseCupcake({ variables: { id: id } });
    };

    // render out the inventory screen
    return (
        <div>
            <h2>Sample Bakery</h2>
            <span>
                Open the website on another browser window then purchase a
                cupcake. Watch the available quantity change automatically via
                the registered subscriptions...
            </span>
            {queryRequest.loading ? (
                <p>Loading Initial Bakery Inventory. Please Wait...</p>
            ) : queryRequest.error ? (
                <p>{'ERROR: ' + queryRequest.error}</p>
            ) : (
                <CupcakeList
                    cupcakes={state.cupcakeList}
                    onCupcakeCreated={onCupcakeCreated}
                    onCupcakePurchased={onCupcakePurchased}
                />
            )}
        </div>
    );
};

export default Bakery;
