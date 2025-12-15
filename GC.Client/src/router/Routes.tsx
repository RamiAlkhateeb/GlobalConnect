import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import BookList from "../components/BooksList";
import UpdateBook from "../components/UpdateBook";
import AddBook from "../components/AddBook";

export const router = createBrowserRouter([
    {
        path: '/',
        element : <App />,
        children : [
            {path : '' , element : <BookList />},
            {path : 'books' , element : <BookList />},
            {path : 'books/:id' , element : <UpdateBook />},
            {path : 'add' , element : <AddBook />}

        ]
    }
])