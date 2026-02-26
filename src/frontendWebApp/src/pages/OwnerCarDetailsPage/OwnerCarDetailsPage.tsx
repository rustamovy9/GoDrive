import { useParams, Link, useNavigate } from "react-router-dom";
import { useCar, useUpdateCar, useDeleteCar } from "../../features/cars/useCars";
import { CarForm } from "../../features/cars/CarForm";
import { Loader } from "../../widgets/Loader/Loader";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";
import { Button } from "../../shared/components/Button";
import type { CarUpdateInput } from "../../entities/Car/types";

export function OwnerCarDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: car, isLoading, error } = useCar(id);
  const updateCar = useUpdateCar(id ?? "");
  const deleteCar = useDeleteCar();

  function handleSubmit(data: CarUpdateInput) {
    if (!id) return;
    updateCar.mutate(data);
  }

  if (isLoading) return <Loader />;
  if (error || !car) return <ErrorMessage message={(error as Error)?.message ?? "Car not found"} />;

  return (
    <div className="max-w-2xl">
      <Link to="/owner/cars" className="text-blue-600 hover:underline mb-4 inline-block">← My cars</Link>
      <div className="bg-white rounded-xl shadow-md overflow-hidden mb-6">
        {car.imageUrl && <img src={car.imageUrl} alt={`${car.make} ${car.model}`} className="w-full aspect-video object-cover" />}
        <div className="p-6">
          <h1 className="text-2xl font-bold text-gray-900 mb-4">{car.make} {car.model}</h1>
          <CarForm
            defaultValues={{ make: car.make, model: car.model, year: car.year, pricePerDay: car.pricePerDay, imageUrl: car.imageUrl ?? "", description: car.description ?? "" }}
            onSubmit={handleSubmit}
            isLoading={updateCar.isPending}
          />
          <Button
            variant="danger"
            className="mt-4"
            onClick={() => deleteCar.mutate(car.id, { onSuccess: () => navigate("/owner/cars") })}
            isLoading={deleteCar.isPending}
          >
            Delete car
          </Button>
        </div>
      </div>
    </div>
  );
}
