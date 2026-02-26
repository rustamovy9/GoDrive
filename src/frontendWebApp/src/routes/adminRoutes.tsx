import { Route } from "react-router-dom";
import { AdminDashboardPage } from "../pages/AdminDashboardPage/AdminDashboardPage";
import { AdminUsersPage } from "../pages/AdminUsersPage/AdminUsersPage";
import { AdminCarsPage } from "../pages/AdminCarsPage/AdminCarsPage";
import { AdminDocumentsPage } from "../pages/AdminDocumentsPage/AdminDocumentsPage";
import { AdminSettingsPage } from "../pages/AdminSettingsPage/AdminSettingsPage";

export const adminRoutes = (
  <>
    <Route path="/admin/dashboard" element={<AdminDashboardPage />} />
    <Route path="/admin/users" element={<AdminUsersPage />} />
    <Route path="/admin/cars" element={<AdminCarsPage />} />
    <Route path="/admin/documents" element={<AdminDocumentsPage />} />
    <Route path="/admin/settings" element={<AdminSettingsPage />} />
  </>
);
