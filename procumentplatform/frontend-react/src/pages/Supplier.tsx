import React from 'react';
import { useNavigate, Link } from 'react-router-dom';

export default function Supplier() {
  const navigate = useNavigate();
  const [company, setCompany] = React.useState('');
  const [taxId, setTaxId] = React.useState('');
  const [contact, setContact] = React.useState('');
  const [email, setEmail] = React.useState('');
  const [saving, setSaving] = React.useState(false);
  const [msg, setMsg] = React.useState<string | null>(null);

  async function onSubmit(e: React.FormEvent) {
    e.preventDefault();
    setSaving(true);
    setMsg(null);
    try {
      // Placeholder submit to API when available
      await new Promise(r => setTimeout(r, 600));
      setMsg('Profile saved. You can now browse and bid on tenders.');
    } finally {
      setSaving(false);
    }
  }

  return (
    <div className="container">
      <h2>Supplier Portal</h2>
      <p>Discover opportunities and submit bids securely.</p>
      <form className="card" style={{ maxWidth: 640 }} onSubmit={onSubmit}>
        {msg && <div className="muted" style={{ marginBottom: 12 }}>{msg}</div>}
        <label>Company name</label>
        <input value={company} onChange={e => setCompany(e.target.value)} placeholder="ABC (Pty) Ltd" required />
        <label>Tax ID / Registration</label>
        <input value={taxId} onChange={e => setTaxId(e.target.value)} placeholder="1234567890" required />
        <label>Contact person</label>
        <input value={contact} onChange={e => setContact(e.target.value)} placeholder="Name and surname" required />
        <label>Contact email</label>
        <input type="email" value={email} onChange={e => setEmail(e.target.value)} placeholder="you@example.com" required />
        <div className="actions" style={{ marginTop: 12 }}>
          <button className="btn btn-primary" type="submit" disabled={saving}>{saving ? 'Saving…' : 'Save profile'}</button>
          <Link className="btn" to="/tenders">Browse tenders</Link>
        </div>
      </form>
    </div>
  );
}
