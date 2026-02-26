export type UserRole = "Guest" | "User" | "Owner" | "Admin";

export interface User {
  id: string;
  email: string;
  name: string;
  role: UserRole;
  avatarUrl?: string;
  createdAt?: string;
  updatedAt?: string;
}
