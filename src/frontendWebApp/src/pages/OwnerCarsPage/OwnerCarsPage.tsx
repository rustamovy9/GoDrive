import { Link } from "react-router-dom";
import { useOwnerCars } from "../../features/cars/useCars";
import { CarCard } from "../../widgets/CarCard/CarCard";
import { Loader } from "../../widgets/Loader/Loader";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";

export function OwnerCarsPage() {
  const { data: cars, isLoading, error } = useOwnerCars();
  if (isLoading) return <Loader />;
  if (error) return <ErrorMessage message={(error as Error).message} />;
  return (
    <div className="space-y-8">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-900">My cars</h1>
        <Link to="/owner/cars/create" className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg font-medium transition-colors duration-200">Add car</Link>
      </div>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {(cars ?? []).map((car) => (
          <div key={car.id}><Link to={`/owner/cars/${car.id}`} className="block"><CarCard car={car} /></Link></div>
        ))}
      </div>
      {(cars?.length ?? 0) === 0 && <p className="text-gray-500 py-8">You have no cars. Add your first car to get started.</p>}
    </div>
  );
}
