import { ChangeEvent, useEffect, useState } from "react";
import Book from "../models/Book";
import BookService from "../services/BookService";
import { Link } from "react-router-dom";


export default function BookList() {

    const [books, setBooks] = useState<Array<Book>>([]);
    const [currentBook, setCurrentBook] = useState<Book | null>(null);
    const [currentIndex, setCurrentIndex] = useState<number>(-1);
    const [searchTitle, setSearchTitle] = useState<string>("");

    useEffect(() => {
        retrieveBooks();
    }, []);

    const onChangeSearchTitle = (e: ChangeEvent<HTMLInputElement>) => {
        const searchTitle = e.target.value;
        setSearchTitle(searchTitle);
    };

    const retrieveBooks = () => {
        BookService.getAll()
            .then((response: any) => {
                setBooks(response.data);
                console.log(response.data);
            })
            .catch((e: Error) => {
                console.log(e);
            });
    };

    const findByTitle = () => {
        BookService.findByTitle(searchTitle)
            .then((response: any) => {
                setBooks(response.data);
                setCurrentBook(null);
                setCurrentIndex(-1);
                console.log(response.data);
            })
            .catch((e: Error) => {
                console.log(e);
            });
    };

    const setActiveBook = (book: Book, index: number) => {
        setCurrentBook(book);
        setCurrentIndex(index);
    };

    return (
        <div className="list row">
            <div className="col-md-8">
                <div className="input-group mb-3">
                    <input
                        type="text"
                        className="form-control"
                        placeholder="Search by title"
                        value={searchTitle}
                        onChange={onChangeSearchTitle}
                    />
                    <div className="input-group-append">
                        <button
                            className="btn btn-outline-secondary"
                            type="button"
                            onClick={findByTitle}
                        >
                            Search
                        </button>
                    </div>
                </div>
            </div>
            <div className="col-md-6">
                <h4>Books List</h4>

                <ul className="list-group">
                    {books &&
                        books.map((book, index) => (
                            <li
                                className={
                                    "list-group-item " + (index === currentIndex ? "active" : "")
                                }
                                onClick={() => setActiveBook(book, index)}
                                key={index}
                            >
                                {book.title}
                            </li>
                        ))}
                </ul>

                {/* <button
                    className="m-3 btn btn-sm btn-danger"
                    onClick={removeAllBooks}
                >
                    Remove All
                </button> */}
            </div>
            <div className="col-md-6">
                {currentBook ? (
                    <div>
                        <h4>Book</h4>
                        <div>
                            <label>
                                <strong>Title:</strong>
                            </label>{" "}
                            {currentBook.title}
                        </div>
                        <div>
                            <label>
                                <strong>Author:</strong>
                            </label>{" "}
                            {currentBook.author }
                        </div>
                        <div>
                            <label>
                                <strong>Description:</strong>
                            </label>{" "}
                            {currentBook.description}
                        </div>
                        <div>
                            <label>
                                <strong>Price:</strong>
                            </label>{" "}
                            {currentBook.price }
                        </div>
                        

                        <Link
                            to={"/books/" + currentBook.id}
                            className="badge badge-warning"
                        >
                            Edit
                        </Link>
                    </div>
                ) : (
                    <div>
                        <br />
                        <p>Please click on a Book...</p>
                    </div>
                )}
            </div>
        </div>
    )
}