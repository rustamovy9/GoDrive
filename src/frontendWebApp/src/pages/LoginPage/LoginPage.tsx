import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuthStore } from "../../features/auth/useAuth";
import { authApi } from "../../services/api/authApi";
import { Button } from "../../shared/components/Button";
import { Input } from "../../shared/components/Input";
import { ErrorMessage } from "../../widgets/ErrorMessage/ErrorMessage";
import { getApiErrorMessage } from "../../shared/utils/apiError";

export function LoginPage() {
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
      const { data } = await authApi.login(email, password);
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
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Log in</h1>
      <form onSubmit={handleSubmit} className="space-y-4">
        {error && <ErrorMessage message={error} />}
        <Input label="Email" type="email" value={email} onChange={(e) => setEmail(e.target.value)} required autoComplete="email" />
        <Input label="Password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required autoComplete="current-password" />
        <Button type="submit" fullWidth isLoading={loading}>Log in</Button>
      </form>
      <p className="mt-4 text-center text-sm text-gray-600">Don&apos;t have an account? <Link to="/register" className="text-blue-600 hover:underline">Sign up</Link></p>
    </div>
  );
}
