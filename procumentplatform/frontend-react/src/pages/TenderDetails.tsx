import React from 'react';
import { useParams, Link } from 'react-router-dom';

export default function TenderDetails() {
  const { ref } = useParams();
  const [tender, setTender] = React.useState<any | null>(null);

  React.useEffect(() => {
    if (!ref) return;
    fetch(`/api/tenders/${ref}`)
      .then(r => (r.ok ? r.json() : Promise.reject()))
      .then(setTender)
      .catch(() => setTender({
        title: 'Tender Details',
        referenceNumber: ref,
        status: 'Open',
        description: 'Details are not available from API right now; this is a placeholder.',
      }));
  }, [ref]);

  return (
    <div className="container">
      <Link to="/tenders">← Back to tenders</Link>
      <h2>{tender?.title}</h2>
      <div className="muted">Ref: {tender?.referenceNumber || ref}</div>
      <p>{tender?.description}</p>
      <div className="actions">
        <Link className="btn btn-primary" to="/login">Login to bid</Link>
      </div>
    </div>
  );
}
