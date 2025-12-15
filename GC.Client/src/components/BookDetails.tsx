import { ChangeEvent, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Book from "../models/Book";
import BookService from "../services/BookService";
import InputText from "./InputText";

interface Props {
    book: Book,
    handleInputChange : (params: any) => any 
}

export default function BookDetails({ book  , handleInputChange}: Props) {


    return (
        <div>
            <div className="form-group">
                <label htmlFor="title">Title</label>
                <InputText value={book.title} inputName="title" handleInputChange={handleInputChange}/>

            </div>

            <div className="form-group">
                <label htmlFor="description">Description</label>
                <InputText value={book.description} inputName="description" handleInputChange={handleInputChange}/>

                
            </div>
            <div className="form-group">
                <label htmlFor="author">Author</label>
                <InputText value={book.author} inputName="author" handleInputChange={handleInputChange}/>

            </div>

            <div className="form-group">
                <label htmlFor="genre">Genre</label>
                <InputText value={book.genre} inputName="genre" handleInputChange={handleInputChange}/>

                
            </div>
            <div className="form-group">
                <label htmlFor="price">Price</label>
                <InputText value={book.price.toString()} inputName="price" handleInputChange={handleInputChange}/>

                
            </div>
        </div>
    )
}