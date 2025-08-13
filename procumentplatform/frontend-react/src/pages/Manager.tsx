import React from 'react';

export default function Manager() {
  const [orgName, setOrgName] = React.useState('');
  const [supportEmail, setSupportEmail] = React.useState('');
  const [scoring, setScoring] = React.useState(70);
  const [msg, setMsg] = React.useState<string | null>(null);

  async function onSave(e: React.FormEvent) {
    e.preventDefault();
    setMsg(null);
    // Placeholder save
    await new Promise(r => setTimeout(r, 500));
    setMsg('Settings saved (demo).');
  }

  return (
    <div className="container">
      <h2>Platform Manager</h2>
      <p>Audit, configure, and ensure compliance.</p>
      <form className="card" style={{ maxWidth: 640 }} onSubmit={onSave}>
        {msg && <div className="muted" style={{ marginBottom: 12 }}>{msg}</div>}
        <label>Organization name</label>
        <input value={orgName} onChange={e => setOrgName(e.target.value)} placeholder="Org name" />
        <label>Support email</label>
        <input type="email" value={supportEmail} onChange={e => setSupportEmail(e.target.value)} placeholder="support@example.com" />
        <label>Minimum passing score (%)</label>
        <input type="number" min={0} max={100} value={scoring} onChange={e => setScoring(parseInt(e.target.value || '0'))} />
        <button className="btn btn-primary" type="submit" style={{ marginTop: 12 }}>Save settings</button>
      </form>
    </div>
  );
}
