"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { Search, MapPin, Calendar, Tag, Loader2 } from "lucide-react";

interface Car {
  id: number;
  brand: string;
  model: string;
  year: number;
  images: string[];
  carStatus: number;
  locationId: number;
  currentPricePerDay?: number;
}

interface Category {
  id: number;
  name: string;
}

interface Location {
  id: number;
  country: string;
  city: string;
}

interface CarWithPrice extends Car {
  pricePerDay: number | null;
}

export default function BrowseCars() {
  const [cars, setCars] = useState<CarWithPrice[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [locations, setLocations] = useState<Location[]>([]);

  const [search, setSearch] = useState("");
  const [selectedCategory, setSelectedCategory] = useState<number | null>(null);
  const [selectedLocation, setSelectedLocation] = useState<number | null>(null);
  const [selectedStatus, setSelectedStatus] = useState<number | null>(null);

  const [loading, setLoading] = useState(true);

  const statusConfig: Record<number, { label: string; color: string; border: string }> = {
    0: { label: "Pending", color: "bg-gray-500/20 text-gray-300", border: "border-gray-500/30" },
    1: { label: "Available", color: "bg-emerald-500/20 text-emerald-300", border: "border-emerald-500/30" },
    2: { label: "Rented", color: "bg-amber-500/20 text-amber-300", border: "border-amber-500/30" },
    3: { label: "Maintenance", color: "bg-blue-500/20 text-blue-300", border: "border-blue-500/30" },
    4: { label: "Blocked", color: "bg-red-500/20 text-red-300", border: "border-red-500/30" },
  };

  const fetchCategories = async () => {
    const res = await fetch("https://godrive-5r3o.onrender.com/api/categories");
    const data = await res.json();
    if (data.isSuccess) setCategories(data.data.data);
  };

  const fetchLocations = async () => {
    const res = await fetch("https://godrive-5r3o.onrender.com/api/locations");
    const data = await res.json();
    if (data.isSuccess) setLocations(data.data.data);
  };

  const fetchCars = async () => {
    try {
      setLoading(true);

      const token = typeof window !== "undefined" ? localStorage.getItem("token") : null;

      let url = "";
      let headers: any = {};

      if (!token) {
        url = `https://godrive-5r3o.onrender.com/api/cars/public`;
      } else {
        url = `https://godrive-ruc4.onrender.com/api/cars?pageNumber=1&pageSize=12`;

        if (search) url += `&search=${search}`;
        if (selectedCategory) url += `&categoryId=${selectedCategory}`;
        if (selectedLocation) url += `&locationId=${selectedLocation}`;
        if (selectedStatus !== null) url += `&status=${selectedStatus}`;

        headers = { Authorization: `Bearer ${token}` };
      }

      const res = await fetch(url, { headers });
      const data = await res.json();

      if (data.isSuccess) {
        const carsData: Car[] = data.data.data;

        if (!token) {
          const mapped = carsData.map((car: any) => ({
            ...car,
            pricePerDay: car.currentPricePerDay ?? null,
          }));
          setCars(mapped);
          return;
        }

        const carsWithPrices = await Promise.all(
          carsData.map(async (car) => {
            try {
              const priceRes = await fetch(
                `https://godrive-ruc4.onrender.com/api/car-prices/by-car/${car.id}`,
                { headers }
              );
              const priceData = await priceRes.json();
              return {
                ...car,
                pricePerDay: priceData?.isSuccess ? priceData.data.pricePerDay : null,
              };
            } catch {
              return { ...car, pricePerDay: null };
            }
          })
        );

        setCars(carsWithPrices);
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCategories();
    fetchLocations();
  }, []);

  useEffect(() => {
    fetchCars();
  }, [search, selectedCategory, selectedLocation, selectedStatus]);

  const activeFiltersCount = [search, selectedCategory, selectedLocation, selectedStatus].filter(
    (v) => v !== null && v !== ""
  ).length;

  return (
    <div className="min-h-screen mt-[50px] bg-gradient-to-br from-gray-950 via-gray-900 to-black text-white">
      <header className="sticky top-[60px] z-40 backdrop-blur-xl bg-gray-950/80 border-b border-gray-800/50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
          <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
            <div>
              <h1 className="text-3xl sm:text-4xl font-bold bg-gradient-to-r from-cyan-400 via-blue-400 to-violet-400 bg-clip-text text-transparent">
                Browse Cars
              </h1>
              <p className="text-gray-400 mt-1 text-sm">
                {cars.length} {cars.length === 1 ? "vehicle" : "vehicles"} found
              </p>
            </div>

            {activeFiltersCount > 0 && (
              <div className="flex items-center gap-2 text-sm text-gray-400">
                <Tag className="w-4 h-4" />
                <span>{activeFiltersCount} filter{activeFiltersCount > 1 ? "s" : ""} active</span>
              </div>
            )}
          </div>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
          <div className="relative group">
            <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500 group-focus-within:text-cyan-400 transition-colors" />
            <input
              type="text"
              placeholder="Search cars..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="w-full pl-11 pr-4 py-3 bg-gray-900/80 border border-gray-800 rounded-xl 
                       text-white placeholder-gray-500 
                       focus:outline-none focus:ring-2 focus:ring-cyan-500/50 focus:border-cyan-500/50
                       transition-all duration-200"
            />
          </div>

          <div className="relative">
            <select
              value={selectedCategory || ""}
              onChange={(e) => setSelectedCategory(e.target.value ? Number(e.target.value) : null)}
              className="w-full pl-4 pr-10 py-3 bg-gray-900/80 border border-gray-800 rounded-xl 
                       text-white appearance-none cursor-pointer
                       focus:outline-none focus:ring-2 focus:ring-cyan-500/50 focus:border-cyan-500/50
                       transition-all duration-200"
            >
              <option value="" className="bg-gray-900">All Categories</option>
              {categories.map((c) => (
                <option key={c.id} value={c.id} className="bg-gray-900">
                  {c.name}
                </option>
              ))}
            </select>
            <Tag className="absolute right-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500 pointer-events-none" />
          </div>

          <div className="relative">
            <select
              value={selectedLocation || ""}
              onChange={(e) => setSelectedLocation(e.target.value ? Number(e.target.value) : null)}
              className="w-full pl-4 pr-10 py-3 bg-gray-900/80 border border-gray-800 rounded-xl 
                       text-white appearance-none cursor-pointer
                       focus:outline-none focus:ring-2 focus:ring-cyan-500/50 focus:border-cyan-500/50
                       transition-all duration-200"
            >
              <option value="" className="bg-gray-900">All Locations</option>
              {locations.map((l) => (
                <option key={l.id} value={l.id} className="bg-gray-900">
                  {l.city}, {l.country}
                </option>
              ))}
            </select>
            <MapPin className="absolute right-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500 pointer-events-none" />
          </div>

          <div className="relative">
            <select
              value={selectedStatus !== null ? selectedStatus : ""}
              onChange={(e) => setSelectedStatus(e.target.value ? Number(e.target.value) : null)}
              className="w-full pl-4 pr-10 py-3 bg-gray-900/80 border border-gray-800 rounded-xl 
                       text-white appearance-none cursor-pointer
                       focus:outline-none focus:ring-2 focus:ring-cyan-500/50 focus:border-cyan-500/50
                       transition-all duration-200"
            >
              <option value="" className="bg-gray-900">All Statuses</option>
              {Object.entries(statusConfig).map(([key, { label }]) => (
                <option key={key} value={key} className="bg-gray-900">
                  {label}
                </option>
              ))}
            </select>
            <FilterIcon className="absolute right-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-500 pointer-events-none" />
          </div>
        </div>

        {loading && (
          <div className="flex flex-col items-center justify-center py-20">
            <Loader2 className="w-10 h-10 text-cyan-400 animate-spin mb-4" />
            <p className="text-gray-400">Finding the perfect ride...</p>
          </div>
        )}

        {!loading && cars.length === 0 && (
          <div className="text-center py-20">
            <div className="w-20 h-20 mx-auto mb-6 rounded-full bg-gray-800/50 flex items-center justify-center">
              <Search className="w-10 h-10 text-gray-600" />
            </div>
            <h3 className="text-xl font-semibold mb-2">No cars found</h3>
            <p className="text-gray-400 max-w-md mx-auto">
              Try adjusting your filters or search terms to find what you're looking for.
            </p>
          </div>
        )}

        {!loading && cars.length > 0 && (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {cars.map((car) => {
              const status = statusConfig[car.carStatus];
              const location = locations.find((l) => l.id === car.locationId);

              return (
                <Link
                  key={car.id}
                  href={`/cars/${car.id}`}
                  className="group block"
                >
                  <article className="relative bg-gradient-to-br from-gray-900 to-gray-950 
                                   border border-gray-800/50 rounded-2xl overflow-hidden
                                   hover:border-cyan-500/30 hover:shadow-2xl hover:shadow-cyan-500/10
                                   transition-all duration-300 ease-out">

                    <div className="relative h-48 overflow-hidden">
                      <img
                        src={car.images?.[0]}
                        alt={`${car.brand} ${car.model}`}
                        className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
                        onError={(e) => {
                          (e.target as HTMLImageElement).src = "/placeholder-car.jpg";
                        }}
                      />
                      <div className="absolute inset-0 bg-gradient-to-t from-gray-950/80 via-transparent to-transparent" />

                      <span className={`absolute top-3 right-3 px-3 py-1 text-xs font-medium rounded-full 
                                     border ${status?.color} ${status?.border} backdrop-blur-sm`}>
                        {status?.label}
                      </span>
                    </div>

                    <div className="p-5">
                      <div className="flex items-start justify-between mb-3">
                        <div>
                          <h2 className="text-lg font-bold text-white group-hover:text-cyan-400 transition-colors">
                            {car.brand} {car.model}
                          </h2>
                          <div className="flex items-center gap-1.5 mt-1 text-gray-500">
                            <Calendar className="w-3.5 h-3.5" />
                            <span className="text-sm">{car.year}</span>
                          </div>
                        </div>
                      </div>

                      {location && (
                        <div className="flex items-center gap-1.5 text-gray-400 text-sm mb-4">
                          <MapPin className="w-4 h-4 flex-shrink-0" />
                          <span className="truncate">{location.city}, {location.country}</span>
                        </div>
                      )}

                      <div className="flex items-center justify-between pt-4 border-t border-gray-800/50">
                        <div>
                          <span className="text-xs text-gray-500 uppercase tracking-wide">Per day</span>
                          <p className="text-xl font-bold bg-gradient-to-r from-cyan-400 to-blue-400 bg-clip-text text-transparent">
                            {car.pricePerDay ? `$${car.pricePerDay}` : "Contact"}
                          </p>
                        </div>
                        <span className="px-4 py-2 bg-cyan-500/10 text-cyan-400 text-sm font-medium rounded-lg 
                                       border border-cyan-500/20 group-hover:bg-cyan-500 group-hover:text-white
                                       transition-all duration-200">
                          View Details
                        </span>
                      </div>
                    </div>

                    <div className="absolute inset-0 rounded-2xl opacity-0 group-hover:opacity-100 
                                  bg-gradient-to-r from-cyan-500/10 via-transparent to-violet-500/10 
                                  pointer-events-none transition-opacity duration-300" />
                  </article>
                </Link>
              );
            })}
          </div>
        )}
      </main>
    </div>
  );
}

function FilterIcon({ className }: { className?: string }) {
  return (
    <svg className={className} fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2}
        d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z" />
    </svg>
  );
}