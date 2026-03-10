'use client';

import { useState } from 'react';
import { Car, Eye, EyeOff, AlertCircle, CheckCircle } from 'lucide-react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';

export default function SignupPage() {
    const router = useRouter();

    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);

    const [passwordError, setPasswordError] = useState('');
    const [apiError, setApiError] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        userName: '',
        email: '',
        phoneNumber: '',
        dateOfBirth: '',
        password: '',
        confirmPassword: '',
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;

        setFormData(prev => ({
            ...prev,
            [name]: value,
        }));

        setPasswordError('');
        setApiError('');
    };

    const validatePasswords = () => {
        if (formData.password.length < 8) {
            setPasswordError('Password must be at least 8 characters');
            return false;
        }

        if (formData.password !== formData.confirmPassword) {
            setPasswordError('Passwords do not match');
            return false;
        }

        return true;
    };

    const validateAgeAndPhone = () => {
        const birthDate = new Date(formData.dateOfBirth);
        const today = new Date();

        let age = today.getFullYear() - birthDate.getFullYear();
        const monthDiff = today.getMonth() - birthDate.getMonth();

        if (
            monthDiff < 0 ||
            (monthDiff === 0 && today.getDate() < birthDate.getDate())
        ) {
            age--;
        }

        if (age < 18) {
            setApiError('You must be at least 18 years old to register');
            return false;
        }

        const phoneRegex = /^\+?[\d\s-]{10,}$/;
        if (!phoneRegex.test(formData.phoneNumber)) {
            setApiError('Please enter a valid phone number');
            return false;
        }

        return true;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        setApiError('');
        setSuccessMessage('');

        if (!validatePasswords()) return;
        if (!validateAgeAndPhone()) return;

        setIsLoading(true);

        try {
            const response = await fetch(
                'https://godrive-ruc4.onrender.com/api/auth/register',
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        userName: formData.userName,
                        firstName: formData.firstName,
                        lastName: formData.lastName,
                        email: formData.email,
                        phoneNumber: formData.phoneNumber,
                        dateOfBirth: formData.dateOfBirth,
                        password: formData.password,
                        confirmPassword: formData.confirmPassword,
                    }),
                }
            );

            const message = await response.text();

            if (!response.ok) {
                throw new Error(message || 'Registration failed');
            }

            setSuccessMessage(message || 'User registered successfully');

            setTimeout(() => {
                router.push('/login');
            }, 2000);

        } catch (error: any) {
            setApiError(error.message || 'Something went wrong');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="min-h-screen mt-[50px] bg-gray-950 flex items-center justify-center px-4 py-12">
            <div className="w-full max-w-lg">

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

                <h1 className="text-3xl font-bold text-white text-center mb-6">
                    Create your account
                </h1>

                {successMessage && (
                    <div className="mb-4 bg-green-500/10 border border-green-500/50 p-4 rounded-xl flex gap-2 text-green-400">
                        <CheckCircle className="w-5 h-5" />
                        {successMessage}
                    </div>
                )}

                {apiError && (
                    <div className="mb-4 bg-red-500/10 border border-red-500/50 p-4 rounded-xl flex gap-2 text-red-400">
                        <AlertCircle className="w-5 h-5" />
                        {apiError}
                    </div>
                )}

                <form
                    onSubmit={handleSubmit}
                    className="space-y-4 bg-gray-900 p-8 rounded-2xl border border-gray-800"
                >

                    <input name="firstName" placeholder="First Name"
                        value={formData.firstName}
                        onChange={handleChange}
                        required
                        className="input-style" />

                    <input name="lastName" placeholder="Last Name"
                        value={formData.lastName}
                        onChange={handleChange}
                        required
                        className="input-style" />

                    <input name="userName" placeholder="Username"
                        value={formData.userName}
                        onChange={handleChange}
                        required
                        minLength={3}
                        className="input-style" />

                    <input type="email" name="email" placeholder="Email"
                        value={formData.email}
                        onChange={handleChange}
                        required
                        className="input-style" />

                    <input type="tel" name="phoneNumber" placeholder="+992 00 000 0000"
                        value={formData.phoneNumber}
                        onChange={handleChange}
                        required
                        className="input-style" />

                    <input type="date" name="dateOfBirth"
                        value={formData.dateOfBirth}
                        onChange={handleChange}
                        required
                        className="input-style" />

                    <div className="relative">
                        <input
                            type={showPassword ? 'text' : 'password'}
                            name="password"
                            placeholder="Password"
                            value={formData.password}
                            onChange={handleChange}
                            required
                            className="input-style pr-12"
                        />
                        <button
                            type="button"
                            onClick={() => setShowPassword(!showPassword)}
                            className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400"
                        >
                            {showPassword ? <EyeOff /> : <Eye />}
                        </button>
                    </div>

                    <div className="relative">
                        <input
                            type={showConfirmPassword ? 'text' : 'password'}
                            name="confirmPassword"
                            placeholder="Confirm Password"
                            value={formData.confirmPassword}
                            onChange={handleChange}
                            required
                            className="input-style pr-12"
                        />
                        <button
                            type="button"
                            onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                            className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400"
                        >
                            {showConfirmPassword ? <EyeOff /> : <Eye />}
                        </button>
                    </div>

                    {passwordError && (
                        <p className="text-red-400 text-sm">{passwordError}</p>
                    )}

                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full bg-cyan-400 hover:bg-cyan-500 text-gray-950 font-bold py-3 rounded-xl transition"
                    >
                        {isLoading ? 'Creating...' : 'Create Account'}
                    </button>
                    <p className="text-center mt-6 text-gray-400">
                        Already, have an account?{' '}
                        <Link
                            href="/login"
                            className="text-cyan-400 hover:text-cyan-300 font-medium"
                        >
                            Login
                        </Link>
                    </p>
                </form>
            </div>


            <style jsx>{`
                .input-style {
                    width: 100%;
                    background: #0f172a;
                    border: 1px solid #1f2937;
                    padding: 12px 16px;
                    border-radius: 12px;
                    color: white;
                }

                .input-style:focus {
                    outline: none;
                    border-color: #22d3ee;
                }
            `}</style>
        </div>
    );
}