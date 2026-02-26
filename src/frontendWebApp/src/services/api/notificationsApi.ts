import axiosInstance from "./axiosInstance";
import type { Notification } from "../../entities/Notification/types";

export const notificationsApi = {
  getMine: () => axiosInstance.get<Notification[]>("/notifications"),
  markRead: (id: string) =>
    axiosInstance.patch<Notification>(`/notifications/${id}`, { read: true }),
  markAllRead: () => axiosInstance.patch("/notifications/read-all"),
};
