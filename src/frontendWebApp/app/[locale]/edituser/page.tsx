"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";

interface UserType {
    id: number;
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber?: string | null;
    address?: string | null;
    dateOfBirth?: string | null;
    createdAt?: string | null;
    avatarPath?: {
        result: string;
    } | null;
}

export default function EditProfile() {
    const router = useRouter();

    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [preview, setPreview] = useState("/default-avatar.png");
    const [file, setFile] = useState<File | null>(null);

    const [form, setForm] = useState({
        firstName: "",
        lastName: "",
        phoneNumber: "",
        address: "",
    });

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
                    const user: UserType = data.data;

                    setForm({
                        firstName: user.firstName || "",
                        lastName: user.lastName || "",
                        phoneNumber: user.phoneNumber || "",
                        address: user.address || "",
                    });

                    if (user.avatarPath?.result) {
                        setPreview(user.avatarPath.result);
                    }
                } else {
                    router.replace("/login");
                }
            })
            .catch(() => router.replace("/login"))
            .finally(() => setLoading(false));
    }, [router]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value,
        });
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const selectedFile = e.target.files?.[0];
        if (selectedFile) {
            setFile(selectedFile);
            setPreview(URL.createObjectURL(selectedFile));
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        const token = localStorage.getItem("token");
        if (!token) return;

        setSaving(true);

        const formData = new FormData();
        formData.append("FirstName", form.firstName);
        formData.append("LastName", form.lastName);
        formData.append("PhoneNumber", form.phoneNumber);
        formData.append("Address", form.address);

        if (file) {
            formData.append("File", file);
        }

        try {
            const res = await fetch(
                "https://godrive-ruc4.onrender.com/me",
                {
                    method: "PUT",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                    body: formData,
                }
            );

            const data = await res.json();

            if (data.isSuccess) {
                router.push("/myprofile");
            } else {
                alert("Update failed");
            }
        } catch {
            alert("Server error");
        } finally {
            setSaving(false);
        }
    };

    if (loading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-[#0d1117] text-white">
                Loading...
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-[#0d1117] text-white pt-32 px-6">
            <div className="max-w-3xl mx-auto bg-[#161b22] border border-[#30363d] rounded-2xl p-8 shadow-xl">

                <h1 className="text-3xl font-bold mb-8">Edit Profile</h1>

                <form onSubmit={handleSubmit} className="space-y-6">

                    <div className="flex flex-col items-center gap-4">
                        <img
                            src={preview}
                            alt="avatar"
                            className="w-32 h-32 rounded-full object-cover border-4 border-cyan-500"
                            onError={(e) => {
                                (e.currentTarget as HTMLImageElement).src =
                                    "/default-avatar.png";
                            }}
                        />

                        <input
                            type="file"
                            accept="image/*"
                            onChange={handleFileChange}
                            className="text-sm text-gray-400"
                        />
                    </div>

                    <div>
                        <label className="block text-gray-400 mb-2">
                            First Name
                        </label>
                        <input
                            type="text"
                            name="firstName"
                            value={form.firstName}
                            onChange={handleChange}
                            className="w-full bg-[#0d1117] border border-[#30363d] rounded-xl px-4 py-2 focus:border-cyan-500 outline-none"
                        />
                    </div>

                    <div>
                        <label className="block text-gray-400 mb-2">
                            Last Name
                        </label>
                        <input
                            type="text"
                            name="lastName"
                            value={form.lastName}
                            onChange={handleChange}
                            className="w-full bg-[#0d1117] border border-[#30363d] rounded-xl px-4 py-2 focus:border-cyan-500 outline-none"
                        />
                    </div>

                    <div>
                        <label className="block text-gray-400 mb-2">
                            Phone Number
                        </label>
                        <input
                            type="text"
                            name="phoneNumber"
                            value={form.phoneNumber}
                            onChange={handleChange}
                            className="w-full bg-[#0d1117] border border-[#30363d] rounded-xl px-4 py-2 focus:border-cyan-500 outline-none"
                        />
                    </div>

                    <div>
                        <label className="block text-gray-400 mb-2">
                            Address
                        </label>
                        <input
                            type="text"
                            name="address"
                            value={form.address}
                            onChange={handleChange}
                            className="w-full bg-[#0d1117] border border-[#30363d] rounded-xl px-4 py-2 focus:border-cyan-500 outline-none"
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={saving}
                        className="w-full bg-cyan-600 hover:bg-cyan-700 transition rounded-xl py-3 font-semibold"
                    >
                        {saving ? "Saving..." : "Save Changes"}
                    </button>

                </form>
            </div>
        </div>
    );
}