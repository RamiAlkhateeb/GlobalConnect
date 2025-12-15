import { ChangeEvent, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Book from "../models/Book";
import BookService from "../services/BookService";
import BookDetails from "./BookDetails";

export default function UpdateBook() {

    const { id } = useParams();
    let navigate = useNavigate();

    const initialBookState = {
        id: null,
        title: "",
        description: "",
        price: 0,
        author: "",
        genre: ""
    };
    const [currentBook, setCurrentBook] = useState<Book>(initialBookState);
    const [message, setMessage] = useState<string>("");

    const getBook = (id: string) => {
        BookService.get(id)
            .then((response: any) => {
                setCurrentBook(response.data);
                console.log(response.data);
            })
            .catch((e: Error) => {
                console.log(e);
            });
    };

    useEffect(() => {
        if (id)
            getBook(id);
    }, [id]);

    const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setCurrentBook({ ...currentBook, [name]: value });
    };

    const updateBook = () => {
        BookService.update(currentBook.id, currentBook)
            .then((response: any) => {
                console.log(response.data);
                setMessage("The book was updated successfully!");
            })
            .catch((e: Error) => {
                console.log(e);
            });
    };

    const deleteBook = () => {
        BookService.remove(currentBook.id)
            .then((response: any) => {
                console.log(response.data);
                navigate("/books");
            })
            .catch((e: Error) => {
                console.log(e);
            });
    };
    return (
        <div>
            {currentBook ? (
                <div className="edit-form">
                    <h4>Book</h4>
                    <form>
                        <BookDetails book={currentBook} handleInputChange={handleInputChange} />

                    </form>



                    <button className="badge badge-danger mr-2" onClick={deleteBook}>
                        Delete
                    </button>

                    <button
                        type="submit"
                        className="badge badge-success"
                        onClick={updateBook}
                    >
                        Update
                    </button>
                    <p>{message}</p>
                </div>
            ) : (
                <div>
                    <br />
                    <p>Please click on a Book...</p>
                </div>
            )}
        </div>
    )
}