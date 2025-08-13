import React from 'react';
import { Link, useNavigate } from 'react-router-dom';

const Stat = ({ value, label }: { value: string; label: string }) => (
  <div className="stat">
    <div className="stat-value">{value}</div>
    <div className="stat-label">{label}</div>
  </div>
);

const Card = ({ title, children, to }: { title: string; children: React.ReactNode; to?: string }) => {
  const content = (
    <div className="card" style={{ cursor: to ? 'pointer' : 'default' }}>
      <h3>{title}</h3>
      <div>{children}</div>
    </div>
  );
  return to ? (
    <Link to={to} style={{ textDecoration: 'none', color: 'inherit' }}>{content}</Link>
  ) : content;
};

export default function Home() {
  const [tenders, setTenders] = React.useState<any[]>([]);
  const [q, setQ] = React.useState('');
  const [cat, setCat] = React.useState('');
  const [showLoginMenu, setShowLoginMenu] = React.useState(false);
  const sampleTenders = [
    {
      title: 'Office Supplies for Government Buildings',
      number: 'TND-2025-001',
      status: 'Open',
      estimatedValue: 'R 250,000',
      description:
        'Supply of office furniture, stationery, and equipment for various government buildings in Johannesburg.',
      closingDate: '2025-02-15',
      buyer: 'Johannesburg Municipality'
    },
    {
      title: 'IT Equipment Procurement',
      number: 'TND-2025-002',
      status: 'Open',
      estimatedValue: 'R 1,500,000',
      description:
        'Procurement of computers, servers, and networking equipment for modernization project.',
      closingDate: '2025-02-28',
      buyer: 'Department of Technology'
    },
    {
      title: 'Marketing Services Campaign',
      number: 'TND-2025-003',
      status: 'Closed',
      estimatedValue: 'R 800,000',
      description:
        'Strategic marketing and communication services for public awareness campaign.',
      closingDate: '2025-01-30',
      buyer: 'Communications Department'
    }
  ];

  React.useEffect(() => {
    fetch('/api/tenders')
      .then(r => (r.ok ? r.json() : Promise.reject()))
      .then(json => {
        if (Array.isArray(json?.data)) setTenders(json.data);
        else if (Array.isArray(json)) setTenders(json);
      })
      .catch(() => setTenders(sampleTenders));
  }, []);

  const list = tenders.length ? tenders.map((t: any) => ({
    title: t.title ?? t.name ?? 'Tender',
    number: t.referenceNumber ?? t.number ?? t.tenderNumber ?? 'TND-XXXX',
    status: typeof t.status === 'string' ? t.status : 'Open',
    estimatedValue: t.estimatedValue ? `R ${t.estimatedValue}` : '—',
    description: t.description ?? '',
    closingDate: t.closingDate ?? t.deadline ?? '',
    buyer: t.buyerName ?? t.location ?? t.buyer ?? '—'
  })) : sampleTenders;

  const navigate = useNavigate();
  return (
    <div className="home">
      {/* Header */}
      <header className="header">
        <div className="container">
          <div className="brand">
            <img src="/assets/company-logo.png" className="logo" />
            <h1>Procurement Platform</h1>
          </div>
          <nav className="nav" style={{ position: 'relative' }}>
            <button className="btn btn-primary" onClick={() => setShowLoginMenu(v => !v)}>Login</button>
            {showLoginMenu && (
              <div style={{ position: 'absolute', right: 0, top: '100%', marginTop: 8, background: '#fff', boxShadow: '0 6px 18px rgba(0,0,0,0.12)', borderRadius: 8, minWidth: 200, zIndex: 10 }}>
                <button onClick={() => { setShowLoginMenu(false); navigate('/login?role=supplier'); }} style={{ display: 'block', width: '100%', textAlign: 'left', padding: '10px 12px', background: 'transparent', border: 'none', cursor: 'pointer' }}>Login as Supplier</button>
                <button onClick={() => { setShowLoginMenu(false); navigate('/login?role=buyer'); }} style={{ display: 'block', width: '100%', textAlign: 'left', padding: '10px 12px', background: 'transparent', border: 'none', cursor: 'pointer' }}>Login as Buyer</button>
              </div>
            )}
          </nav>
        </div>
      </header>

      {/* Hero + Search */}
      <section className="hero">
        <div className="container hero-inner">
          <div className="hero-content">
            <h2>Find, Bid, and Manage Tenders in One Place</h2>
            <p>Transparent, compliant procurement for public and private organizations in South Africa.</p>
            <div className="search">
              <input placeholder="Search by keyword..." value={q} onChange={(e) => setQ(e.target.value)} />
              <select value={cat} onChange={(e) => setCat(e.target.value)}>
                <option value="">All Categories</option>
                <option>Office Supplies</option>
                <option>IT Equipment</option>
                <option>Construction</option>
                <option>Consulting</option>
              </select>
              <button className="btn btn-primary" onClick={() => navigate(`/tenders${q || cat ? `?q=${encodeURIComponent(q)}&cat=${encodeURIComponent(cat)}` : ''}`)}>Search</button>
            </div>
            <div className="popular">Popular: laptops, cleaning, security, catering</div>
          </div>
          <div className="stats">
            <Stat value="89" label="Active Tenders" />
            <Stat value="2,156" label="Total Bids" />
            <Stat value="156" label="Active Suppliers" />
            <Stat value="R47M" label="Contracts Value" />
          </div>
        </div>
      </section>

      {/* Who it's for */}
    <section className="who">
        <div className="container cards">
      <Card title="Public Viewer" to="/tenders">Browse open tenders and awarded contracts.</Card>
      <Card title="Supplier" to="/supplier">Discover opportunities and submit bids securely.</Card>
      <Card title="Buyer" to="/buyer">Publish, evaluate, and award tenders efficiently.</Card>
      <Card title="Platform Manager" to="/manager">Audit, configure, and ensure compliance.</Card>
        </div>
      </section>

      {/* Featured tenders */}
      <section className="featured">
        <div className="container">
          <div className="split">
            <h3>Featured tenders</h3>
            <Link to="/tenders" className="link">View all</Link>
          </div>
          <div className="grid">
            {list.map(t => (
              <div key={t.number} className="card">
                <div className="split">
                  <h4>{t.title}</h4>
                  <span className={`chip ${t.status.toLowerCase()}`}>{t.status}</span>
                </div>
                <div className="subtle">{t.number} • {t.buyer}</div>
                <p>{t.description}</p>
                <div className="split subtle">
                  <span>Closes: {t.closingDate}</span>
                  <strong className="value">{t.estimatedValue}</strong>
                </div>
                <div className="actions">
                  <button className="btn" onClick={() => navigate(`/tenders/${t.number}`)}>Details</button>
                  {t.status === 'Open' && <Link className="btn btn-primary" to="/login?role=supplier">Login to bid</Link>}
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* How it works */}
      <section className="how">
        <div className="container grid-4">
          <Card title="1) Publish">Buyers create and publish tenders with documents.</Card>
          <Card title="2) Bid">Suppliers submit bids with pricing and attachments.</Card>
          <Card title="3) Evaluate">Evaluation panel scores and shortlists bids.</Card>
          <Card title="4) Award">Managers approve and award contracts.</Card>
        </div>
      </section>

      {/* Compliance */}
      <section className="compliance">
        <div className="container split">
          <div>
            <h3>Compliance & Security</h3>
            <ul>
              <li>Aligned with South Africa's Public Procurement Act (2024)</li>
              <li>Audit logs, role-based access, and document versioning</li>
              <li>Secure file storage and data protection best practices</li>
            </ul>
          </div>
          <div className="card">
            <div className="muted">Need help?</div>
            <p>Browse FAQs or contact support for onboarding.</p>
            <div className="actions">
              <Link className="btn btn-primary" to="/tenders">Browse tenders</Link>
              <Link className="btn" to="/register">Register</Link>
            </div>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="footer">
        <div className="container center">
          <div>© 2025 Procurement Platform. All rights reserved.</div>
          <div className="muted">Compliant with South Africa's Public Procurement Act (2024)</div>
        </div>
      </footer>
    </div>
  );
}
