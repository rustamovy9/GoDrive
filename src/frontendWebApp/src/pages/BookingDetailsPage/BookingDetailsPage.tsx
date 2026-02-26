import { useParams, Link } from "react-router-dom";
import { useBooking, useCancelBooking } from "../../features/bookings/useBookings";
import { formatDate } from "../../shared/utils/formatDate";
import { Loader } from "../../widgets/Loader/Loader";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";
import { Button } from "../../shared/components/Button";

export function BookingDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const { data: booking, isLoading, error } = useBooking(id);
  const cancelBooking = useCancelBooking();

  if (isLoading) return <Loader />;
  if (error || !booking) return <ErrorMessage message={(error as Error)?.message ?? "Booking not found"} />;

  const carName = booking.car ? `${booking.car.make} ${booking.car.model} (${booking.car.year})` : "Car";

  return (
    <div className="max-w-2xl space-y-8">
      <Link to="/my-bookings" className="text-blue-600 hover:underline text-sm font-medium inline-block">← My bookings</Link>
      <div className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-6">
        <h1 className="text-3xl font-bold text-gray-900 mb-6">Booking details</h1>
        <dl className="space-y-2">
          <dt className="text-gray-500">Car</dt>
          <dd className="font-medium">{carName}</dd>
          <dt className="text-gray-500">Period</dt>
          <dd className="font-medium">{formatDate(booking.startDate)} – {formatDate(booking.endDate)}</dd>
          <dt className="text-gray-500">Status</dt>
          <dd>
            <span className={`inline-block px-2 py-0.5 rounded text-sm font-medium ${
              booking.status === "Confirmed" ? "bg-green-100 text-green-800" :
              booking.status === "Cancelled" ? "bg-red-100 text-red-800" : "bg-yellow-100 text-yellow-800"
            }`}>{booking.status}</span>
          </dd>
        </dl>
        {booking.status !== "Cancelled" && booking.status !== "Completed" && (
          <Button variant="danger" className="mt-6" onClick={() => cancelBooking.mutate(booking.id)} isLoading={cancelBooking.isPending}>
            Cancel booking
          </Button>
        )}
      </div>
    </div>
  );
}
