import { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import './LoginPage.css';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5008/api';

export default function LoginPage() {
    const [error, setError] = useState('');
    const [searchParams] = useSearchParams();

    useEffect(() => {
        // Verifica se há erro de autenticação Google na URL
        const errorParam = searchParams.get('error');
        if (errorParam) {
            const errorMessages = {
                'google_auth_failed': 'Falha na autenticação com Google',
                'invalid_google_response': 'Resposta inválida do Google',
                'callback_failed': 'Erro ao processar autenticação'
            };
            setError(errorMessages[errorParam] || 'Erro de autenticação');
        }
    }, [searchParams]);

    const handleGoogleLogin = () => {
        // Redireciona para o endpoint de autenticação Google do backend
        window.location.href = `${API_URL}/auth/google`;
    };

    return (
        <div className="login-page">
            <div className="login-container">
                <div className="login-header">
                    <div className="login-logo">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                            <path d="M12 2a10 10 0 1 0 10 10A10 10 0 0 0 12 2z" />
                            <path d="M12 6v6l4 2" />
                        </svg>
                    </div>
                    <h1 className="login-title">PsicoNotes</h1>
                    <p className="login-subtitle">Acompanhamento de Pacientes</p>
                </div>

                {error && <div className="login-error">{error}</div>}

                <div className="login-form card">
                    {/* Botão Google OAuth */}
                    <button
                        type="button"
                        className="btn btn-google"
                        onClick={handleGoogleLogin}
                    >
                        <svg className="google-icon" viewBox="0 0 24 24">
                            <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z" />
                            <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z" />
                            <path fill="#FBBC05" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z" />
                            <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z" />
                        </svg>
                        Continuar com Google
                    </button>
                    <p className="login-hint text-muted" style={{ marginTop: '1rem', textAlign: 'center' }}>
                        Acesso exclusivo via conta Google autorizada.
                    </p>
                </div>
            </div>
        </div>
    );
}


