import { Link } from "react-router-dom";
import type { Car } from "../../entities/Car/types";

export function CarCard({ car }: { car: Car }) {
  const imageUrl = car.imageUrl ?? car.images?.[0];

  return (
    <Link
      to={`/cars/${car.id}`}
      className="block bg-white rounded-xl shadow-sm hover:shadow-lg hover:-translate-y-1 transition-all duration-200 overflow-hidden cursor-pointer"
    >
      <div className="aspect-[4/3] bg-gray-100 w-full overflow-hidden">
        {imageUrl ? (
          <img
            src={imageUrl}
            alt={`${car.make} ${car.model}`}
            className="w-full h-full object-cover"
          />
        ) : (
          <div className="w-full h-full flex items-center justify-center text-gray-400 text-sm">
            No image
          </div>
        )}
      </div>
      <div className="p-4">
        <h3 className="text-lg font-bold text-gray-900">
          {car.make} {car.model}
        </h3>
        <p className="text-gray-400 text-sm mt-0.5">
          {car.city ?? "—"} · {car.year}
        </p>
        <p className="mt-3 text-xl font-bold text-blue-600">
          ${car.pricePerDay}
          <span className="text-gray-500 font-normal text-sm">/day</span>
        </p>
      </div>
    </Link>
  );
}
