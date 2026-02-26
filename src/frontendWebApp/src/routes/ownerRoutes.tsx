import { Route } from "react-router-dom";
import { OwnerDashboardPage } from "../pages/OwnerDashboardPage/OwnerDashboardPage";
import { OwnerCarsPage } from "../pages/OwnerCarsPage/OwnerCarsPage";
import { OwnerCarCreatePage } from "../pages/OwnerCarCreatePage/OwnerCarCreatePage";
import { OwnerCarDetailsPage } from "../pages/OwnerCarDetailsPage/OwnerCarDetailsPage";
import { OwnerBookingsPage } from "../pages/OwnerBookingsPage/OwnerBookingsPage";

export const ownerRoutes = (
  <>
    <Route path="/owner/dashboard" element={<OwnerDashboardPage />} />
    <Route path="/owner/cars" element={<OwnerCarsPage />} />
    <Route path="/owner/cars/create" element={<OwnerCarCreatePage />} />
    <Route path="/owner/cars/:id" element={<OwnerCarDetailsPage />} />
    <Route path="/owner/bookings" element={<OwnerBookingsPage />} />
  </>
);
