import { Cupcake, PastryFlavor } from '../model';
import './cupcake-card.css';
import ChocolateImage from '../images/chocolate.svg';
import VanillaImage from '../images/vanilla.svg';
import StrawberryImage from '../images/strawberry.svg';
import PlainImage from '../images/plain.svg';

type ComponentProps = {
    cupcake: Cupcake;
    onCupcakePurchased: (cupcake: number) => void;
};

const CupcakeCard = function (props: ComponentProps) {
    const item = props.cupcake;

    let imageData: string;
    switch (item.flavor) {
        case PastryFlavor.Chocolate:
            imageData = ChocolateImage;
            break;
        case PastryFlavor.Vanilla:
            imageData = VanillaImage;
            break;
        case PastryFlavor.Strawberry:
            imageData = StrawberryImage;
            break;
        default:
            imageData = PlainImage;
            break;
    }

    return (
        <div className="card">
            <div className="card-image">
                <img src={imageData} alt={item.name} />
            </div>
            <div className="card-details">
                <div className="card-name">{item.name}</div>
                <div className="card-row card-flavor">
                    <div className="card-column item-title">Flavor:</div>
                    <div className="card-column">
                        {item.flavor.charAt(0).toUpperCase() +
                            item.flavor.slice(1).toLowerCase()}
                    </div>
                </div>
                <div className="card-row card-quantity">
                    <div className="card-column item-title">Remaining:</div>
                    <div className="card-column">{item.quantity}</div>
                </div>

                <div className="purchaseButton">
                    {item.quantity > 0 ? (
                        <button
                            onClick={(e) => {
                                e.preventDefault();
                                props.onCupcakePurchased(item.id);
                            }}
                        >
                            Purchase
                        </button>
                    ) : (
                        <span>Sold Out :(</span>
                    )}
                </div>
            </div>
        </div>
    );
};

export default CupcakeCard;
