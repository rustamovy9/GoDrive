"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import { MapPin } from "lucide-react";
import { Modal, Button } from "antd";

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
    } | null>(null);

    const token =
        typeof window !== "undefined" ? localStorage.getItem("token") : null;
    const isLoggedIn = !!token;

    useEffect(() => {
        if (id) {
            fetchCar();
            fetchLocations();
        }
    }, [id, token]);

    const showModal = (title: string, message: string, type: "success" | "error" | "info" = "info") => {
        setModalContent({ title, message, type });
        setIsModalOpen(true);
    };

    const handleModalOk = () => {
        setIsModalOpen(false);
        setModalContent(null);
    };

    const handleModalCancel = () => {
        setIsModalOpen(false);
        setModalContent(null);
    };

    const fetchCar = async () => {
        try {
            const res = await fetch(
                `https://godrive-ruc4.onrender.com/api/cars/${id}`,
                {
                    headers: token
                        ? { Authorization: `Bearer ${token}` }
                        : {}
                }
            );
            const data = await res.json();
            if (data.isSuccess) {
                setCar(data.data);
                fetchPrice(data.data.id);
            }
        } catch (err) {
            console.error(err);
            showModal("Error", "Failed to load car details", "error");
        }
    };

    const fetchPrice = async (carId: number) => {
        try {
            const res = await fetch(
                `https://godrive-ruc4.onrender.com/api/car-prices/by-car/${carId}`,
                {
                    headers: token
                        ? { Authorization: `Bearer ${token}` }
                        : {}
                }
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
                {
                    headers: token
                        ? { Authorization: `Bearer ${token}` }
                        : {}
                }
            );
            const data = await res.json();
            if (data.isSuccess) {
                const locs = Array.isArray(data.data?.data) ? data.data.data : [];
                setLocations(
                    locs.map((loc: any) => ({
                        id: loc.id,
                        name: `${loc.city}, ${loc.country}`,
                    }))
                );
            }
        } catch (err) {
            console.error(err);
            showModal("Error", "Failed to load locations", "error");
        }
    };

    const handleBooking = async () => {
        if (!startDate || !endDate) {
            showModal("Missing Information", "Please select pick-up and drop-off dates", "error");
            return;
        }
        if (!pickupLocationId || !dropOffLocationId) {
            showModal("Missing Information", "Please select pickup and drop-off locations", "error");
            return;
        }
        if (pickupLocationId === dropOffLocationId) {
            showModal("Invalid Selection", "Pickup and drop-off locations cannot be the same", "error");
            return;
        }
        if (!car) return;

        const start = new Date(startDate);
        const end = new Date(endDate);
        const now = new Date();

        if (start <= now) {
            showModal("Invalid Date", "Start date must be in the future", "error");
            return;
        }
        if (end <= start) {
            showModal("Invalid Date", "End date must be after start date", "error");
            return;
        }
        const diffHours = (end.getTime() - start.getTime()) / 1000 / 3600;
        if (diffHours < 1) {
            showModal("Invalid Duration", "Booking duration must be at least 1 hour", "error");
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
                        ...(token ? { Authorization: `Bearer ${token}` } : {}),
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
                showModal("Success!", "Booking successful!", "success");
            } else {
                showModal("Booking Failed", data.error?.message || "Unknown error", "error");
            }
        } catch (err) {
            console.error(err);
            showModal("Error", "Booking failed. Please try again.", "error");
        } finally {
            setIsBookingLoading(false);
        }
    };

    const handleLoginAlert = () => {
        showModal("Login Required", "Please login to book this car", "info");
    };

    if (!car) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-black text-white">
                Loading...
            </div>
        );
    }

    const getModalStyles = () => {
        if (!modalContent) return {};
        switch (modalContent.type) {
            case "success":
                return {
                    confirmButtonProps: { className: "bg-green-500 hover:bg-green-600 text-white border-0" },
                    contentClass: "text-green-700",
                };
            case "error":
                return {
                    confirmButtonProps: { className: "bg-red-500 hover:bg-red-600 text-white border-0" },
                    contentClass: "text-red-700",
                };
            default:
                return {
                    confirmButtonProps: { className: "bg-cyan-400 hover:bg-cyan-300 text-black border-0" },
                    contentClass: "text-gray-700",
                };
        }
    };

    const modalStyles = getModalStyles();

    return (
        <div className="min-h-screen bg-gradient-to-b from-black via-gray-950 to-black text-white pt-[100px] px-8 pb-20">
            <div className="max-w-7xl mx-auto flex flex-col lg:flex-row gap-10">

                <div className="flex-1">
                    <div className="relative rounded-3xl overflow-hidden border border-gray-800 shadow-2xl">
                        <img
                            src={car.images?.[activeImage] || "/default-car.jpg"}
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
                                onClick={() => setActiveImage(index)}
                                alt={`Thumbnail ${index + 1}`}
                                className={`h-20 w-28 object-cover rounded-lg cursor-pointer border transition ${activeImage === index
                                    ? "border-cyan-400 scale-105"
                                    : "border-gray-700 hover:border-gray-500"
                                    }`}
                            />
                        ))}
                    </div>
                </div>

                <div className="flex-1 flex flex-col gap-6">

                    <div>
                        <h1 className="text-4xl font-bold">
                            {car.brand} {car.model}
                        </h1>
                        <p className="text-gray-500 mt-1">{car.year}</p>
                        <div className="flex items-center gap-2 text-gray-400 mt-3">
                            <MapPin size={18} />
                            {car.location}
                        </div>
                    </div>

                    <div className="bg-[#111] p-6 rounded-2xl shadow-lg flex flex-col gap-4">
                        <div className="text-center">
                            <p className="text-3xl font-bold text-cyan-400">${price || 0}</p>
                            <span className="text-gray-400 text-sm">per day</span>
                        </div>

                        <select
                            value={pickupLocationId || ""}
                            onChange={(e) => setPickupLocationId(Number(e.target.value))}
                            className="bg-[#222] text-white p-3 rounded-lg w-full border border-gray-700 focus:outline-none focus:border-cyan-400"
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
                            className="bg-[#222] text-white p-3 rounded-lg w-full border border-gray-700 focus:outline-none focus:border-cyan-400"
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
                            className="bg-[#222] text-white p-3 rounded-lg placeholder-gray-500 w-full border border-gray-700 focus:outline-none focus:border-cyan-400"
                            placeholder="Pick-up"
                        />
                        <input
                            type="datetime-local"
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                            className="bg-[#222] text-white p-3 rounded-lg placeholder-gray-500 w-full border border-gray-700 focus:outline-none focus:border-cyan-400"
                            placeholder="Drop-off"
                        />

                        <button
                            onClick={isLoggedIn ? handleBooking : handleLoginAlert}
                            className={`w-full py-3 rounded-lg font-semibold transition-all ${isLoggedIn
                                ? "bg-cyan-400 text-black hover:bg-cyan-300 hover:shadow-[0_0_25px_rgba(34,211,238,0.5)]"
                                : "bg-blue-500 text-white cursor-not-allowed opacity-80 hover:opacity-100"
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

            <Modal
                title={modalContent?.title}
                open={isModalOpen}
                onOk={handleModalOk}
                onCancel={handleModalCancel}
                footer={[
                    <Button
                        key="ok"
                        onClick={handleModalOk}
                        {...modalStyles.confirmButtonProps}
                    >
                        OK
                    </Button>,
                ]}
                closable
                centered
                className="dark-modal"
                styles={{
                    header: {
                        borderBottom: "1px solid #374151",
                        paddingBottom: "12px",
                    },
                    body: {
                        padding: "16px 24px",
                    },
                    content: {
                        backgroundColor: "#1f2937",
                        color: "#f3f4f6",
                        borderRadius: "12px",
                    },
                    title: {
                        color: "#f3f4f6",
                        fontSize: "1.25rem",
                        fontWeight: "600",
                    },
                }}
            >
                <p className={modalStyles.contentClass}>{modalContent?.message}</p>
            </Modal>
        </div>
    );
}