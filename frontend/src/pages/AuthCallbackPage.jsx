import { useEffect, useRef } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

export default function AuthCallbackPage() {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const { login } = useAuth();
    const processedRef = useRef(false);

    useEffect(() => {
        // Evita processamento duplo
        if (processedRef.current) return;

        const token = searchParams.get('token');
        const nome = searchParams.get('nome');
        const picture = searchParams.get('picture');

        if (token && nome) {
            processedRef.current = true;
            login(token, nome, picture);
            navigate('/pacientes', { replace: true });
        } else {
            processedRef.current = true;
            navigate('/login?error=callback_failed', { replace: true });
        }
    }, [searchParams, login, navigate]);

    return (
        <div style={{
            minHeight: '100vh',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            flexDirection: 'column',
            gap: '1rem'
        }}>
            <span className="loader" style={{ width: 48, height: 48 }}></span>
            <p style={{ color: 'var(--gray-400)' }}>Autenticando...</p>
        </div>
    );
}
