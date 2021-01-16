import { useState } from 'react';
import { Cupcake } from '../model';
import CupcakeCard from './cupcake-card';
import './cupcake-list.css';
import AddCupcake from './cupcake-new';

const CupcakeList = function (props: { cupcakes: Cupcake[] }) {
    return (
        <div>
            <div className="card-list">
                {props.cupcakes.map((cake) => (
                    <CupcakeCard key={cake.id} cupcake={cake} />
                ))}
            </div>
            <div className="card-list">
                <AddCupcake />
            </div>
        </div>
    );
};

export default CupcakeList;
