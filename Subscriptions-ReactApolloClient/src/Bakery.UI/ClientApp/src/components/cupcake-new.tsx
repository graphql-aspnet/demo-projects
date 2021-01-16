import React, { ChangeEvent, SyntheticEvent, useState } from 'react';
import { Cupcake, PastryFlavor } from '../model';
import './cupcake-card.css';
import { gql, useMutation } from '@apollo/client';
import { resultKeyNameFromField } from '@apollo/client/utilities';

const ADD_CUPCAKE = gql`
    mutation AddCupcake($cupcake: Cupcake_Input) {
        addCupcake(cupcake: $cupcake) {
            id
            name
            quantity
            flavor
        }
    }
`;

const createCupcake = function (addMethod: any, cakeToAdd: Cupcake) {
    addMethod({ variables: { cupcake: cakeToAdd } });
};

const AddCupCake = function () {
    const emptyCake: Cupcake = {
        id: -1,
        name: '',
        flavor: PastryFlavor.Vanilla,
        quantity: 0,
    };

    const [state, setState] = useState(emptyCake);
    const [addCupcakeMutation, {}] = useMutation(ADD_CUPCAKE);

    const onInputChange = function (
        event: ChangeEvent<HTMLInputElement | HTMLSelectElement>
    ) {
        const target = event.target;
        let value;
        if (target.name == 'quantity') {
            value = parseInt(target.value);
        } else {
            value = target.value;
        }
        const newState = { ...state, [target.name]: value };
        setState(newState);
    };

    const resetForm = function () {
        setState(emptyCake);
    };

    return (
        <div className="card">
            <div className="card-details">
                <div className="card-name">Bake a New Batch!</div>
                <form
                    onSubmit={(e) => {
                        e.preventDefault();
                        createCupcake(addCupcakeMutation, state);
                        resetForm();
                    }}
                >
                    <div className="card-row card-flavor">
                        <div className="card-column item-title">Name:</div>
                        <div className="card-column">
                            <input
                                name="name"
                                onChange={onInputChange}
                                placeholder="Name"
                                maxLength={50}
                                value={state.name}
                            ></input>
                        </div>
                    </div>
                    <div className="card-row card-flavor">
                        <div className="card-column item-title">Flavor:</div>
                        <div className="card-column">
                            <select
                                name="flavor"
                                onChange={onInputChange}
                                value={state.flavor}
                            >
                                {Object.keys(PastryFlavor).map((flav) => (
                                    <option value={flav} key={flav}>
                                        {flav.charAt(0).toUpperCase() +
                                            flav.slice(1).toLowerCase()}
                                    </option>
                                ))}
                            </select>
                        </div>
                    </div>
                    <div className="card-row card-quantity">
                        <div className="card-column item-title">Quantity:</div>
                        <div className="card-column">
                            <input
                                onChange={onInputChange}
                                name="quantity"
                                type="text"
                                maxLength={2}
                                value={state.quantity}
                            ></input>
                        </div>
                    </div>

                    <div className="purchaseButton">
                        <button type="submit">Bake It!</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default AddCupCake;
