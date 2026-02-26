import { useQuery } from "@tanstack/react-query";
import { carsApi } from "../../services/api/carsApi";
import { Loader } from "../../widgets/Loader/Loader";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";

export function AdminCarsPage() {
  const { data: cars, isLoading, error } = useQuery({
    queryKey: ["admin", "cars"],
    queryFn: async () => {
      const { data } = await carsApi.getAll();
      return data;
    },
  });

  if (isLoading) return <Loader />;
  if (error) return <ErrorMessage message={(error as Error).message} />;

  return (
    <div>
      <h1 className="text-2xl font-bold text-gray-900 mb-6">All cars</h1>
      <div className="bg-white rounded-xl shadow-md overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Car</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Year</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Price/day</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {(cars ?? []).map((car) => (
              <tr key={car.id}>
                <td className="px-6 py-4 whitespace-nowrap font-medium text-gray-900">{car.make} {car.model}</td>
                <td className="px-6 py-4 whitespace-nowrap text-gray-500">{car.year}</td>
                <td className="px-6 py-4 whitespace-nowrap text-gray-500">${car.pricePerDay}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
