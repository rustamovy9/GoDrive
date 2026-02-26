import { useState } from "react";
import { useCars, type CarsFilterParams } from "../../features/cars/useCars";
import { useDebounce } from "../../shared/hooks/useDebounce";
import { CarCard } from "../../widgets/CarCard/CarCard";
import { Loader } from "../../widgets/Loader/Loader";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";
import { Input } from "../../shared/components/Input";
import { Select } from "../../shared/components/Select";

const CATEGORIES = [
  { value: "", label: "All categories" },
  { value: "economy", label: "Economy" },
  { value: "sedan", label: "Sedan" },
  { value: "suv", label: "SUV" },
  { value: "luxury", label: "Luxury" },
];

export function CarsPage() {
  const [search, setSearch] = useState("");
  const [city, setCity] = useState("");
  const [minPrice, setMinPrice] = useState<string>("");
  const [maxPrice, setMaxPrice] = useState<string>("");
  const [category, setCategory] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const debouncedSearch = useDebounce(search, 300);
  const filters: CarsFilterParams = {
    search: debouncedSearch || undefined,
    city: city || undefined,
    minPrice: minPrice ? Number(minPrice) : undefined,
    maxPrice: maxPrice ? Number(maxPrice) : undefined,
    category: category || undefined,
    startDate: startDate || undefined,
    endDate: endDate || undefined,
  };

  const { data: cars, isLoading, error } = useCars(filters);

  if (isLoading) return <Loader />;
  if (error) return <ErrorMessage message={(error as Error).message} />;

  return (
    <div className="space-y-8">
      <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-6">
        <h1 className="text-3xl font-bold text-gray-900">Browse Cars</h1>
        <div className="flex flex-wrap items-center gap-3">
          <Input
            placeholder="Search..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="bg-white border-gray-200 w-48"
          />
          <Input
            placeholder="City"
            value={city}
            onChange={(e) => setCity(e.target.value)}
            className="bg-white border-gray-200 w-32"
          />
          <Input
            type="number"
            placeholder="Min $"
            value={minPrice}
            onChange={(e) => setMinPrice(e.target.value)}
            className="bg-white border-gray-200 w-24"
          />
          <Input
            type="number"
            placeholder="Max $"
            value={maxPrice}
            onChange={(e) => setMaxPrice(e.target.value)}
            className="bg-white border-gray-200 w-24"
          />
          <Select
            options={CATEGORIES}
            value={category}
            onChange={(e) => setCategory(e.target.value)}
            className="w-36"
          />
          <Input
            type="date"
            placeholder="Start"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
            className="bg-white border-gray-200 w-40"
          />
          <Input
            type="date"
            placeholder="End"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
            className="bg-white border-gray-200 w-40"
          />
        </div>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {(cars ?? []).map((car) => (
          <CarCard key={car.id} car={car} />
        ))}
      </div>
      {(cars?.length ?? 0) === 0 && (
        <p className="text-gray-400 text-sm text-center py-12">No cars found.</p>
      )}
    </div>
  );
}
