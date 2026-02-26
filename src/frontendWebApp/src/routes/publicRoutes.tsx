import { Route } from "react-router-dom";
import { HomePage } from "../pages/HomePage/HomePage";
import { LoginPage } from "../pages/LoginPage/LoginPage";
import { RegisterPage } from "../pages/RegisterPage/RegisterPage";
import { CarsPage } from "../pages/CarsPage/CarsPage";
import { CarDetailsPage } from "../pages/CarDetailsPage/CarDetailsPage";

export const publicRoutes = (
  <>
    <Route path="/" element={<HomePage />} />
    <Route path="/login" element={<LoginPage />} />
    <Route path="/register" element={<RegisterPage />} />
    <Route path="/cars" element={<CarsPage />} />
    <Route path="/cars/:id" element={<CarDetailsPage />} />
  </>
);
