"use client";

import { useEffect, useState } from "react";

interface User {
    id: number;
    userName: string;
    firstName: string;
    lastName: string;
    email: string | null;
    phoneNumber: string;
    avatarPath: string | null;
}

export default function UsersPage() {
    const [users, setUsers] = useState<User[]>([]);
    const [token, setToken] = useState("");
    const [loading, setLoading] = useState(false);

    const [deleteUserId, setDeleteUserId] = useState<number | null>(null);
    const [editUser, setEditUser] = useState<User | null>(null);

    const [roleUserId, setRoleUserId] = useState<number | null>(null);
    const [selectedRole, setSelectedRole] = useState("User");

    const [avatar, setAvatar] = useState<File | null>(null);

    const DEFAULT_AVATAR =
        "https://img.freepik.com/premium-vector/user-profile-icon-flat-style-member-avatar-vector-illustration-isolated-background-human-permission-sign-business-concept_157943-15752.jpg";

    useEffect(() => {
        const storedToken = localStorage.getItem("token");
        if (storedToken) setToken(storedToken);
    }, []);

    const fetchUsers = async (authToken: string) => {
        setLoading(true);
        try {
            const res = await fetch("https://godrive-5r3o.onrender.com/api/users", {
                headers: {
                    Authorization: `Bearer ${authToken}`,
                },
            });
            const data = await res.json();
            setUsers(data.data?.data || []);
        } catch (error) {
            console.error(error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (token) fetchUsers(token);
    }, [token]);

    const confirmDeleteUser = async () => {
        if (!deleteUserId) return;

        await fetch(`https://godrive-5r3o.onrender.com/api/users/${deleteUserId}`, {
            method: "DELETE",
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });

        setDeleteUserId(null);
        fetchUsers(token);
    };

    const updateUser = async () => {
        if (!editUser) return;

        const formData = new FormData();
        formData.append("FirstName", editUser.firstName);
        formData.append("LastName", editUser.lastName);
        formData.append("PhoneNumber", editUser.phoneNumber);
        formData.append("Address", "Updated");

        if (avatar) {
            formData.append("AvatarPath", avatar);
        }

        await fetch(`https://godrive-5r3o.onrender.com/api/users/${editUser.id}`, {
            method: "PUT",
            headers: {
                Authorization: `Bearer ${token}`,
            },
            body: formData,
        });

        setEditUser(null);
        fetchUsers(token);
    };

    const assignRole = async () => {
        if (!roleUserId) return;

        await fetch(
            `https://godrive-5r3o.onrender.com/api/users/${roleUserId}/assign-role?roleName=${selectedRole}`,
            {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        );

        setRoleUserId(null);
        fetchUsers(token);
    };

    return (
        <div className="p-6 text-white">
            <h1 className="text-2xl font-bold mb-6">Users Management</h1>

            <div className="hidden md:block bg-black/40 border border-gray-800 rounded-xl overflow-hidden relative">
                {loading && (
                    <div className="absolute inset-0 bg-black/50 flex items-center justify-center z-10">
                        <div className="border-4 border-t-white border-gray-600 rounded-full w-12 h-12 animate-spin"></div>
                    </div>
                )}
                <table className="w-full">
                    <thead className="border-b border-gray-800 text-gray-400">
                        <tr>
                            <th className="p-4 text-left">User</th>
                            <th className="p-4 text-left">Email</th>
                            <th className="p-4 text-left">Phone</th>
                            <th className="p-4 text-left">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user) => (
                            <tr
                                key={user.id}
                                className="border-b border-gray-800 hover:bg-white/5"
                            >
                                <td className="p-4 flex items-center gap-3">
                                    <img
                                        src={user.avatarPath || DEFAULT_AVATAR}
                                        className="w-10 h-10 rounded-full object-cover"
                                    />
                                    <div>
                                        <p className="font-semibold">
                                            {user.firstName} {user.lastName}
                                        </p>
                                        <p className="text-gray-400 text-sm">@{user.userName}</p>
                                    </div>
                                </td>
                                <td className="p-4">{user.email || "No email"}</td>
                                <td className="p-4">{user.phoneNumber}</td>
                                <td className="p-4 space-x-4">
                                    <button
                                        onClick={() => setEditUser(user)}
                                        className="text-blue-400 hover:text-blue-600"
                                    >
                                        Edit
                                    </button>
                                    <button
                                        onClick={() => {
                                            setRoleUserId(user.id);
                                            setSelectedRole("User");
                                        }}
                                        className="text-yellow-400 hover:text-yellow-600"
                                    >
                                        Assign Role
                                    </button>
                                    <button
                                        onClick={() => setDeleteUserId(user.id)}
                                        className="text-red-500 hover:text-red-700"
                                    >
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            <div className="md:hidden space-y-4">
                {users.map((user) => (
                    <div
                        key={user.id}
                        className="bg-black/40 border border-gray-800 rounded-xl p-4 flex flex-col gap-2"
                    >
                        <div className="flex items-center gap-3">
                            <img
                                src={user.avatarPath || DEFAULT_AVATAR}
                                className="w-12 h-12 rounded-full object-cover"
                            />
                            <div>
                                <p className="font-semibold">{user.firstName} {user.lastName}</p>
                                <p className="text-gray-400 text-sm">@{user.userName}</p>
                            </div>
                        </div>
                        <p className="text-gray-300"><strong>Email:</strong> {user.email || "No email"}</p>
                        <p className="text-gray-300"><strong>Phone:</strong> {user.phoneNumber}</p>
                        <div className="flex gap-2 mt-2 flex-wrap">
                            <button
                                onClick={() => setEditUser(user)}
                                className="px-3 py-1 text-sm bg-blue-500 rounded hover:bg-blue-600"
                            >
                                Edit
                            </button>
                            <button
                                onClick={() => {
                                    setRoleUserId(user.id);
                                    setSelectedRole("User");
                                }}
                                className="px-3 py-1 text-sm bg-yellow-500 rounded hover:bg-yellow-600"
                            >
                                Assign Role
                            </button>
                            <button
                                onClick={() => setDeleteUserId(user.id)}
                                className="px-3 py-1 text-sm bg-red-500 rounded hover:bg-red-600"
                            >
                                Delete
                            </button>
                        </div>
                    </div>
                ))}
            </div>

            {deleteUserId && (
                <div className="fixed inset-0 bg-black/60 flex items-center justify-center">
                    <div className="bg-gray-900 p-6 rounded-xl w-96 text-center">
                        <h2 className="text-xl mb-4">Delete User?</h2>
                        <div className="flex justify-center gap-4">
                            <button
                                onClick={() => setDeleteUserId(null)}
                                className="px-4 py-2 bg-gray-700 rounded"
                            >
                                Cancel
                            </button>
                            <button
                                onClick={confirmDeleteUser}
                                className="px-4 py-2 bg-red-600 rounded"
                            >
                                Delete
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {editUser && (
                <div className="fixed inset-0 bg-black/60 flex items-center justify-center">
                    <div className="bg-gray-900 p-6 rounded-xl w-96 space-y-4">
                        <h2 className="text-xl font-bold">Edit User</h2>
                        <input
                            value={editUser.firstName}
                            onChange={(e) =>
                                setEditUser({ ...editUser, firstName: e.target.value })
                            }
                            className="w-full p-2 bg-black border border-gray-700 rounded"
                            placeholder="First Name"
                        />
                        <input
                            value={editUser.lastName}
                            onChange={(e) =>
                                setEditUser({ ...editUser, lastName: e.target.value })
                            }
                            className="w-full p-2 bg-black border border-gray-700 rounded"
                            placeholder="Last Name"
                        />
                        <input
                            value={editUser.phoneNumber}
                            onChange={(e) =>
                                setEditUser({ ...editUser, phoneNumber: e.target.value })
                            }
                            className="w-full p-2 bg-black border border-gray-700 rounded"
                        />
                        <input
                            type="file"
                            onChange={(e) => {
                                if (e.target.files) setAvatar(e.target.files[0]);
                            }}
                        />
                        <div className="flex gap-4 justify-end">
                            <button
                                onClick={() => setEditUser(null)}
                                className="px-4 py-2 bg-gray-700 rounded"
                            >
                                Cancel
                            </button>
                            <button
                                onClick={updateUser}
                                className="px-4 py-2 bg-blue-600 rounded"
                            >
                                Update
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {roleUserId && (
                <div className="fixed inset-0 bg-black/60 flex items-center justify-center">
                    <div className="bg-gray-900 p-6 rounded-xl w-96 space-y-4 text-center">
                        <h2 className="text-xl font-bold">Assign Role</h2>
                        <select
                            value={selectedRole}
                            onChange={(e) => setSelectedRole(e.target.value)}
                            className="w-full p-2 bg-black border border-gray-700 rounded"
                        >
                            <option value="Owner">Owner</option>
                            <option value="Admin">Admin</option>
                            <option value="User">User</option>
                        </select>
                        <div className="flex justify-center gap-4">
                            <button
                                onClick={() => setRoleUserId(null)}
                                className="px-4 py-2 bg-gray-700 rounded"
                            >
                                Cancel
                            </button>
                            <button
                                onClick={assignRole}
                                className="px-4 py-2 bg-yellow-600 rounded"
                            >
                                Assign
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}