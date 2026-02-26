import type { AxiosError } from "axios";

export function getApiErrorMessage(err: unknown): string {
  const axiosErr = err as AxiosError<{ message?: string; error?: string }>;
  return (
    axiosErr.response?.data?.message ??
    axiosErr.response?.data?.error ??
    (axiosErr.message || "Something went wrong")
  );
}
