"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { Car } from "lucide-react";

export default function LoginPage() {
    const [usernameOrEmail, setUsernameOrEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);
    const router = useRouter();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");
        setLoading(true);

        try {
            const res = await fetch("https://godrive-5r3o.onrender.com/api/auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    userNameOrEmail: usernameOrEmail,
                    password: password,
                }),
            });

            const data = await res.json();

            if (res.ok) {
                localStorage.setItem("token", data.token);

                document.cookie = `token=${data.token}; path=/; max-age=${4 * 60 * 60}; SameSite=Lax`;

                window.location.href = "/";
                router.refresh();
            } else {
                setError(data.message || "Login failed. Please check your credentials.");
            }
        } catch (err) {
            setError("Connection error. Please try again.");
            console.error("Login error:", err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-black via-zinc-950 to-black">
            <div className="bg-zinc-900/80 backdrop-blur-xl p-8 rounded-2xl border border-zinc-800 w-full max-w-md">

                <div className="text-center mb-8">
                    <div className="flex items-center justify-center gap-2 text-cyan-400 mb-4">
                        <Car size={32} />
                        <h1 className="text-2xl font-bold">GoDrive Admin</h1>
                    </div>
                    <p className="text-zinc-400">Sign in to your account</p>
                </div>

                {error && (
                    <div className="bg-red-500/10 border border-red-500/50 text-red-400 px-4 py-3 rounded-lg mb-6">
                        {error}
                    </div>
                )}

                <form onSubmit={handleLogin} className="space-y-6">

                    <div>
                        <label className="block text-sm text-zinc-400 mb-2">
                            Username or Email
                        </label>
                        <input
                            type="text"
                            value={usernameOrEmail}
                            onChange={(e) => setUsernameOrEmail(e.target.value)}
                            className="w-full bg-zinc-800 border border-zinc-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-cyan-500 transition"
                            placeholder="admin or admin@godrive.com"
                            required
                            autoComplete="username"
                        />
                    </div>

                    <div>
                        <label className="block text-sm text-zinc-400 mb-2">
                            Password
                        </label>
                        <input
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            className="w-full bg-zinc-800 border border-zinc-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-cyan-500 transition"
                            placeholder="••••••••"
                            required
                            autoComplete="current-password"
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={loading}
                        className="w-full bg-cyan-500 hover:bg-cyan-600 text-black font-semibold py-3 rounded-lg transition disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                        {loading ? "Signing in..." : "Sign In"}
                    </button>

                </form>

                <p className="text-center text-zinc-500 text-xs mt-6">
                    © 2026 GoDrive. All rights reserved.
                </p>

            </div>
        </div>
    );
}