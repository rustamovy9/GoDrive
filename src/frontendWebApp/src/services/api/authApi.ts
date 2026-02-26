import axiosInstance from "./axiosInstance";
import type { User } from "../../entities/User/types";

export const authApi = {
  me: () => axiosInstance.get<User>("/auth/me"),
  login: (email: string, password: string) =>
    axiosInstance.post<{ user: User }>("/auth/login", { email, password }),
  register: (data: { email: string; password: string; name: string }) =>
    axiosInstance.post<{ user: User }>("/auth/register", data),
  logout: () => axiosInstance.post("/auth/logout"),
};
