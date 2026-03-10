"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import { Star, MapPin, Users, Settings, Fuel } from "lucide-react";

interface CarType {
    id: number;
    brand: string;
    model: string;
    year: number;
    location: string;
    images: string[];
}

interface LocationType {
    id: number;
    name: string;
}

export default function CarDetails() {
    const params = useParams();
    const id = params?.id as string;

    const [car, setCar] = useState<CarType | null>(null);
    const [price, setPrice] = useState<number | null>(null);
    const [activeImage, setActiveImage] = useState(0);

    // Booking state
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [pickupLocationId, setPickupLocationId] = useState<number | null>(null);
    const [dropOffLocationId, setDropOffLocationId] = useState<number | null>(null);
    const [isBookingLoading, setIsBookingLoading] = useState(false);

    const [locations, setLocations] = useState<LocationType[]>([]);

    const token =
        typeof window !== "undefined" ? localStorage.getItem("token") : null;
    const isLoggedIn = !!token;

    useEffect(() => {
        fetchCar();
        fetchLocations();
    }, []);

    const fetchCar = async () => {
        try {
            const res = await fetch(
                `https://godrive-ruc4.onrender.com/api/cars/${id}`,
                { headers: { Authorization: `Bearer ${token}` } }
            );
            const data = await res.json();
            if (data.isSuccess) {
                setCar(data.data);
                fetchPrice(data.data.id);
            }
        } catch (err) {
            console.error(err);
        }
    };

    const fetchPrice = async (carId: number) => {
        try {
            const res = await fetch(
                `https://godrive-ruc4.onrender.com/api/car-prices/by-car/${carId}`,
                { headers: { Authorization: `Bearer ${token}` } }
            );
            const data = await res.json();
            if (data.isSuccess) setPrice(data.data.pricePerDay);
        } catch {
            setPrice(null);
        }
    };

    const fetchLocations = async () => {
        try {
            const res = await fetch(
                "https://godrive-ruc4.onrender.com/api/locations",
                { headers: { Authorization: `Bearer ${token}` } }
            );
            const data = await res.json();
            if (data.isSuccess) {
                // locations дар server дар data.data.data аст
                const locs = Array.isArray(data.data?.data) ? data.data.data : [];
                // city + country барои select хубтар нишон дода шавад
                setLocations(
                    locs.map((loc: any) => ({
                        id: loc.id,
                        name: `${loc.city}, ${loc.country}`,
                    }))
                );
            }
        } catch (err) {
            console.error(err);
        }
    };

    const handleBooking = async () => {
        if (!startDate || !endDate) {
            alert("Please select pick-up and drop-off dates");
            return;
        }
        if (!pickupLocationId || !dropOffLocationId) {
            alert("Please select pickup and drop-off locations");
            return;
        }
        if (pickupLocationId === dropOffLocationId) {
            alert("Pickup and drop-off locations cannot be the same");
            return;
        }
        if (!car) return;

        const start = new Date(startDate);
        const end = new Date(endDate);
        const now = new Date();

        if (start <= now) {
            alert("Start date must be in the future");
            return;
        }
        if (end <= start) {
            alert("End date must be after start date");
            return;
        }
        const diffHours = (end.getTime() - start.getTime()) / 1000 / 3600;
        if (diffHours < 1) {
            alert("Booking duration must be at least 1 hour");
            return;
        }

        setIsBookingLoading(true);

        try {
            const res = await fetch(
                "https://godrive-ruc4.onrender.com/api/bookings",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${token}`,
                    },
                    body: JSON.stringify({
                        carId: car.id,
                        startDateTime: start.toISOString(),
                        endDateTime: end.toISOString(),
                        pickupLocationId,
                        dropOffLocationId,
                        comment: "",
                    }),
                }
            );

            const data = await res.json();

            if (data.isSuccess) {
                alert("Booking successful!");
            } else {
                alert("Booking failed: " + (data.error?.message || "Unknown error"));
            }
        } catch (err) {
            console.error(err);
            alert("Booking failed");
        } finally {
            setIsBookingLoading(false);
        }
    };

    if (!car) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-black text-white">
                Loading...
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-gradient-to-b from-black via-gray-950 to-black text-white pt-[100px] px-8 pb-20">
            <div className="max-w-7xl mx-auto flex flex-col lg:flex-row gap-10">

                {/* Left: Car Images */}
                <div className="flex-1">
                    <div className="relative rounded-3xl overflow-hidden border border-gray-800 shadow-2xl">
                        <img
                            src={car.images?.[activeImage] || "/default-car.jpg"}
                            className="w-full h-[520px] object-cover"
                        />
                        <span className="absolute top-6 right-6 bg-green-500 text-sm font-semibold px-4 py-1 rounded-full">
                            Available
                        </span>
                    </div>

                    <div className="flex gap-4 mt-4">
                        {car.images?.map((img, index) => (
                            <img
                                key={index}
                                src={img}
                                onClick={() => setActiveImage(index)}
                                className={`h-20 w-28 object-cover rounded-lg cursor-pointer border transition ${activeImage === index
                                    ? "border-cyan-400 scale-105"
                                    : "border-gray-700 hover:border-gray-500"
                                    }`}
                            />
                        ))}
                    </div>
                </div>

                {/* Right: Info + Booking */}
                <div className="flex-1 flex flex-col gap-6">

                    {/* Car Info */}
                    <div>
                        <h1 className="text-4xl font-bold">
                            {car.brand} {car.model}
                        </h1>
                        <p className="text-gray-500 mt-1">{car.year}</p>
                        <div className="flex items-center gap-2 text-gray-400 mt-3">
                            <MapPin size={18} />
                            {car.location}
                        </div>

                        <div className="flex flex-wrap gap-6 mt-6 text-gray-300 border-t border-gray-800 pt-6">
                            <div className="flex items-center gap-2">
                                <Star size={18} className="text-yellow-400" fill="currentColor" />
                                4.8 (94 reviews)
                            </div>
                            <div className="flex items-center gap-2">
                                <Users size={18} />
                                4 Seats
                            </div>
                            <div className="flex items-center gap-2">
                                <Settings size={18} />
                                Automatic
                            </div>
                            <div className="flex items-center gap-2">
                                <Fuel size={18} />
                                Gasoline
                            </div>
                        </div>
                    </div>

                    {/* Booking Box */}
                    <div className="bg-[#111] p-6 rounded-2xl shadow-lg flex flex-col gap-4">
                        <div className="text-center">
                            <p className="text-3xl font-bold text-cyan-400">${price || 0}</p>
                            <span className="text-gray-400 text-sm">per day</span>
                        </div>

                        {/* Pickup / Drop-off */}
                        <select
                            value={pickupLocationId || ""}
                            onChange={(e) => setPickupLocationId(Number(e.target.value))}
                            className="bg-[#222] text-white p-3 rounded-lg w-full"
                        >
                            <option value="">Select Pickup Location</option>
                            {locations.map((loc) => (
                                <option key={loc.id} value={loc.id}>
                                    {loc.name}
                                </option>
                            ))}
                        </select>

                        <select
                            value={dropOffLocationId || ""}
                            onChange={(e) => setDropOffLocationId(Number(e.target.value))}
                            className="bg-[#222] text-white p-3 rounded-lg w-full"
                        >
                            <option value="">Select Drop-off Location</option>
                            {locations.map((loc) => (
                                <option key={loc.id} value={loc.id}>
                                    {loc.name}
                                </option>
                            ))}
                        </select>

                        <input
                            type="datetime-local"
                            value={startDate}
                            onChange={(e) => setStartDate(e.target.value)}
                            className="bg-[#222] text-white p-3 rounded-lg placeholder-gray-500 w-full"
                            placeholder="Pick-up"
                        />
                        <input
                            type="datetime-local"
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                            className="bg-[#222] text-white p-3 rounded-lg placeholder-gray-500 w-full"
                            placeholder="Drop-off"
                        />

                        <button
                            onClick={isLoggedIn ? handleBooking : () => alert("Please login first")}
                            className={`w-full py-3 rounded-lg font-semibold transition-all ${isLoggedIn
                                ? "bg-cyan-400 text-black hover:bg-cyan-300 hover:shadow-[0_0_25px_rgba(34,211,238,0.5)]"
                                : "bg-blue-500 text-white cursor-not-allowed opacity-80"
                                }`}
                            disabled={isBookingLoading}
                        >
                            {isLoggedIn ? (isBookingLoading ? "Booking..." : "Book Now") : "Login to Book"}
                        </button>

                        <p className="text-gray-500 text-xs text-center mt-2">
                            You won't be charged yet
                        </p>
                    </div>

                </div>
            </div>
        </div>
    );
}