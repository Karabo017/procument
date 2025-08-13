import React from 'react';
import { useNavigate, Link, useLocation } from 'react-router-dom';

export default function Login() {
  const navigate = useNavigate();
  const location = useLocation();
  const roleHint = React.useMemo(() => new URLSearchParams(location.search).get('role')?.toLowerCase() || '', [location.search]);
  const [email, setEmail] = React.useState('');
  const [password, setPassword] = React.useState('');
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState<string | null>(null);

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      const res = await fetch('/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
      });
      const data = await res.json().catch(() => ({}));
      if (!res.ok || !data?.token) {
        setError(data?.message || 'Invalid email or password');
        return;
      }
      // Save token and basic user info
      localStorage.setItem('auth.token', data.token);
      if (data.user) localStorage.setItem('auth.user', JSON.stringify(data.user));

      // Route by role if present
  const role = (data.user?.userType || roleHint || '').toLowerCase();
      if (role === 'supplier') navigate('/supplier');
      else if (role === 'buyer') navigate('/buyer');
      else if (role === 'platformmanager' || role === 'manager') navigate('/manager');
      else navigate('/');
    } catch (err: any) {
      setError(err?.message || 'Login failed');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="container">
      <h2>Login</h2>
      <form className="card" style={{ maxWidth: 420 }} onSubmit={onSubmit}>
        {error && <div className="error" style={{ color: '#b00020', marginBottom: 12 }}>{error}</div>}
        <label>Email</label>
        <input
          type="email"
          placeholder="you@example.com"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <label>Password</label>
        <input
          type="password"
          placeholder="••••••••"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <button className="btn btn-primary" type="submit" disabled={loading}>
          {loading ? 'Signing in…' : 'Sign in'}
        </button>
        <div style={{ marginTop: 12 }}>
          No account? <Link to="/register">Register</Link>
        </div>
      </form>
    </div>
  );
}
