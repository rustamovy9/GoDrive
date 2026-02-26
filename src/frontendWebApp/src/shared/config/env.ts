const PRODUCTION_API = "https://godrive-ruc4.onrender.com/api";
export const API_BASE_URL =
  import.meta.env.VITE_API_URL ??
  (import.meta.env.DEV ? "/api" : PRODUCTION_API);