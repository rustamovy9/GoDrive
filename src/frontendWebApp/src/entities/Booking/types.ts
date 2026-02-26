import type { Car } from "../Car/types";
import type { User } from "../User/types";

export type BookingStatus = "Pending" | "Confirmed" | "Cancelled" | "Completed";

export interface Booking {
  id: string;
  carId: string;
  userId: string;
  startDate: string;
  endDate: string;
  status: BookingStatus;
  totalPrice?: number;
  createdAt?: string;
  updatedAt?: string;
  car?: Car;
  user?: User;
}

export interface BookingCreateInput {
  carId: string;
  startDate: string;
  endDate: string;
}
