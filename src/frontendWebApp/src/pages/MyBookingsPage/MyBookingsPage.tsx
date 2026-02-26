import { useMyBookings } from "../../features/bookings/useBookings";
import { BookingCard } from "../../widgets/BookingCard/BookingCard";
import { Loader } from "../../widgets/Loader/Loader";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";

export function MyBookingsPage() {
  const { data: bookings, isLoading, error } = useMyBookings();
  if (isLoading) return <Loader />;
  if (error) return <ErrorMessage message={(error as Error).message} />;
  return (
    <div className="space-y-8">
      <h1 className="text-3xl font-bold text-gray-900">My bookings</h1>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {(bookings ?? []).map((b) => (
          <BookingCard key={b.id} booking={b} />
        ))}
      </div>
      {(bookings?.length ?? 0) === 0 && (
        <p className="text-gray-500 py-8">You have no bookings yet.</p>
      )}
    </div>
  );
}
