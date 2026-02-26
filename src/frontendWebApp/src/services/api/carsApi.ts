import axiosInstance from "./axiosInstance";
import type { Car, CarCreateInput, CarUpdateInput } from "../../entities/Car/types";

export const carsApi = {

  getAll: (params?: {
    search?: string;
    page?: number;
    limit?: number;
    city?: string;
    minPrice?: number;
    maxPrice?: number;
    category?: string;
    startDate?: string;
    endDate?: string;
  }) =>
    axiosInstance.get<Car[]>("/cars", { params }),

  getById: (id: number) =>
    axiosInstance.get<Car>(`/cars/${id}`),

  create: (data: CarCreateInput) =>
    axiosInstance.post<Car>("/cars", data),

  update: (id: number, data: CarUpdateInput) =>
    axiosInstance.patch<Car>(`/cars/${id}`, data),

  delete: (id: number) =>
    axiosInstance.delete(`/cars/${id}`),

  getMyCars: () =>
    axiosInstance.get<Car[]>("/cars/owner/mine"),

  uploadImages: (carId: number, formData: FormData) =>
    axiosInstance.post(`/cars/${carId}/images`, formData),

  uploadDocuments: (carId: number, formData: FormData) =>
    axiosInstance.post(`/cars/${carId}/documents`, formData),
};