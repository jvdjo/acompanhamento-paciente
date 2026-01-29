import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import './Header.css';

export default function Header() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <header className="header glass">
            <div className="header-content">
                <div className="header-brand" onClick={() => navigate('/pacientes')}>
                    <div className="header-logo">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                            <path d="M12 2a10 10 0 1 0 10 10A10 10 0 0 0 12 2z" />
                            <path d="M12 6v6l4 2" />
                        </svg>
                    </div>
                    <span className="header-title">PsicoNotes</span>
                </div>

                <div className="header-user">
                    <span className="header-username">{user?.name}</span>
                    <button className="btn btn-secondary" onClick={handleLogout}>
                        Sair
                    </button>
                </div>
            </div>
        </header>
    );
}
