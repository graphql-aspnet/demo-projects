export enum PastryFlavor {
    Vanilla = 'VANILLA',
    Chocolate = 'CHOCOLATE',
    Strawberry = 'STRAWBERRY',
    Plain = 'PLAIN',
}

export interface Cupcake {
    id: number;
    name: string;
    flavor: PastryFlavor;
    quantity: number;
}
