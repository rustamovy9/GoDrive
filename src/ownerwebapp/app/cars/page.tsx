"use client";

import Link from "next/link";
import { useEffect, useState } from "react";

interface Car {
    id: number;
    brand: string;
    model: string;
    year: number;
    carStatus: number;
    images: string[];
    locationId: number;
}

interface Location {
    id: number;
    country: string;
    city: string;
}

interface CarWithExtras extends Car {
    pricePerDay: number | null;
    location?: Location;
}

export default function CarsPage() {
    const [cars, setCars] = useState<CarWithExtras[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchAll = async () => {
            try {
                const token = localStorage.getItem("token");
                if (!token) return;

                const [carsRes, locationsRes] = await Promise.all([
                    fetch("https://godrive-5r3o.onrender.com/api/cars", {
                        headers: { Authorization: `Bearer ${token}` },
                    }),
                    fetch("https://godrive-ruc4.onrender.com/api/locations", {
                        headers: { Authorization: `Bearer ${token}` },
                    }),
                ]);

                const carsData = await carsRes.json();
                const locationsData = await locationsRes.json();

                const carsList: Car[] = carsData.data.data;
                const locationsList: Location[] = locationsData.data.data;

                const carsWithExtras = await Promise.all(
                    carsList.map(async (car) => {
                        try {
                            const priceRes = await fetch(
                                `https://godrive-ruc4.onrender.com/api/car-prices/by-car/${car.id}`,
                                { headers: { Authorization: `Bearer ${token}` } }
                            );
                            const priceData = await priceRes.json();

                            return {
                                ...car,
                                pricePerDay: priceData?.isSuccess
                                    ? priceData.data.pricePerDay
                                    : null,
                                location: locationsList.find((l) => l.id === car.locationId),
                            };
                        } catch {
                            return {
                                ...car,
                                pricePerDay: null,
                                location: locationsList.find((l) => l.id === car.locationId),
                            };
                        }
                    })
                );

                setCars(carsWithExtras);
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchAll();
    }, []);

    const getStatus = (status: number) => {
        switch (status) {
            case 0:
                return "Blocked";
            case 1:
                return "Available";
            case 2:
                return "Rented";
            case 3:
                return "Service";
            default:
                return "Unknown";
        }
    };

    const getStatusColor = (status: number) => {
        switch (status) {
            case 0:
                return "bg-red-500/20 text-red-400";
            case 1:
                return "bg-green-500/20 text-green-400";
            case 2:
                return "bg-yellow-500/20 text-yellow-400";
            case 3:
                return "bg-blue-500/20 text-blue-400";
            default:
                return "bg-gray-500/20 text-gray-400";
        }
    };

    if (loading) {
        return (
            <div className="p-6 space-y-4">
                {[1, 2, 3].map((i) => (
                    <div
                        key={i}
                        className="animate-pulse bg-zinc-900 h-32 rounded-xl"
                    />
                ))}
            </div>
        );
    }

    return (
        <div className="p-4 md:p-6 text-white">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-2xl md:text-3xl font-bold">My Cars</h1>
                <Link href="/addcar">
                    <button className="bg-cyan-500 hover:bg-cyan-600 text-black font-semibold px-4 py-2 rounded-lg transition text-sm md:text-base">
                        + Add Car
                    </button>
                </Link>
            </div>

            <div className="space-y-4">
                {cars.map((car) => (
                    <div
                        key={car.id}
                        className="flex flex-col sm:flex-row items-start sm:items-center gap-4 sm:gap-6 bg-zinc-900/80 border border-zinc-800 rounded-2xl p-4 backdrop-blur-xl hover:shadow-lg hover:shadow-cyan-500/10 transition"
                    >
                        <div className="w-full sm:w-40 h-28 bg-zinc-800 rounded-xl overflow-hidden">
                            {car.images?.length > 0 ? (
                                <img
                                    src={car.images[0]}
                                    className="w-full h-full object-cover"
                                />
                            ) : (
                                <div className="flex items-center justify-center h-full text-zinc-500">
                                    No Image
                                </div>
                            )}
                        </div>

                        <div className="flex-1 w-full">
                            <h2 className="text-lg font-semibold">
                                {car.brand} {car.model}
                            </h2>

                            <p className="text-zinc-400 text-sm">Year: {car.year}</p>
                            <p className="text-zinc-500 text-sm">
                                {car.location
                                    ? `${car.location.country} - ${car.location.city}`
                                    : "Unknown Location"}
                            </p>

                            <p className="text-cyan-400 font-semibold mt-1">
                                {car.pricePerDay ? `$${car.pricePerDay}` : "No Price"} /day
                            </p>
                        </div>

                        <div className="mt-2 sm:mt-0">
                            <span
                                className={`px-3 py-1 text-sm rounded-full ${getStatusColor(
                                    car.carStatus
                                )}`}
                            >
                                {getStatus(car.carStatus)}
                            </span>
                        </div>
                        <div className="flex gap-2 mt-2 sm:mt-0">
                            <Link href={`/editcar/${car.id}`}>
                                <button className="bg-cyan-500/20 hover:bg-cyan-500/30 text-cyan-400 px-3 py-1 rounded-lg text-sm transition">
                                    ✏️ Edit
                                </button>
                            </Link>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}