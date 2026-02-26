import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { bookingsApi } from "../../services/api/bookingsApi";
import type { BookingCreateInput } from "../../entities/Booking/types";

const keys = { all: ["bookings"] as const, mine: () => [...keys.all, "mine"] as const, detail: (id: string) => [...keys.all, "detail", id] as const, owner: () => [...keys.all, "owner"] as const };

export function useMyBookings() {
  return useQuery({
    queryKey: keys.mine(),
    queryFn: async () => {
      const { data } = await bookingsApi.getMyBookings();
      return data;
    },
  });
}

export function useBooking(id: string | undefined) {
  return useQuery({
    queryKey: keys.detail(id ?? ""),
    queryFn: async () => {
      const { data } = await bookingsApi.getById(id!);
      return data;
    },
    enabled: !!id,
  });
}

export function useOwnerBookings() {
  return useQuery({
    queryKey: keys.owner(),
    queryFn: async () => {
      const { data } = await bookingsApi.getOwnerBookings();
      return data;
    },
  });
}

export function useCreateBooking() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: BookingCreateInput) => bookingsApi.create(input),
    onSuccess: () => qc.invalidateQueries({ queryKey: keys.all }),
  });
}

export function useCancelBooking() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => bookingsApi.cancel(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: keys.all }),
  });
}

export function useUpdateBookingStatus() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, status }: { id: string; status: string }) => bookingsApi.updateStatus(id, status),
    onSuccess: () => qc.invalidateQueries({ queryKey: keys.all }),
  });
}
