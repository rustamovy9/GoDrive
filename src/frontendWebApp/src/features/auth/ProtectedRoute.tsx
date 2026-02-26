import { Navigate, Outlet } from "react-router-dom";
import { useAuthStore } from "./useAuth";
import type { UserRole } from "../../entities/User/types";

export function ProtectedRoute({ roles }: { roles?: UserRole[] }) {
  const user = useAuthStore((s) => s.user);
  if (!user) return <Navigate to="/login" replace />;
  if (roles?.length && !roles.includes(user.role))
    return <Navigate to="/" replace />;
  return <Outlet />;
}
