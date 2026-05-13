"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import { MapPin, X, CheckCircle, AlertCircle, Info } from "lucide-react";

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

interface CustomModalProps {
    open: boolean;
    title: string;
    message: string;
    type: "success" | "error" | "info";
    onClose: () => void;
}

function CustomModal({
    open,
    title,
    message,
    type,
    onClose,
}: CustomModalProps) {
    if (!open) return null;

    const modalStyles = {
        success: {
            icon: <CheckCircle size={55} className="text-green-400" />,
            button: "bg-green-500 hover:bg-green-400",
            glow: "shadow-[0_0_30px_rgba(34,197,94,0.35)]",
        },
        error: {
            icon: <AlertCircle size={55} className="text-red-400" />,
            button: "bg-red-500 hover:bg-red-400",
            glow: "shadow-[0_0_30px_rgba(239,68,68,0.35)]",
        },
        info: {
            icon: <Info size={55} className="text-cyan-400" />,
            button: "bg-cyan-500 hover:bg-cyan-400",
            glow: "shadow-[0_0_30px_rgba(6,182,212,0.35)]",
        },
    };

    const style = modalStyles[type];

    return (
        <div className="fixed inset-0 z-[9999] flex items-center justify-center bg-black/70 backdrop-blur-sm px-4">
            <div
                className={`relative w-full max-w-md bg-[#111827] border border-gray-700 rounded-3xl p-8 animate-[popup_.3s_ease] ${style.glow}`}
            >
                <button
                    onClick={onClose}
                    className="absolute top-4 right-4 text-gray-400 hover:text-white transition"
                >
                    <X size={24} />
                </button>

                <div className="flex flex-col items-center text-center">
                    <div className="mb-5">{style.icon}</div>

                    <h2 className="text-2xl font-bold text-white mb-3">
                        {title}
                    </h2>

                    <p className="text-gray-300 leading-relaxed">
                        {message}
                    </p>

                    <button
                        onClick={onClose}
                        className={`mt-7 px-8 py-3 rounded-xl text-white font-semibold transition-all duration-300 ${style.button}`}
                    >
                        OK
                    </button>
                </div>
            </div>

            <style jsx>{`
                @keyframes popup {
                    0% {
                        opacity: 0;
                        transform: scale(0.8) translateY(30px);
                    }
                    100% {
                        opacity: 1;
                        transform: scale(1) translateY(0);
                    }
                }
            `}</style>
        </div>
    );
}

