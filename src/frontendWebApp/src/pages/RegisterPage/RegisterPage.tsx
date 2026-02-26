import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuthStore } from "../../features/auth/useAuth";
import { authApi } from "../../services/api/authApi";
import { Button } from "../../shared/components/Button";
import { Input } from "../../shared/components/Input";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";
import { getApiErrorMessage } from "../../shared/utils/apiError";

export function RegisterPage() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const setUser = useAuthStore((s) => s.setUser);
  const navigate = useNavigate();

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError("");
    setLoading(true);
    try {
      const { data } = await authApi.register({ name, email, password });
      setUser(data.user);
      navigate("/");
    } catch (err) {
      setError(getApiErrorMessage(err));
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="bg-white rounded-xl shadow-md p-8">
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Sign up</h1>
      <form onSubmit={handleSubmit} className="space-y-4">
        {error && <ErrorMessage message={error} />}
        <Input label="Name" value={name} onChange={(e) => setName(e.target.value)} required autoComplete="name" />
        <Input label="Email" type="email" value={email} onChange={(e) => setEmail(e.target.value)} required autoComplete="email" />
        <Input label="Password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required autoComplete="new-password" />
        <Button type="submit" fullWidth isLoading={loading}>Sign up</Button>
      </form>
      <p className="mt-4 text-center text-sm text-gray-600">Already have an account? <Link to="/login" className="text-blue-600 hover:underline">Log in</Link></p>
    </div>
  );
}
