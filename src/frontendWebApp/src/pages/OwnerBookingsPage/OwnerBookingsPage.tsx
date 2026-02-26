import { useOwnerBookings, useUpdateBookingStatus } from "../../features/bookings/useBookings";
import { BookingCard } from "../../widgets/BookingCard/BookingCard";
import { Loader } from "../../widgets/Loader/Loader";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";
import { Button } from "../../shared/components/Button";
import { formatDate } from "../../shared/utils/formatDate";

export function OwnerBookingsPage() {
  const { data: bookings, isLoading, error } = useOwnerBookings();
  const updateStatus = useUpdateBookingStatus();

  if (isLoading) return <Loader />;
  if (error) return <ErrorMessage message={(error as Error).message} />;

  return (
    <div className="space-y-8">
      <h1 className="text-3xl font-bold text-gray-900">Bookings</h1>
      <div className="space-y-4">
        {(bookings ?? []).map((booking) => (
          <div
            key={booking.id}
            className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-5"
          >
            <BookingCard booking={booking} noLink />
            {booking.status === "Pending" && (
              <div className="mt-4 flex gap-3">
                <Button
                  onClick={() =>
                    updateStatus.mutate({ id: booking.id, status: "Confirmed" })
                  }
                  isLoading={updateStatus.isPending}
                >
                  Confirm
                </Button>
                <Button
                  variant="danger"
                  onClick={() =>
                    updateStatus.mutate({ id: booking.id, status: "Cancelled" })
                  }
                  isLoading={updateStatus.isPending}
                >
                  Reject
                </Button>
              </div>
            )}
          </div>
        ))}
      </div>
      {(bookings?.length ?? 0) === 0 && (
        <p className="text-gray-500 py-8">No bookings yet.</p>
      )}
    </div>
  );
}
