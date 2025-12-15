import http from "../http-common";
import Book from "../models/Book";

const getAll = () => {
  return http.get<Book[]>("/books");
};

const get = (id: any) => {
  return http.get<Book>(`/books/${id}`);
};

const create = (data: Book) => {
  return http.post<Book>("/books", data);
};

const update = (id: any, data: Book) => {
  return http.put<any>(`/books/${id}`, data);
};

const remove = (id: any) => {
  return http.delete<any>(`/books/${id}`);
};

const removeAll = () => {
  return http.delete<any>(`/books`);
};

const findByTitle = (title: string) => {
  return http.get<Array<Book>>(`/books?title=${title}`);
};

const BookService = {
  getAll,
  get,
  create,
  update,
  remove,
  removeAll,
  findByTitle,
};

export default BookService;