"use client";

import { useState, useRef, useEffect } from "react";
import { SendHorizonal, Bot } from "lucide-react";

type Message = {
    role: "user" | "ai";
    text: string;
};

export default function ChatPage() {
    const [messages, setMessages] = useState<Message[]>([]);
    const [input, setInput] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(false);

    const bottomRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        bottomRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [messages, loading]);

    const sendMessage = async () => {
        if (!input.trim()) return;

        const userMessage: Message = { role: "user", text: input };

        setMessages((prev) => [...prev, userMessage]);
        setInput("");
        setLoading(true);

        try {
            const res = await fetch(
                "https://godrive-5r3o.onrender.com/api/ai/chat",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${localStorage.getItem("token")}`,
                    },
                    body: JSON.stringify({
                        message: userMessage.text,
                    }),
                }
            );

            const data = await res.json();

            setMessages((prev) => [
                ...prev,
                { role: "ai", text: data.reply },
            ]);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <main className="flex flex-col h-screen bg-zinc-950 text-white">
            <div className="flex items-center gap-2 px-4 py-3 border-b border-zinc-800 text-sm font-medium">
                <Bot size={18} />
                GoDrive AI
            </div>

            <div className="flex-1 overflow-y-auto px-4 py-3 space-y-3">
                {messages.map((msg, i) => (
                    <div
                        key={i}
                        className={`max-w-[40%] px-3 py-2 rounded-xl text-sm ${msg.role === "user"
                                ? "ml-auto bg-blue-600"
                                : "bg-zinc-800"
                            }`}
                    >
                        {msg.text}
                    </div>
                ))}

                {loading && (
                    <div className="bg-zinc-800 px-3 py-2 rounded-xl w-fit text-sm animate-pulse">
                        Typing...
                    </div>
                )}

                <div ref={bottomRef} />
            </div>

            <div className="px-3 py-2 border-t border-zinc-800 flex items-center gap-2">
                <input
                    value={input}
                    onChange={(e) => setInput(e.target.value)}
                    placeholder="Type a message..."
                    className="flex-1 bg-zinc-900 border border-zinc-700 rounded-lg px-3 py-2.5 text-sm outline-none focus:border-blue-500"
                    onKeyDown={(e) => e.key === "Enter" && sendMessage()}
                />

                <button
                    onClick={sendMessage}
                    className="p-2.5 rounded-lg bg-blue-600 hover:bg-blue-700 transition"
                >
                    <SendHorizonal size={16} />
                </button>
            </div>
        </main>
    );
}