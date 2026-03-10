"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { LogOut, PenIcon, Trash2 } from "lucide-react";
import Link from "next/link";
import { Modal, message } from "antd";

interface UserType {
    id: number;
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber?: string | null;
    dateOfBirth?: string | null;
    avatarPath?: {
        result: string;
    } | null;
    createdAt?: string | null;
}

export default function MyProfile() {
    const [user, setUser] = useState<UserType | null>(null);
    const [loading, setLoading] = useState(true);
    const [deleting, setDeleting] = useState(false);
    const router = useRouter();

    useEffect(() => {
        const token = localStorage.getItem("token");

        if (!token) {
            router.replace("/login");
            return;
        }

        fetch("https://godrive-ruc4.onrender.com/me", {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        })
            .then((res) => res.json())
            .then((data) => {
                if (data.isSuccess && data.data) {
                    setUser(data.data);
                } else {
                    router.replace("/login");
                }
            })
            .catch(() => router.replace("/login"))
            .finally(() => setLoading(false));
    }, [router]);

    const handleLogout = () => {
        localStorage.removeItem("token");
        router.replace("/login");
    };

    const handleDelete = () => {
        Modal.confirm({
            title: "Delete Account",
            content:
                "Are you sure you want to delete your account? This action cannot be undone.",
            okText: "Yes, Delete",
            okType: "danger",
            cancelText: "Cancel",
            async onOk() {
                const token = localStorage.getItem("token");
                if (!token) return;

                setDeleting(true);

                try {
                    const res = await fetch(
                        "https://godrive-ruc4.onrender.com/api/users/me",
                        {
                            method: "DELETE",
                            headers: {
                                Authorization: `Bearer ${token}`,
                            },
                        }
                    );

                    const data = await res.json();

                    if (data.isSuccess) {
                        message.success("Account deleted successfully");
                        localStorage.removeItem("token");
                        router.replace("/register");
                    } else {
                        message.error("Failed to delete account");
                    }
                } catch (error) {
                    console.error(error);
                    message.error("Server error");
                } finally {
                    setDeleting(false);
                }
            },
        });
    };

    if (loading) {
        return (
            <div className="min-h-screen bg-[#0d1117] flex items-center justify-center text-white text-xl">
                Loading...
            </div>
        );
    }

    if (!user) return null;

    const avatarUrl =
        user.avatarPath?.result || "/default-avatar.png";

    return (
        <div className="min-h-screen bg-[#0d1117] pt-32 px-6 text-white">
            <div className="max-w-4xl mx-auto bg-[#161b22] border border-[#30363d] rounded-2xl p-10 shadow-xl">

                <div className="flex flex-col md:flex-row items-center gap-8">

                    <div className="relative w-40 h-40">
                        <img
                            src={avatarUrl}
                            alt="avatar"
                            className="rounded-full w-40 h-40 object-cover border-4 border-cyan-500"
                            onError={(e) =>
                            ((e.target as HTMLImageElement).src =
                                "/default-avatar.png")
                            }
                        />
                    </div>

                    <div className="flex-1 text-center md:text-left">
                        <h1 className="text-3xl font-bold">
                            {user.firstName} {user.lastName}
                        </h1>

                        <p className="text-gray-400 mt-2">
                            @{user.userName}
                        </p>

                        <p className="text-gray-400 mt-2">
                            {user.email}
                        </p>

                        <p className="text-gray-500 mt-3">
                            Member since{" "}
                            {user.createdAt
                                ? new Date(
                                    user.createdAt
                                ).toLocaleDateString()
                                : "-"}
                        </p>
                    </div>

                    <div className="mt-6 md:mt-0 flex flex-col gap-4 w-full md:w-auto">

                        <button
                            onClick={handleLogout}
                            className="flex items-center justify-center gap-2 bg-red-600 hover:bg-red-700 transition px-6 py-2 rounded-xl font-semibold"
                        >
                            <LogOut size={18} />
                            Logout
                        </button>

                        <Link href="/edituser">
                            <button className="w-full flex items-center justify-center gap-2 bg-blue-600 hover:bg-blue-700 transition px-6 py-2 rounded-xl font-semibold">
                                <PenIcon size={18} />
                                Edit Profile
                            </button>
                        </Link>

                        <button
                            onClick={handleDelete}
                            disabled={deleting}
                            className="flex items-center justify-center gap-2 bg-red-800 hover:bg-red-900 transition px-6 py-2 rounded-xl font-semibold"
                        >
                            <Trash2 size={18} />
                            Delete Account
                        </button>

                    </div>
                </div>

                <div className="mt-12 border-t border-[#30363d] pt-8 grid md:grid-cols-2 gap-8 text-gray-300">

                    <div>
                        <p className="text-gray-500 text-sm">First Name</p>
                        <p className="text-lg">{user.firstName || "-"}</p>
                    </div>

                    <div>
                        <p className="text-gray-500 text-sm">Last Name</p>
                        <p className="text-lg">{user.lastName || "-"}</p>
                    </div>

                    <div>
                        <p className="text-gray-500 text-sm">Phone</p>
                        <p className="text-lg">{user.phoneNumber || "-"}</p>
                    </div>

                    <div>
                        <p className="text-gray-500 text-sm">Birth Date</p>
                        <p className="text-lg">
                            {user.dateOfBirth ? new Date(user.dateOfBirth).toLocaleDateString() : "-"}
                        </p>
                    </div>

                </div>

            </div>
        </div>
    );
}