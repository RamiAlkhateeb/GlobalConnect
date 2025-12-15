export default interface Book {
    id?: number | null,
    title: string,
    author: string,
    genre: string,
    description: string;
    price: number,
  }