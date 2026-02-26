import { useAuthStore } from "./useAuth";
import type { UserRole } from "../../entities/User/types";

export function useRoleGuard(allowedRoles: UserRole[]): boolean {
  const user = useAuthStore((s) => s.user);
  return !!user && allowedRoles.includes(user.role);
}
