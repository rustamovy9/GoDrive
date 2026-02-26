import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ProtectedRoute } from "../features/auth/ProtectedRoute";
import { MainLayout } from "../layouts/MainLayout/MainLayout";
import { OwnerLayout } from "../layouts/OwnerLayout/OwnerLayout";
import { AdminLayout } from "../layouts/AdminLayout/AdminLayout";
import { publicRoutes } from "./publicRoutes";
import { userRoutes } from "./userRoutes";
import { ownerRoutes } from "./ownerRoutes";
import { adminRoutes } from "./adminRoutes";

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<MainLayout />}>{publicRoutes}</Route>
        <Route element={<ProtectedRoute roles={["User"]} />}>
          <Route element={<MainLayout />}>{userRoutes}</Route>
        </Route>
        <Route element={<ProtectedRoute roles={["Owner"]} />}>
          <Route element={<OwnerLayout />}>{ownerRoutes}</Route>
        </Route>
        <Route element={<ProtectedRoute roles={["Admin"]} />}>
          <Route element={<AdminLayout />}>{adminRoutes}</Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
