import React from 'react';
import { Link, useLocation } from 'react-router-dom';

export default function Tenders() {
  const [tenders, setTenders] = React.useState<any[]>([]);
  const location = useLocation();
  const params = React.useMemo(() => new URLSearchParams(location.search), [location.search]);
  const q = (params.get('q') || '').toLowerCase();
  const cat = (params.get('cat') || '').toLowerCase();
  React.useEffect(() => {
    fetch('/api/tenders')
      .then(r => (r.ok ? r.json() : Promise.reject()))
      .then(json => Array.isArray(json?.data) ? setTenders(json.data) : Array.isArray(json) ? setTenders(json) : undefined)
      .catch(() => setTenders([
        { title: 'Office Supplies', referenceNumber: 'TND-2025-001', status: 'Open' },
        { title: 'IT Refresh', referenceNumber: 'TND-2025-002', status: 'Open' },
      ]));
  }, []);

  return (
    <div className="container">
      <h2>All Tenders</h2>
      <ul>
        {(tenders.filter((t: any) => {
          const title = (t.title || '').toLowerCase();
          const category = (t.category || '').toLowerCase();
          const okQ = q ? title.includes(q) : true;
          const okCat = cat ? (category.includes(cat) || title.includes(cat)) : true;
          return okQ && okCat;
        })).map((t: any) => (
          <li key={t.referenceNumber || t.number}>
            <Link to={`/tenders/${t.referenceNumber || t.number}`}>{t.title || 'Tender'}</Link>
            <span style={{ marginLeft: 8, color: '#666' }}>• {t.status || 'Open'}</span>
          </li>
        ))}
      </ul>
    </div>
  );
}
