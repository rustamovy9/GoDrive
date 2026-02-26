You are a senior frontend architect.

Build a production-ready frontend for a car rental platform using:

Tech stack:

* React
* TypeScript
* Vite
* TailwindCSS
* React Router
* Axios
* React Query
* Zustand

Backend API already exists.

Base API URL:
https://godrive-ruc4.onrender.com/api

Implement full frontend with clean architecture.

Project structure:

src/
app/
pages/
widgets/
features/
entities/
shared/
services/
routes/
layouts/

Roles:
Guest
User
Owner
Admin

Pages required:
**Project Structure:**
```
src/
  app/
    App.tsx
    main.tsx
  pages/
    HomePage/
      HomePage.tsx
    LoginPage/
      LoginPage.tsx
    RegisterPage/
      RegisterPage.tsx
    CarsPage/
      CarsPage.tsx
    CarDetailsPage/
      CarDetailsPage.tsx
    ProfilePage/
      ProfilePage.tsx
    MyBookingsPage/
      MyBookingsPage.tsx
    BookingDetailsPage/
      BookingDetailsPage.tsx
    NotificationsPage/
      NotificationsPage.tsx
    OwnerDashboardPage/
      OwnerDashboardPage.tsx
    OwnerCarsPage/
      OwnerCarsPage.tsx
    OwnerCarCreatePage/
      OwnerCarCreatePage.tsx
    OwnerCarDetailsPage/
      OwnerCarDetailsPage.tsx
    OwnerBookingsPage/
      OwnerBookingsPage.tsx
    AdminDashboardPage/
      AdminDashboardPage.tsx
    AdminUsersPage/
      AdminUsersPage.tsx
    AdminCarsPage/
      AdminCarsPage.tsx
    AdminDocumentsPage/
      AdminDocumentsPage.tsx
  widgets/
    Navbar/
      Navbar.tsx
    Sidebar/
      Sidebar.tsx
    CarCard/
      CarCard.tsx
    BookingCard/
      BookingCard.tsx
    UserMenu/
      UserMenu.tsx
    Loader/
      Loader.tsx
    ErrorMessage/
      ErrorMessage.tsx
  features/
    auth/
      useAuth.ts
      AuthProvider.tsx
      ProtectedRoute.tsx
      useRoleGuard.ts
    bookings/
      useBookings.ts
      BookingForm.tsx
    cars/
      useCars.ts
      CarForm.tsx
    notifications/
      useNotifications.ts
  entities/
    User/
      types.ts
    Car/
      types.ts
    Booking/
      types.ts
    Notification/
      types.ts
  shared/
    components/
      Button.tsx
      Input.tsx
      Select.tsx
      Modal.tsx
      Avatar.tsx
    hooks/
      useToast.ts
      useDebounce.ts
    utils/
      formatDate.ts
      apiError.ts
      constants.ts
    config/
      env.ts
  services/
    api/
      axiosInstance.ts
      authApi.ts
      carsApi.ts
      bookingsApi.ts
      notificationsApi.ts
      usersApi.ts
  routes/
    publicRoutes.tsx
    userRoutes.tsx
    ownerRoutes.tsx
    adminRoutes.tsx
    AppRouter.tsx
  layouts/
    MainLayout/
      MainLayout.tsx
    AuthLayout/
      AuthLayout.tsx
    OwnerLayout/
      OwnerLayout.tsx
    AdminLayout/
      AdminLayout.tsx
  index.css
  vite-env.d.ts
  types/
    global.d.ts
```

**Implementation Plan:**

- **Routing:** Use React Router v6 routes for all roles. Encapsulate Role-protected and public routes. Use `ProtectedRoute` and `useRoleGuard` for protected access.
- **State Management:** Use Zustand for auth state and global stores (user, cars, bookings, notifications).
- **Data Fetching:** Use Axios with baseURL, request/response interceptors for handling tokens, and React Query for caching/fetching.
- **Auth:** Store JWT in httpOnly cookie/localStorage. `AuthProvider` populates auth state on mount. Wrap protected pages with `ProtectedRoute`.
- **UI:** Use TailwindCSS for consistent modern design and mobile responsiveness.
- **Components:** Build reusable UI components (Button, Input, Loader, Modal, etc.).
- **Loading/Error Handling:** Show `<Loader />` on fetches; display `<ErrorMessage />` on errors.
- **Role-based Layouts:** Sidebar/nav changes based on user role (Guest, User, Owner, Admin).

**Example Core Files**

***src/services/api/axiosInstance.ts***
```ts
import axios from "axios";
const axiosInstance = axios.create({
  baseURL: "https://godrive-ruc4.onrender.com/api",
  withCredentials: true,
});
axiosInstance.interceptors.request.use(config => {
  // Attach token from Zustand store if needed
  return config;
});
axiosInstance.interceptors.response.use(
  res => res,
  err => {
    // Handle errors globally
    return Promise.reject(err);
  }
);
export default axiosInstance;
```

***src/features/auth/AuthProvider.tsx***
```tsx
import { useEffect } from "react";
import { useAuthStore } from "./useAuth";
export const AuthProvider = ({ children }) => {
  const { fetchUser } = useAuthStore();
  useEffect(() => { fetchUser(); }, []);
  return children;
};
```

