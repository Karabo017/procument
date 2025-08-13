import React from 'react';
import { Link } from 'react-router-dom';

export default function Buyer() {
  const [title, setTitle] = React.useState('');
  const [category, setCategory] = React.useState('');
  const [deadline, setDeadline] = React.useState('');
  const [value, setValue] = React.useState('');
  const [msg, setMsg] = React.useState<string | null>(null);
  const [saving, setSaving] = React.useState(false);

  async function onCreate(e: React.FormEvent) {
    e.preventDefault();
    setSaving(true);
    setMsg(null);
    try {
      // Placeholder create tender call
      await new Promise(r => setTimeout(r, 600));
      setMsg('Tender created (demo). It will appear in the list once API is wired.');
      setTitle(''); setCategory(''); setDeadline(''); setValue('');
    } finally {
      setSaving(false);
    }
  }

  return (
    <div className="container">
      <h2>Buyer Portal</h2>
      <p>Publish, evaluate, and award tenders efficiently.</p>
      <form className="card" style={{ maxWidth: 720 }} onSubmit={onCreate}>
        {msg && <div className="muted" style={{ marginBottom: 12 }}>{msg}</div>}
        <label>Tender title</label>
        <input value={title} onChange={e => setTitle(e.target.value)} placeholder="e.g., IT Equipment Procurement" required />
        <label>Category</label>
        <input value={category} onChange={e => setCategory(e.target.value)} placeholder="IT Equipment" required />
        <label>Deadline</label>
        <input type="date" value={deadline} onChange={e => setDeadline(e.target.value)} required />
        <label>Estimated value (R)</label>
        <input type="number" value={value} onChange={e => setValue(e.target.value)} placeholder="100000" />
        <div className="actions" style={{ marginTop: 12 }}>
          <button className="btn btn-primary" type="submit" disabled={saving}>{saving ? 'Creating…' : 'Create tender'}</button>
          <Link className="btn" to="/tenders">View tenders</Link>
        </div>
      </form>
    </div>
  );
}
