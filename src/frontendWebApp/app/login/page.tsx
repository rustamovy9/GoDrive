'use client';

import { useState } from 'react';
import { Car, Eye, EyeOff, AlertCircle } from 'lucide-react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';

export default function LoginPage() {
    const router = useRouter();

    const [showPassword, setShowPassword] = useState(false);
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');

    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        setError('');
        setIsLoading(true);

        try {
            const response = await fetch(
                'https://godrive-ruc4.onrender.com/api/auth/login',
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        UserNameOrEmail: userName,
                        password: password,
                    }),
                }
            );

            const data = await response.json();

            if (!response.ok || !data.isAuthenticated) {
                throw new Error('Invalid username or password');
            }

            localStorage.setItem('token', data.token);
            localStorage.setItem('isAuthenticated', 'true');

            router.push('/');

        } catch (err: any) {
            setError(err.message || 'Login failed');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="mt-[50px] min-h-screen bg-gray-950 flex items-center justify-center px-4">
            <div className="w-full max-w-md">

                <div className="flex justify-center mb-8">
                    <Link href="/" className="flex items-center gap-2">
                        <div className="bg-cyan-400 rounded-lg p-2">
                            <Car className="w-6 h-6 text-gray-950" />
                        </div>
                        <span className="text-2xl font-bold text-white">
                            Go<span className="text-cyan-400">Drive</span>
                        </span>
                    </Link>
                </div>

                <div className="bg-gray-900 border border-gray-800 rounded-2xl p-8">

                    <h1 className="text-3xl font-bold text-white mb-6 text-center">
                        Welcome back
                    </h1>

                    {error && (
                        <div className="mb-4 bg-red-500/10 border border-red-500/50 p-4 rounded-xl flex gap-2 text-red-400">
                            <AlertCircle className="w-5 h-5" />
                            <p>{error}</p>
                        </div>
                    )}

                    <form onSubmit={handleSubmit} className="space-y-6">

                        <div>
                            <label className="block text-sm text-gray-300 mb-2">
                                Username or Email
                            </label>
                            <input
                                type="text"
                                value={userName}
                                onChange={(e) => setUserName(e.target.value)}
                                required
                                className="w-full bg-gray-950 border border-gray-800 rounded-xl px-4 py-3 text-white focus:outline-none focus:ring-2 focus:ring-cyan-400"
                                placeholder="Enter username or email"
                            />
                        </div>

                        <div className="relative">
                            <label className="block text-sm text-gray-300 mb-2">
                                Password
                            </label>

                            <input
                                type={showPassword ? 'text' : 'password'}
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                                className="w-full bg-gray-950 border border-gray-800 rounded-xl px-4 py-3 text-white pr-12 focus:outline-none focus:ring-2 focus:ring-cyan-400"
                                placeholder="Enter password"
                            />

                            <button
                                type="button"
                                onClick={() => setShowPassword(!showPassword)}
                                className="absolute right-3 top-[42px] text-gray-400"
                            >
                                {showPassword ? <EyeOff /> : <Eye />}
                            </button>
                        </div>

                        <button
                            type="submit"
                            disabled={isLoading}
                            className="w-full bg-cyan-400 hover:bg-cyan-500 text-gray-950 font-bold py-3 rounded-xl transition duration-300"
                        >
                            {isLoading ? 'Signing in...' : 'Sign In'}
                        </button>

                    </form>
                </div>

                <p className="text-center mt-6 text-gray-400">
                    Don&apos;t have an account?{' '}
                    <Link
                        href="/signup"
                        className="text-cyan-400 hover:text-cyan-300 font-medium"
                    >
                        Sign up
                    </Link>
                </p>

            </div>
        </div>
    );
}