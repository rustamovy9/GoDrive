import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { notificationsApi } from "../../services/api/notificationsApi";

const keys = { all: ["notifications"] as const };

export function useNotifications() {
  return useQuery({
    queryKey: keys.all,
    queryFn: async () => {
      const { data } = await notificationsApi.getMine();
      return data;
    },
  });
}

export function useMarkNotificationRead() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => notificationsApi.markRead(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: keys.all }),
  });
}