***src/features/auth/ProtectedRoute.tsx***
```tsx
import { Navigate, Outlet } from "react-router-dom";
import { useAuthStore } from "./useAuth";
export const ProtectedRoute = ({ roles }) => {
  const { user } = useAuthStore();
  if (!user) return <Navigate to="/login" />;
  if (!roles?.includes(user.role)) return <Navigate to="/" />;
  return <Outlet />;
};
```

***src/routes/AppRouter.tsx***
```tsx
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ProtectedRoute } from "../features/auth/ProtectedRoute";
import { MainLayout } from "../layouts/MainLayout/MainLayout";
import { AuthLayout } from "../layouts/AuthLayout/AuthLayout";
import { OwnerLayout } from "../layouts/OwnerLayout/OwnerLayout";
import { AdminLayout } from "../layouts/AdminLayout/AdminLayout";

export const AppRouter = () => (
  <BrowserRouter>
    <Routes>
      {/* PUBLIC */}
      <Route element={<MainLayout />}>
        <Route path="/" element={<HomePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/cars" element={<CarsPage />} />
        <Route path="/cars/:id" element={<CarDetailsPage />} />
      </Route>

      {/* USER */}
      <Route element={<ProtectedRoute roles={['User']} />}>
        <Route element={<MainLayout />}>
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/my-bookings" element={<MyBookingsPage />} />
          <Route path="/my-bookings/:id" element={<BookingDetailsPage />} />
          <Route path="/notifications" element={<NotificationsPage />} />
        </Route>
      </Route>

      {/* OWNER */}
      <Route element={<ProtectedRoute roles={['Owner']} />}>
        <Route element={<OwnerLayout />}>
          <Route path="/owner/dashboard" element={<OwnerDashboardPage />} />
          <Route path="/owner/cars" element={<OwnerCarsPage />} />
          <Route path="/owner/cars/create" element={<OwnerCarCreatePage />} />
          <Route path="/owner/cars/:id" element={<OwnerCarDetailsPage />} />
          <Route path="/owner/bookings" element={<OwnerBookingsPage />} />
        </Route>
      </Route>

      {/* ADMIN */}
      <Route element={<ProtectedRoute roles={['Admin']} />}>
        <Route element={<AdminLayout />}>
          <Route path="/admin/dashboard" element={<AdminDashboardPage />} />
          <Route path="/admin/users" element={<AdminUsersPage />} />
          <Route path="/admin/cars" element={<AdminCarsPage />} />
          <Route path="/admin/documents" element={<AdminDocumentsPage />} />
        </Route>
      </Route>
    </Routes>
  </BrowserRouter>
);
```

***src/features/auth/useAuth.ts***
```ts
import { create } from "zustand";
import axiosInstance from "../../services/api/axiosInstance";
export const useAuthStore = create(set => ({
  user: null,
  setUser: (user) => set({ user }),
  fetchUser: async () => {
    try {
      const { data } = await axiosInstance.get("/auth/me");
      set({ user: data });
    } catch { set({ user: null }); }
  },
  logout: async () => {
    await axiosInstance.post("/auth/logout");
    set({ user: null });
  }
}));
```

***src/shared/components/Loader.tsx***
```tsx
export const Loader = () => (
  <div className="flex justify-center items-center h-full w-full">
    <span className="loader" />
  </div>
);
```

***src/shared/components/ErrorMessage.tsx***
```tsx
type Props = { message: string; };
export const ErrorMessage = ({ message }: Props) => (
  <div className="text-red-600 bg-red-100 border border-red-300 rounded p-2 my-2">
    {message}
  </div>
);
```

**Other Notes:**
- All pages are placed in `pages/`, each with its own folder.
- Use React Query hooks in features for data fetching (e.g. `useCars`, `useBookings`).
- Use Zustand for global auth and lightweight feature stores.
- Shared UI in `shared/components/`: buttons, inputs, modals, etc.
- Extend responsiveness and style with Tailwind classes.
- Layouts: MainLayout (basic site), OwnerLayout/AdminLayout (role-specific sidebar/navigation).

**To Implement:**
- Create each page and widget as functional components, consuming hooks and services as needed.
- Map out role-based navigation/menus.
- Ensure all API calls use `axiosInstance` for base URL and tokens.
- Leverage React Query for auto caching and refetching on mutation.

_This structure matches clean architecture, supports extension, and delivers on all requirements for robustness and maintainability._

PUBLIC:

* /
* /login
* /register
* /cars
* /cars/:id

USER:

* /profile
* /my-bookings
* /my-bookings/:id
* /notifications

OWNER:

* /owner/dashboard
* /owner/cars
* /owner/cars/create
* /owner/cars/:id
* /owner/bookings

ADMIN:

* /admin/dashboard
* /admin/users
* /admin/cars
* /admin/documents

Requirements:

* modern clean UI
* responsive design
* role-based routing
* protected routes
* axios client
* auth token handling
* reusable components
* loading states
* error handling

Generate full project structure and implementation.
