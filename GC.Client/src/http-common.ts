import axios from "axios";

export default axios.create({
  baseURL: "https://librarymsbackend.azurewebsites.net/api",
  headers: {
    "Content-type": "application/json"
  }
});