export default function CarDetails() {
    const params = useParams();
    const id = params?.id as string;

    const [car, setCar] = useState<CarType | null>(null);
    const [price, setPrice] = useState<number | null>(null);
    const [activeImage, setActiveImage] = useState(0);

    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [pickupLocationId, setPickupLocationId] = useState<number | null>(null);
    const [dropOffLocationId, setDropOffLocationId] = useState<number | null>(null);
    const [isBookingLoading, setIsBookingLoading] = useState(false);

    const [locations, setLocations] = useState<LocationType[]>([]);

    const [isModalOpen, setIsModalOpen] = useState(false);

    const [modalContent, setModalContent] = useState<{
        title: string;
        message: string;
        type: "success" | "error" | "info";
    }>({
        title: "",
        message: "",
        type: "info",
    });

    const token =
        typeof window !== "undefined"
            ? localStorage.getItem("token")
            : null;

    const isLoggedIn = !!token;

    useEffect(() => {
        if (id) {
            fetchCar();
            fetchLocations();
        }
    }, [id]);

    const showModal = (
        title: string,
        message: string,
        type: "success" | "error" | "info" = "info"
    ) => {
        setModalContent({ title, message, type });
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
    };

    const fetchCar = async () => {
        try {
            const res = await fetch(
                `https://godrive-ruc4.onrender.com/api/cars/${id}`,
                {
                    headers: token
                        ? {
                            Authorization: `Bearer ${token}`,
                        }
                        : {},
                }
            );

            const data = await res.json();

            if (data.isSuccess) {
                setCar(data.data);
                fetchPrice(data.data.id);
            }
        } catch (err) {
            console.error(err);
            showModal(
                "Error",
                "Failed to load car details",
                "error"
            );
        }
    };

    const fetchPrice = async (carId: number) => {
        try {
            const res = await fetch(
                `https://godrive-ruc4.onrender.com/api/car-prices/by-car/${carId}`,
                {
                    headers: token
                        ? {
                            Authorization: `Bearer ${token}`,
                        }
                        : {},
                }
            );

            const data = await res.json();

            if (data.isSuccess) {
                setPrice(data.data.pricePerDay);
            }
        } catch {
            setPrice(null);
        }
    };

    const fetchLocations = async () => {
        try {
            const res = await fetch(
                "https://godrive-ruc4.onrender.com/api/locations",
                {
                    headers: token
                        ? {
                            Authorization: `Bearer ${token}`,
                        }
                        : {},
                }
            );

            const data = await res.json();

            if (data.isSuccess) {
                const locs = Array.isArray(data.data?.data)
                    ? data.data.data
                    : [];

                setLocations(
                    locs.map((loc: any) => ({
                        id: loc.id,
                        name: `${loc.city}, ${loc.country}`,
                    }))
                );
            }
        } catch (err) {
            console.error(err);
            showModal(
                "Error",
                "Failed to load locations",
                "error"
            );
        }
    };

    const handleBooking = async () => {
        if (!startDate || !endDate) {
            showModal(
                "Missing Information",
                "Please select pick-up and drop-off dates",
                "error"
            );
            return;
        }

        if (!pickupLocationId || !dropOffLocationId) {
            showModal(
                "Missing Information",
                "Please select pickup and drop-off locations",
                "error"
            );
            return;
        }

        if (pickupLocationId === dropOffLocationId) {
            showModal(
                "Invalid Selection",
                "Pickup and drop-off locations cannot be the same",
                "error"
            );
            return;
        }

        if (!car) return;

        const start = new Date(startDate);
        const end = new Date(endDate);
        const now = new Date();

        if (start <= now) {
            showModal(
                "Invalid Date",
                "Start date must be in the future",
                "error"
            );
            return;
        }

        if (end <= start) {
            showModal(
                "Invalid Date",
                "End date must be after start date",
                "error"
            );
            return;
        }

        const diffHours =
            (end.getTime() - start.getTime()) / 1000 / 3600;

        if (diffHours < 1) {
            showModal(
                "Invalid Duration",
                "Booking duration must be at least 1 hour",
                "error"
            );
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
                        ...(token
                            ? {
                                Authorization: `Bearer ${token}`,
                            }
                            : {}),
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
                showModal(
                    "Success!",
                    "Booking successful!",
                    "success"
                );
            } else {
                showModal(
                    "Booking Failed",
                    data.error?.message || "Unknown error",
                    "error"
                );
            }
        } catch (err) {
            console.error(err);

            showModal(
                "Error",
                "Booking failed. Please try again.",
                "error"
            );
        } finally {
            setIsBookingLoading(false);
        }
    };

    const handleLoginAlert = () => {
        showModal(
            "Login Required",
            "Please login to book this car",
            "info"
        );
    };

    if (!car) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-black text-white text-2xl font-bold">
                Loading...
            </div>
        );
    }

    return (
        <>
            <div className="min-h-screen bg-gradient-to-b from-black via-gray-950 to-black text-white pt-[100px] px-8 pb-20">
                <div className="max-w-7xl mx-auto flex flex-col lg:flex-row gap-10">

                    <div className="flex-1">
                        <div className="relative rounded-3xl overflow-hidden border border-gray-800 shadow-2xl">
                            <img
                                src={
                                    car.images?.[activeImage] ||
                                    "/default-car.jpg"
                                }
                                className="w-full h-[520px] object-cover"
                                alt={`${car.brand} ${car.model}`}
                            />

                            <span className="absolute top-6 right-6 bg-green-500 text-sm font-semibold px-4 py-1 rounded-full">
                                Available
                            </span>
                        </div>

                        <div className="flex gap-4 mt-4 flex-wrap">
                            {car.images?.map((img, index) => (
                                <img
                                    key={index}
                                    src={img}
                                    onClick={() =>
                                        setActiveImage(index)
                                    }
                                    alt={`Thumbnail ${index + 1}`}
                                    className={`h-20 w-28 object-cover rounded-xl cursor-pointer border transition-all duration-300 ${activeImage === index
                                            ? "border-cyan-400 scale-105"
                                            : "border-gray-700 hover:border-gray-500"
                                        }`}
                                />
                            ))}
                        </div>
                    </div>

                    <div className="flex-1 flex flex-col gap-6">

                        <div>
                            <h1 className="text-5xl font-bold">
                                {car.brand} {car.model}
                            </h1>

                            <p className="text-gray-500 mt-2">
                                {car.year}
                            </p>

                            <div className="flex items-center gap-2 text-gray-400 mt-4">
                                <MapPin size={18} />
                                {car.location}
                            </div>
                        </div>

                        <div className="bg-[#111] p-7 rounded-3xl shadow-2xl flex flex-col gap-4 border border-gray-800">
                            <div className="text-center">
                                <p className="text-4xl font-bold text-cyan-400">
                                    ${price || 0}
                                </p>

                                <span className="text-gray-400 text-sm">
                                    per day
                                </span>
                            </div>

                            <select
                                value={pickupLocationId || ""}
                                onChange={(e) =>
                                    setPickupLocationId(
                                        Number(e.target.value)
                                    )
                                }
                                className="bg-[#222] text-white p-3 rounded-xl border border-gray-700 focus:outline-none focus:border-cyan-400"
                            >
                                <option value="">
                                    Select Pickup Location
                                </option>

                                {locations.map((loc) => (
                                    <option
                                        key={loc.id}
                                        value={loc.id}
                                    >
                                        {loc.name}
                                    </option>
                                ))}
                            </select>

                            <select
                                value={dropOffLocationId || ""}
                                onChange={(e) =>
                                    setDropOffLocationId(
                                        Number(e.target.value)
                                    )
                                }
                                className="bg-[#222] text-white p-3 rounded-xl border border-gray-700 focus:outline-none focus:border-cyan-400"
                            >
                                <option value="">
                                    Select Drop-off Location
                                </option>

                                {locations.map((loc) => (
                                    <option
                                        key={loc.id}
                                        value={loc.id}
                                    >
                                        {loc.name}
                                    </option>
                                ))}
                            </select>

                            <input
                                type="datetime-local"
                                value={startDate}
                                onChange={(e) =>
                                    setStartDate(e.target.value)
                                }
                                className="bg-[#222] text-white p-3 rounded-xl border border-gray-700 focus:outline-none focus:border-cyan-400"
                            />

                            <input
                                type="datetime-local"
                                value={endDate}
                                onChange={(e) =>
                                    setEndDate(e.target.value)
                                }
                                className="bg-[#222] text-white p-3 rounded-xl border border-gray-700 focus:outline-none focus:border-cyan-400"
                            />

                            <button
                                onClick={
                                    isLoggedIn
                                        ? handleBooking
                                        : handleLoginAlert
                                }
                                disabled={isBookingLoading}
                                className={`w-full py-4 rounded-xl font-bold text-lg transition-all duration-300 ${isLoggedIn
                                        ? "bg-cyan-400 text-black hover:bg-cyan-300 hover:shadow-[0_0_25px_rgba(34,211,238,0.5)]"
                                        : "bg-blue-500 text-white"
                                    }`}
                            >
                                {isLoggedIn
                                    ? isBookingLoading
                                        ? "Booking..."
                                        : "Book Now"
                                    : "Login to Book"}
                            </button>

                            <p className="text-gray-500 text-xs text-center mt-2">
                                You won't be charged yet
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <CustomModal
                open={isModalOpen}
                title={modalContent.title}
                message={modalContent.message}
                type={modalContent.type}
                onClose={closeModal}
            />
        </>
    );
}