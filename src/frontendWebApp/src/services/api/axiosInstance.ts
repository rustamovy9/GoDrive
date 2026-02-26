import axios from "axios";
import { API_BASE_URL } from "../../shared/config/env";

const axiosInstance = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

axiosInstance.interceptors.request.use((config) => config);

axiosInstance.interceptors.response.use(
  (res) => res,
  (err) => {
    if (err.response?.status === 401) {
      // можно сбросить auth при 401
    }
    return Promise.reject(err);
  }
);

export default axiosInstance;