import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import DrawingCanvas from '../components/DrawingCanvas';
import { getSessao, updateSessao, getPaciente } from '../services/api';
import './SessaoPage.css';

export default function SessaoPage() {
    const { pacienteId, sessaoId } = useParams();
    const navigate = useNavigate();

    const [sessao, setSessao] = useState(null);
    const [paciente, setPaciente] = useState(null);
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');
    const [savedMessage, setSavedMessage] = useState('');

    useEffect(() => {
        loadData();
    }, [pacienteId, sessaoId]);

    const loadData = async () => {
        try {
            const [sessaoData, pacienteData] = await Promise.all([
                getSessao(pacienteId, sessaoId),
                getPaciente(pacienteId)
            ]);
            setSessao(sessaoData);
            setPaciente(pacienteData);
        } catch (err) {
            setError('Erro ao carregar sessão');
        } finally {
            setLoading(false);
        }
    };

    const handleSave = async (canvasData) => {
        setSaving(true);
        setSavedMessage('');

        try {
            await updateSessao(pacienteId, sessaoId, canvasData);
            setSavedMessage('Anotações salvas com sucesso!');
            setTimeout(() => setSavedMessage(''), 3000);
        } catch (err) {
            setError('Erro ao salvar anotações');
        } finally {
            setSaving(false);
        }
    };

    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString('pt-BR', {
            weekday: 'long',
            day: '2-digit',
            month: 'long',
            year: 'numeric'
        });
    };

    if (loading) {
        return (
            <>
                <Header />
                <main className="page">
                    <div className="loading-container">
                        <div className="loader"></div>
                    </div>
                </main>
            </>
        );
    }

    return (
        <>
            <Header />
            <main className="page sessao-page">
                <button className="back-button" onClick={() => navigate(`/pacientes/${pacienteId}`)}>
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                        <path d="M19 12H5M12 19l-7-7 7-7" />
                    </svg>
                    Voltar para {paciente?.nome}
                </button>

                <div className="sessao-header">
                    <h1 className="page-title">Sessão</h1>
                    <p className="sessao-date text-muted">
                        {sessao && formatDate(sessao.data)}
                    </p>
                </div>

                {error && <div className="error-message">{error}</div>}
                {savedMessage && <div className="success-message">{savedMessage}</div>}

                <div className="canvas-container">
                    <DrawingCanvas
                        initialData={sessao?.anotacoes}
                        onSave={handleSave}
                    />
                </div>

                {saving && (
                    <div className="saving-indicator">
                        <div className="loader" style={{ width: 20, height: 20 }}></div>
                        Salvando...
                    </div>
                )}
            </main>
        </>
    );
}
