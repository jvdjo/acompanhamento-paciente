import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { useTheme } from '../contexts/ThemeContext';
import './Header.css';

export default function Header() {
    const { user, logout } = useAuth();
    const { theme, toggleTheme } = useTheme();
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
                    <button
                        className="btn btn-icon"
                        onClick={toggleTheme}
                        title={theme === 'dark' ? 'Mudar para tema claro' : 'Mudar para tema escuro'}
                    >
                        {theme === 'dark' ? (
                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" width="20" height="20">
                                <circle cx="12" cy="12" r="5" />
                                <path d="M12 1v2M12 21v2M4.22 4.22l1.42 1.42M18.36 18.36l1.42 1.42M1 12h2M21 12h2M4.22 19.78l1.42-1.42M18.36 5.64l1.42-1.42" />
                            </svg>
                        ) : (
                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" width="20" height="20">
                                <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z" />
                            </svg>
                        )}
                    </button>
                    <span className="header-username">{user?.name}</span>
                    <button className="btn btn-secondary" onClick={handleLogout}>
                        Sair
                    </button>
                </div>
            </div>
        </header>
    );
}
