import { useState } from 'react';
import { Cupcake } from '../model';
import CupcakeCard from './cupcake-card';
import './cupcake-list.css';
import AddCupcake from './cupcake-new';

type ComponentProps = {
    cupcakes: Cupcake[];
    onCupcakeCreated: (cupcake: Cupcake) => void;
    onCupcakePurchased: (cupcake: number) => void;
};

const CupcakeList = function (props: ComponentProps) {
    return (
        <div>
            <div className="card-list">
                {props.cupcakes.map((cake) => (
                    <CupcakeCard
                        key={cake.id}
                        cupcake={cake}
                        onCupcakePurchased={props.onCupcakePurchased}
                    />
                ))}
            </div>
            <div className="card-list">
                <AddCupcake onCupcakeCreated={props.onCupcakeCreated} />
            </div>
        </div>
    );
};

export default CupcakeList;
