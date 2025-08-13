import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Tenders from './pages/Tenders';
import TenderDetails from './pages/TenderDetails';
import Login from './pages/Login';
import Register from './pages/Register';
import Supplier from './pages/Supplier';
import Buyer from './pages/Buyer';
import Manager from './pages/Manager';

export default function App() {
  return (
    <div className="app">
      <Routes>
        <Route path="/" element={<Home />} />
  <Route path="/tenders" element={<Tenders />} />
  <Route path="/tenders/:ref" element={<TenderDetails />} />
  <Route path="/login" element={<Login />} />
  <Route path="/register" element={<Register />} />
  <Route path="/supplier" element={<Supplier />} />
  <Route path="/buyer" element={<Buyer />} />
  <Route path="/manager" element={<Manager />} />
        <Route path="*" element={<Home />} />
      </Routes>
    </div>
  );
}
