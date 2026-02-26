import { Link } from "react-router-dom";
import { useOwnerCars } from "../../features/cars/useCars";
import { useOwnerBookings } from "../../features/bookings/useBookings";
import { Loader } from "../../widgets/Loader/Loader";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";

export function OwnerDashboardPage() {
  const { data: cars, isLoading: carsLoading, error: carsError } = useOwnerCars();
  const { data: bookings, isLoading: bookingsLoading, error: bookingsError } = useOwnerBookings();

  if (carsLoading || bookingsLoading) return <Loader />;
  if (carsError) return <ErrorMessage message={(carsError as Error).message} />;
  if (bookingsError) return <ErrorMessage message={(bookingsError as Error).message} />;

  const confirmedBookings = bookings?.filter((b) => b.status === "Confirmed") ?? [];
  const earningsEstimate = confirmedBookings.reduce((sum, b) => sum + (b.totalPrice ?? 0), 0);

  return (
    <div className="space-y-8">
      <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-6">
          <h2 className="text-xl font-semibold text-gray-800">My cars</h2>
          <p className="text-3xl font-bold text-blue-600 mt-2">{cars?.length ?? 0}</p>
          <Link to="/owner/cars" className="text-blue-600 hover:underline mt-2 inline-block text-sm font-medium">
            Manage cars →
          </Link>
        </div>
        <div className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-6">
          <h2 className="text-xl font-semibold text-gray-800">Bookings</h2>
          <p className="text-3xl font-bold text-blue-600 mt-2">{bookings?.length ?? 0}</p>
          <Link to="/owner/bookings" className="text-blue-600 hover:underline mt-2 inline-block text-sm font-medium">
            View bookings →
          </Link>
        </div>
        <div className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-6">
          <h2 className="text-xl font-semibold text-gray-800">Earnings summary</h2>
          <p className="text-3xl font-bold text-green-600 mt-2">${earningsEstimate.toFixed(0)}</p>
          <p className="text-gray-400 text-sm mt-1">From confirmed bookings</p>
        </div>
      </div>
      <Link
        to="/owner/cars/create"
        className="inline-block bg-blue-600 hover:bg-blue-700 text-white rounded-lg px-4 py-2 font-medium transition-colors duration-200"
      >
        Add a car
      </Link>
    </div>
  );
}
