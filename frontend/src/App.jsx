import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import LoginPage from './pages/LoginPage';
import PacientesPage from './pages/PacientesPage';
import PacientePage from './pages/PacientePage';
import SessaoPage from './pages/SessaoPage';
import './index.css';

function ProtectedRoute({ children }) {
  const { isAuthenticated, loading } = useAuth();

  if (loading) {
    return (
      <div className="page flex items-center justify-center">
        <div className="loader"></div>
      </div>
    );
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return children;
}

function PublicRoute({ children }) {
  const { isAuthenticated, loading } = useAuth();

  if (loading) {
    return (
      <div className="page flex items-center justify-center">
        <div className="loader"></div>
      </div>
    );
  }

  if (isAuthenticated) {
    return <Navigate to="/pacientes" replace />;
  }

  return children;
}

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route
            path="/login"
            element={
              <PublicRoute>
                <LoginPage />
              </PublicRoute>
            }
          />
          <Route
            path="/pacientes"
            element={
              <ProtectedRoute>
                <PacientesPage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/pacientes/:id"
            element={
              <ProtectedRoute>
                <PacientePage />
              </ProtectedRoute>
            }
          />
          <Route
            path="/pacientes/:pacienteId/sessoes/:sessaoId"
            element={
              <ProtectedRoute>
                <SessaoPage />
              </ProtectedRoute>
            }
          />
          <Route path="/" element={<Navigate to="/pacientes" replace />} />
          <Route path="*" element={<Navigate to="/pacientes" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
