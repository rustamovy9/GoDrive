import { Route } from "react-router-dom";
import { ProfilePage } from "../pages/ProfilePage/ProfilePage";
import { MyBookingsPage } from "../pages/MyBookingsPage/MyBookingsPage";
import { BookingDetailsPage } from "../pages/BookingDetailsPage/BookingDetailsPage";
import { NotificationsPage } from "../pages/NotificationsPage/NotificationsPage";
import { ReviewsPage } from "../pages/ReviewsPage/ReviewsPage";

export const userRoutes = (
  <>
    <Route path="/profile" element={<ProfilePage />} />
    <Route path="/my-bookings" element={<MyBookingsPage />} />
    <Route path="/my-bookings/:id" element={<BookingDetailsPage />} />
    <Route path="/notifications" element={<NotificationsPage />} />
    <Route path="/reviews" element={<ReviewsPage />} />
  </>
);
