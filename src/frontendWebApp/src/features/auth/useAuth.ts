import { create } from "zustand";
import { authApi } from "../../services/api/authApi";
import type { User } from "../../entities/User/types";

interface AuthState {
  user: User | null;
  setUser: (user: User | null) => void;
  fetchUser: () => Promise<void>;
  logout: () => Promise<void>;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  setUser: (user) => set({ user }),
  fetchUser: async () => {
    try {
      const { data } = await authApi.me();
      set({ user: data });
    } catch {
      set({ user: null });
    }
  },
  logout: async () => {
    try {
      await authApi.logout();
    } finally {
      set({ user: null });
    }
  },
}));
