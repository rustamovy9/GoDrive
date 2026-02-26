import axiosInstance from "./axiosInstance";
import type { User } from "../../entities/User/types";

export const usersApi = {
  getAll: (params?: { page?: number; limit?: number }) =>
    axiosInstance.get<User[]>("/users", { params }),
  getById: (id: string) => axiosInstance.get<User>(`/users/${id}`),
  update: (id: string, data: Partial<User>) =>
    axiosInstance.patch<User>(`/users/${id}`, data),
  uploadAvatar: (id: string, formData: FormData) =>
    axiosInstance.patch<User>(`/users/${id}/avatar`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    }),
};
