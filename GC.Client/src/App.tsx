import { Link, Outlet } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";


function App() {



  return (
    <>
      <nav className="navbar navbar-expand navbar-dark bg-dark mb-5">
        <a href="/books" className="navbar-brand">
          Library system management
        </a>
        <div className="navbar-nav mr-auto">
          <li className="nav-item">
            <Link to={"/books"} className="nav-link">
              Books
            </Link>
          </li>
          <li className="nav-item">
            <Link to={"/add"} className="nav-link">
              Add book
            </Link>
          </li>
        </div>
      </nav>


        <Outlet />
      

    </>
  );
}

export default App;
