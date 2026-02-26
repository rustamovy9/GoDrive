import axiosInstance from "./axiosInstance";
import type { Booking, BookingCreateInput } from "../../entities/Booking/types";

export const bookingsApi = {
  getMyBookings: () => axiosInstance.get<Booking[]>("/bookings/mine"),
  getById: (id: string) => axiosInstance.get<Booking>(`/bookings/${id}`),
  create: (data: BookingCreateInput) =>
    axiosInstance.post<Booking>("/bookings", data),
  cancel: (id: string) => axiosInstance.post(`/bookings/${id}/cancel`),
  getOwnerBookings: () => axiosInstance.get<Booking[]>("/bookings/owner"),
  updateStatus: (id: string, status: string) =>
    axiosInstance.patch<Booking>(`/bookings/${id}`, { status }),
};
