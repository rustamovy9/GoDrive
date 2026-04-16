"use client";

import { useEffect, useState } from "react";
import { CheckCheck, Trash2 } from "lucide-react";
import { useTranslations } from "next-intl";

type Notification = {
    id: number;
    title: string;
    message: string;
    isRead: boolean;
    createdAt: string;
};

export default function NotificationsPage() {
    const t = useTranslations("NotificationsPage");

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
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchNotifications();
    }, []);

    const markAsRead = async (id: number) => {
        await fetch(
            `https://godrive-5r3o.onrender.com/api/notifications/${id}/read`,
            {
                method: "PUT",
                headers: { Authorization: `Bearer ${getToken()}` },
            }
        );

        setNotifications((prev) =>
            prev.map((n) =>
                n.id === id ? { ...n, isRead: true } : n
            )
        );
    };

    const markAllAsRead = async () => {
        await fetch(
            "https://godrive-5r3o.onrender.com/api/notifications/read-all",
            {
                method: "PUT",
                headers: { Authorization: `Bearer ${getToken()}` },
            }
        );

        setNotifications((prev) =>
            prev.map((n) => ({ ...n, isRead: true }))
        );
    };

    const deleteNotification = async (id: number) => {
        await fetch(
            `https://godrive-5r3o.onrender.com/api/notifications/${id}`,
            {
                method: "DELETE",
                headers: { Authorization: `Bearer ${getToken()}` },
            }
        );

        setNotifications((prev) =>
            prev.filter((n) => n.id !== id)
        );
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

        if (days > 0) return t("dayAgo", { count: days });
        if (hours > 0) return t("hourAgo", { count: hours });
        return t("justNow");
    };

    return (
        <main className="p-6 md:p-10 text-white">
            <div className="flex items-center justify-between mb-8">
                <h1 className="text-3xl font-bold">{t("title")}</h1>

                <button
                    onClick={() => setShowReadAllModal(true)}
                    className="flex items-center gap-2 text-sm px-3 py-2 rounded-lg bg-cyan-500/10 text-cyan-400"
                >
                    <CheckCheck size={16} />
                    {t("markAll")}
                </button>
            </div>

            {loading ? (
                <p className="text-zinc-400">{t("loading")}</p>
            ) : notifications.length === 0 ? (
                <p className="text-zinc-500">{t("noNotifications")}</p>
            ) : (
                <div className="space-y-4">
                    {notifications.map((n) => (
                        <div key={n.id} className="flex gap-4 p-4 rounded-xl border border-zinc-800 bg-zinc-900/50">
                            
                            <div className={`mt-2 w-2.5 h-2.5 rounded-full ${n.isRead ? "bg-zinc-600" : "bg-cyan-400"}`} />

                            <div className="flex-1 cursor-pointer" onClick={() => !n.isRead && markAsRead(n.id)}>
                                <p className={n.isRead ? "text-zinc-400" : "text-white"}>
                                    <span className="font-semibold">{n.title}</span> — {n.message}
                                </p>

                                <p className="text-xs text-zinc-500 mt-1">
                                    {formatTime(n.createdAt)}
                                </p>
                            </div>

                            <div className="flex gap-2">
                                {!n.isRead && (
                                    <button onClick={() => markAsRead(n.id)}>
                                        ✓
                                    </button>
                                )}

                                <button onClick={() => {
                                    setSelectedId(n.id);
                                    setShowDeleteModal(true);
                                }}>
                                    <Trash2 size={16} />
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {showDeleteModal && (
                <div className="fixed inset-0 bg-black/60 flex items-center justify-center">
                    <div className="bg-zinc-900 p-6 rounded-xl w-[320px]">
                        <h2>{t("deleteTitle")}</h2>
                        <p className="text-sm text-zinc-400">{t("deleteDesc")}</p>

                        <div className="flex justify-end gap-2 mt-4">
                            <button onClick={() => setShowDeleteModal(false)}>
                                {t("cancel")}
                            </button>
                            <button onClick={confirmDelete}>
                                {t("delete")}
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {showReadAllModal && (
                <div className="fixed inset-0 bg-black/60 flex items-center justify-center">
                    <div className="bg-zinc-900 p-6 rounded-xl w-[320px]">
                        <h2>{t("readAllTitle")}</h2>
                        <p className="text-sm text-zinc-400">{t("readAllDesc")}</p>

                        <div className="flex justify-end gap-2 mt-4">
                            <button onClick={() => setShowReadAllModal(false)}>
                                {t("cancel")}
                            </button>
                            <button onClick={confirmReadAll}>
                                {t("confirm")}
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </main>
    );
}