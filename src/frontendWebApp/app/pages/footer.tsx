import { Car } from 'lucide-react';
import Link from 'next/link';

export default function Footer() {
    return (
        <footer className="bg-gray-950 border-t border-gray-900">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
                <div className="grid grid-cols-1 md:grid-cols-4 gap-8 mb-8">
                    <div className="col-span-1 md:col-span-1">
                        <Link href="/" className="flex items-center gap-2 mb-4">
                            <div className="bg-cyan-400 rounded-lg p-1.5">
                                <Car className="w-6 h-6 text-gray-950" />
                            </div>
                            <span className="text-2xl font-bold text-white">
                                Go<span className="text-cyan-400">Drive</span>
                            </span>
                        </Link>
                        <p className="text-gray-400 text-sm leading-relaxed">
                            Premium car rental platform. Drive your dream car today.
                        </p>
                    </div>

                    <div>
                        <h3 className="text-white font-semibold mb-4">Platform</h3>
                        <ul className="space-y-3">
                            <li>
                                <Link href="/browse" className="text-gray-400 hover:text-cyan-400 transition-colors text-sm">
                                    Browse Cars
                                </Link>
                            </li>
                            <li>
                                <Link href="/how-it-works" className="text-gray-400 hover:text-cyan-400 transition-colors text-sm">
                                    How it Works
                                </Link>
                            </li>
                            <li>
                                <Link href="/pricing" className="text-gray-400 hover:text-cyan-400 transition-colors text-sm">
                                    Pricing
                                </Link>
                            </li>
                        </ul>
                    </div>

                    <div>
                        <h3 className="text-white font-semibold mb-4">Company</h3>
                        <ul className="space-y-3">
                            <li>
                                <Link href="/about" className="text-gray-400 hover:text-cyan-400 transition-colors text-sm">
                                    About
                                </Link>
                            </li>
                            <li>
                                <Link href="/careers" className="text-gray-400 hover:text-cyan-400 transition-colors text-sm">
                                    Careers
                                </Link>
                            </li>
                            <li>
                                <Link href="/contact" className="text-gray-400 hover:text-cyan-400 transition-colors text-sm">
                                    Contact
                                </Link>
                            </li>
                        </ul>
                    </div>

                    <div>
                        <h3 className="text-white font-semibold mb-4">Legal</h3>
                        <ul className="space-y-3">
                            <li>
                                <Link href="/privacy" className="text-gray-400 hover:text-cyan-400 transition-colors text-sm">
                                    Privacy
                                </Link>
                            </li>
                            <li>
                                <Link href="/terms" className="text-gray-400 hover:text-cyan-400 transition-colors text-sm">
                                    Terms
                                </Link>
                            </li>
                            <li>
                                <Link href="/cookies" className="text-gray-400 hover:text-cyan-400 transition-colors text-sm">
                                    Cookies
                                </Link>
                            </li>
                        </ul>
                    </div>
                </div>

                <div className="border-t border-gray-900 pt-8">
                    <p className="text-gray-500 text-sm text-center">
                        © 2026 GoDrive. All rights reserved.
                    </p>
                </div>
            </div>
        </footer>
    );
}