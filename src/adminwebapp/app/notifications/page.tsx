"use client";

import { useEffect, useState } from "react";
import { CheckCheck, Trash2 } from "lucide-react";

type Notification = {
    id: number;
    title: string;
    message: string;
    isRead: boolean;
    createdAt: string;
};

export default function NotificationsPage() {
    const [notifications, setNotifications] = useState<Notification[]>([]);
    const [loading, setLoading] = useState(true);

    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [showReadAllModal, setShowReadAllModal] = useState(false);
    const [selectedId, setSelectedId] = useState<number | null>(null);

    const getToken = () => localStorage.getItem("token");

    const fetchNotifications = async () => {
        try {
            const res = await fetch(
                "https://godrive-5r3o.onrender.com/api/notifications",
                {
                    headers: {
                        Authorization: `Bearer ${getToken()}`,
                    },
                }
            );

            const data = await res.json();
            setNotifications(data.data.data);
        } catch (err) {
            console.error("Fetch error:", err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchNotifications();
    }, []);

    const markAsRead = async (id: number) => {
        try {
            await fetch(
                `https://godrive-5r3o.onrender.com/api/notifications/${id}/read`,
                {
                    method: "PUT",
                    headers: {
                        Authorization: `Bearer ${getToken()}`,
                    },
                }
            );

            setNotifications((prev) =>
                prev.map((n) =>
                    n.id === id ? { ...n, isRead: true } : n
                )
            );
        } catch (err) {
            console.error(err);
        }
    };

    const markAllAsRead = async () => {
        try {
            await fetch(
                "https://godrive-5r3o.onrender.com/api/notifications/read-all",
                {
                    method: "PUT",
                    headers: {
                        Authorization: `Bearer ${getToken()}`,
                    },
                }
            );

            setNotifications((prev) =>
                prev.map((n) => ({ ...n, isRead: true }))
            );
        } catch (err) {
            console.error(err);
        }
    };

    const deleteNotification = async (id: number) => {
        try {
            await fetch(
                `https://godrive-5r3o.onrender.com/api/notifications/${id}`,
                {
                    method: "DELETE",
                    headers: {
                        Authorization: `Bearer ${getToken()}`,
                    },
                }
            );

            setNotifications((prev) =>
                prev.filter((n) => n.id !== id)
            );
        } catch (err) {
            console.error(err);
        }
    };

    const confirmDelete = async () => {
        if (!selectedId) return;

        await deleteNotification(selectedId);
        setShowDeleteModal(false);
        setSelectedId(null);
    };

    const confirmReadAll = async () => {
        await markAllAsRead();
        setShowReadAllModal(false);
    };

    const formatTime = (date: string) => {
        const diff = Math.floor(
            (new Date().getTime() - new Date(date).getTime()) / 1000
        );

        const hours = Math.floor(diff / 3600);
        const days = Math.floor(diff / 86400);

        if (days > 0) return `${days} day ago`;
        if (hours > 0) return `${hours} hour ago`;
        return "Just now";
    };

    return (
        <main className="p-6 md:p-10 text-white">
            <div className="flex items-center justify-between mb-8">
                <h1 className="text-3xl font-bold">Notifications</h1>

                <button
                    onClick={() => setShowReadAllModal(true)}
                    className="flex items-center gap-2 text-sm px-3 py-2 rounded-lg bg-cyan-500/10 text-cyan-400 hover:bg-cyan-500/20 transition"
                >
                    <CheckCheck size={16} />
                    Mark all as read
                </button>
            </div>

            {loading ? (
                <p className="text-zinc-400">Loading...</p>
            ) : notifications.length === 0 ? (
                <p className="text-zinc-500">No notifications</p>
            ) : (
                <div className="space-y-4">
                    {notifications.map((n) => (
                        <div
                            key={n.id}
                            className="group flex items-start gap-4 p-4 rounded-xl border border-zinc-800 bg-zinc-900/50 hover:bg-zinc-800/60 transition"
                        >
                            <div
                                className={`mt-2 w-2.5 h-2.5 rounded-full ${n.isRead ? "bg-zinc-600" : "bg-cyan-400"
                                    }`}
                            />

                            <div
                                className="flex-1 cursor-pointer"
                                onClick={() => !n.isRead && markAsRead(n.id)}
                            >
                                <p
                                    className={`text-sm ${n.isRead ? "text-zinc-400" : "text-white"
                                        }`}
                                >
                                    <span className="font-semibold">{n.title}</span> —{" "}
                                    {n.message}
                                </p>

                                <p className="text-xs text-zinc-500 mt-1">
                                    {formatTime(n.createdAt)}
                                </p>
                            </div>

                            <div className="flex items-center gap-2 opacity-0 group-hover:opacity-100 transition">
                                {!n.isRead && (
                                    <button
                                        onClick={() => markAsRead(n.id)}
                                        className="p-2 rounded-lg hover:bg-cyan-500/10 text-cyan-400"
                                    >
                                        ✓
                                    </button>
                                )}

                                <button
                                    onClick={() => {
                                        setSelectedId(n.id);
                                        setShowDeleteModal(true);
                                    }}
                                    className="p-2 rounded-lg hover:bg-red-500/10 text-red-400"
                                >
                                    <Trash2 size={16} />
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {showDeleteModal && (
                <div className="fixed inset-0 bg-black/60 flex items-center justify-center z-50">
                    <div className="bg-zinc-900 p-6 rounded-xl border border-zinc-800 w-[320px]">
                        <h2 className="text-lg font-semibold mb-2">
                            Delete notification?
                        </h2>
                        <p className="text-sm text-zinc-400 mb-4">
                            This action cannot be undone.
                        </p>

                        <div className="flex justify-end gap-2">
                            <button
                                onClick={() => setShowDeleteModal(false)}
                                className="px-3 py-2 text-sm rounded-lg bg-zinc-800 hover:bg-zinc-700"
                            >
                                Cancel
                            </button>

                            <button
                                onClick={confirmDelete}
                                className="px-3 py-2 text-sm rounded-lg bg-red-500/20 text-red-400 hover:bg-red-500/30"
                            >
                                Delete
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {showReadAllModal && (
                <div className="fixed inset-0 bg-black/60 flex items-center justify-center z-50">
                    <div className="bg-zinc-900 p-6 rounded-xl border border-zinc-800 w-[320px]">
                        <h2 className="text-lg font-semibold mb-2">
                            Mark all as read?
                        </h2>
                        <p className="text-sm text-zinc-400 mb-4">
                            All notifications will be marked as read.
                        </p>

                        <div className="flex justify-end gap-2">
                            <button
                                onClick={() => setShowReadAllModal(false)}
                                className="px-3 py-2 text-sm rounded-lg bg-zinc-800 hover:bg-zinc-700"
                            >
                                Cancel
                            </button>

                            <button
                                onClick={confirmReadAll}
                                className="px-3 py-2 text-sm rounded-lg bg-cyan-500/20 text-cyan-400 hover:bg-cyan-500/30"
                            >
                                Confirm
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </main>
    );
}