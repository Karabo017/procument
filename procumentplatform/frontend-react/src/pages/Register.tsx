import React from 'react';
import { useNavigate } from 'react-router-dom';

export default function Register() {
  const navigate = useNavigate();
  const [firstName, setFirstName] = React.useState('');
  const [lastName, setLastName] = React.useState('');
  const [email, setEmail] = React.useState('');
  const [password, setPassword] = React.useState('');
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState<string | null>(null);

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    setLoading(true);
    try {
      const res = await fetch('/api/auth/register/manager', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ firstName, lastName, email, password })
      });
      const data = await res.json().catch(() => ({}));
      if (!res.ok || !data?.token) {
        setError(data?.message || 'Registration failed');
        return;
      }
      localStorage.setItem('auth.token', data.token);
      if (data.user) localStorage.setItem('auth.user', JSON.stringify(data.user));
      navigate('/manager');
    } catch (err: any) {
      setError(err?.message || 'Registration failed');
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="container">
      <h2>Register</h2>
      <form className="card" style={{ maxWidth: 540 }} onSubmit={onSubmit}>
        {error && <div className="error" style={{ color: '#b00020', marginBottom: 12 }}>{error}</div>}
        <label>First name</label>
        <input value={firstName} onChange={(e) => setFirstName(e.target.value)} placeholder="First name" required />
        <label>Last name</label>
        <input value={lastName} onChange={(e) => setLastName(e.target.value)} placeholder="Last name" required />
        <label>Email</label>
        <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} placeholder="you@example.com" required />
        <label>Password</label>
        <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Create a strong password" required />
        <button className="btn btn-primary" type="submit" disabled={loading}>
          {loading ? 'Creating…' : 'Create account'}
        </button>
      </form>
    </div>
  );
}
