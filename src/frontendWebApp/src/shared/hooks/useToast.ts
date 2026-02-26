import { useCallback, useState } from "react";

export type ToastType = "success" | "error" | "info";

export interface Toast {
  id: number;
  message: string;
  type: ToastType;
}

let id = 0;

export function useToast() {
  const [toasts, setToasts] = useState<Toast[]>([]);
  const add = useCallback((message: string, type: ToastType = "info") => {
    const toastId = ++id;
    setToasts((prev) => [...prev, { id: toastId, message, type }]);
    setTimeout(() => setToasts((p) => p.filter((t) => t.id !== toastId)), 4000);
  }, []);
  const remove = useCallback((toastId: number) => {
    setToasts((p) => p.filter((t) => t.id !== toastId));
  }, []);
  return { toasts, addToast: add, removeToast: remove };
}
