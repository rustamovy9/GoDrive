import { useState } from "react";
import { useParams, Link } from "react-router-dom";
import { useCar } from "../../features/cars/useCars";
import { useAuthStore } from "../../features/auth/useAuth";
import { BookingForm } from "../../features/bookings/BookingForm";
import { useCreateBooking } from "../../features/bookings/useBookings";
import { Loader } from "../../widgets/Loader/Loader";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";
import { Button } from "../../shared/components/Button";
import { getApiErrorMessage } from "../../shared/utils/apiError";
import type { BookingCreateInput } from "../../entities/Booking/types";

export function CarDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const { data: car, isLoading, error } = useCar(id);
  const user = useAuthStore((s) => s.user);
  const createBooking = useCreateBooking();
  const minDate = new Date().toISOString().slice(0, 10);
  const [galleryIndex, setGalleryIndex] = useState(0);

  const images = car?.images?.length
    ? car.images
    : car?.imageUrl
      ? [car.imageUrl]
      : [];

  function handleBook(data: BookingCreateInput) {
    createBooking.mutate(data);
  }

  if (isLoading) return <Loader />;
  if (error || !car)
    return <ErrorMessage message={(error as Error)?.message ?? "Car not found"} />;

  return (
    <div className="max-w-5xl mx-auto space-y-8">
      <Link
        to="/cars"
        className="text-blue-600 hover:underline text-sm font-medium inline-block"
      >
        ← Back to cars
      </Link>

      <div className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 overflow-hidden">
        <div className="grid md:grid-cols-2 gap-0">
          <div className="relative">
            {images.length > 0 ? (
              <>
                <div className="aspect-[4/3] bg-gray-100">
                  <img
                    src={images[galleryIndex]}
                    alt={`${car.make} ${car.model}`}
                    className="w-full h-full object-cover"
                  />
                </div>
                {images.length > 1 && (
                  <div className="absolute bottom-3 left-3 right-3 flex gap-2 overflow-x-auto pb-1">
                    {images.map((src, i) => (
                      <button
                        key={src}
                        type="button"
                        onClick={() => setGalleryIndex(i)}
                        className={`flex-shrink-0 w-14 h-14 rounded-lg overflow-hidden border-2 ${
                          i === galleryIndex ? "border-blue-600" : "border-white"
                        }`}
                      >
                        <img src={src} alt="" className="w-full h-full object-cover" />
                      </button>
                    ))}
                  </div>
                )}
              </>
            ) : (
              <div className="aspect-[4/3] bg-gray-100 flex items-center justify-center text-gray-400">
                No image
              </div>
            )}
          </div>
          <div className="p-6 md:p-8 flex flex-col justify-between">
            <div>
              <h1 className="text-2xl font-bold text-gray-900">
                {car.make} {car.model}
              </h1>
              <p className="text-gray-500 mt-1">{car.year}</p>
              {car.city && (
                <p className="text-gray-500 text-sm mt-0.5">{car.city}</p>
              )}
              <p className="mt-4 text-xl font-bold text-blue-600">
                ${car.pricePerDay}
                <span className="text-gray-500 font-normal text-base">/day</span>
              </p>
              {car.description && (
                <p className="mt-4 text-gray-600">{car.description}</p>
              )}
            </div>
            <div className="mt-8">
              {user?.role === "User" ? (
                <>
                  {createBooking.isError && (
                    <ErrorMessage message={getApiErrorMessage(createBooking.error)} />
                  )}
                  {createBooking.isSuccess && (
                    <p className="text-green-600 mb-2 text-sm font-medium">
                      Booking requested.
                    </p>
                  )}
                  <BookingForm
                    carId={car.id}
                    onSubmit={handleBook}
                    isLoading={createBooking.isPending}
                    minDate={minDate}
                  />
                </>
              ) : (
                <Link to="/login">
                  <Button className="w-full">Login to book</Button>
                </Link>
              )}
            </div>
          </div>
        </div>
      </div>

      <div className="bg-white rounded-xl shadow-sm hover:shadow-md transition-all duration-200 p-6">
        <h2 className="text-xl font-semibold text-gray-800 mb-4">Reviews</h2>
        <p className="text-gray-500 text-sm">No reviews yet.</p>
      </div>
    </div>
  );
}
