import { useState, ChangeEvent } from "react";
import Book from "../models/Book";
import BookService from "../services/BookService";
import InputText from "./InputText";
import BookDetails from "./BookDetails";

export default function AddBook() {
    const initialBookState = {
        id: null,
        title: "",
        description: "",
        price: 0,
        genre : "",
        author : ""
    };
    const [book, setBook] = useState<Book>(initialBookState);
    const [submitted, setSubmitted] = useState<boolean>(false);

    const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setBook({ ...book, [name]: value });
    };

    const saveBook = () => {
        var data = {
            title: book.title,
            description: book.description,
            price: book.price,
            genre : book.genre,
            author : book.author
        };

        BookService.create(data)
            .then((response: any) => {
                setBook({
                    id: response.data.id,
                    title: response.data.title,
                    description: response.data.description,
                    price: response.data.price,
                    author : response.data.author,
                    genre : response.data.genre
                    
                });
                setSubmitted(true);
                console.log(response.data);
            })
            .catch((e: Error) => {
                console.log(e);
            });
    };

    const newBook = () => {
        setBook(initialBookState);
        setSubmitted(false);
    };

    return (
        <div className="submit-form">
            {submitted ? (
                <div>
                    <h4>You submitted successfully!</h4>
                    <button className="btn btn-success" onClick={newBook}>
                        Add
                    </button>
                </div>
            ) : (
                <div>
                    <BookDetails book={book} handleInputChange={handleInputChange}/>

                    <button onClick={saveBook} className="btn btn-success">
                        Submit
                    </button>
                </div>
            )}
        </div>
    )
}