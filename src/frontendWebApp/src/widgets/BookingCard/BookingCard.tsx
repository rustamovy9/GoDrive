import { Link } from "react-router-dom";
import { formatDate } from "../../shared/utils/formatDate";
import type { Booking } from "../../entities/Booking/types";

interface Props {
  booking: Booking;
  basePath?: string;
  noLink?: boolean;
}

export function BookingCard({ booking, basePath = "/my-bookings", noLink }: Props) {
  const name = booking.car ? `${booking.car.make} ${booking.car.model}` : "Car";
  const statusClass =
    booking.status === "Confirmed" ? "bg-green-100 text-green-800" :
    booking.status === "Cancelled" ? "bg-red-100 text-red-800" :
    booking.status === "Completed" ? "bg-gray-100 text-gray-800" : "bg-yellow-100 text-yellow-800";

  const content = (
    <>
      <h3 className="font-semibold text-gray-900">{name}</h3>
      <p className="text-sm text-gray-500 mt-1">{formatDate(booking.startDate)} – {formatDate(booking.endDate)}</p>
      <span className={`inline-block mt-2 px-2 py-0.5 rounded text-xs font-medium ${statusClass}`}>{booking.status}</span>
    </>
  );

  const cn = "block bg-white rounded-xl shadow-md p-4 hover:shadow-lg transition-shadow border border-gray-100";
  if (noLink) return <div className={cn}>{content}</div>;
  return <Link to={`${basePath}/${booking.id}`} className={cn}>{content}</Link>;
}
