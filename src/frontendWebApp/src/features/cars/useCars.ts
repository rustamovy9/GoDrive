import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { carsApi } from "../../services/api/carsApi";
import type { CarCreateInput, CarUpdateInput } from "../../entities/Car/types";

export interface CarsFilterParams {
  search?: string;
  city?: string;
  minPrice?: number;
  maxPrice?: number;
  category?: string;
  startDate?: string;
  endDate?: string;
}

const keys = {
  all: ["cars"] as const,
  list: (params?: CarsFilterParams) => [...keys.all, "list", params] as const,
  detail: (id: string) => [...keys.all, "detail", id] as const,
  owner: () => [...keys.all, "owner"] as const,
};

export function useCars(params?: CarsFilterParams) {
  return useQuery({
    queryKey: keys.list(params),
    queryFn: async () => {
      const { data } = await carsApi.getAll(params);
      return data;
    },
  });
}

export function useCar(id: string | undefined) {
  return useQuery({
    queryKey: keys.detail(id ?? ""),
    queryFn: async () => {
      const { data } = await carsApi.getById(id!);
      return data;
    },
    enabled: !!id,
  });
}

export function useOwnerCars() {
  return useQuery({
    queryKey: keys.owner(),
    queryFn: async () => {
      const { data } = await carsApi.getMyCars();
      return data;
    },
  });
}

export function useCreateCar() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: CarCreateInput) => carsApi.create(input),
    onSuccess: () => qc.invalidateQueries({ queryKey: keys.all }),
  });
}

export function useUpdateCar(id: string) {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (input: CarUpdateInput) => carsApi.update(id, input),
    onSuccess: () => qc.invalidateQueries({ queryKey: keys.all }),
  });
}

export function useDeleteCar() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => carsApi.delete(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: keys.all }),
  });
}
