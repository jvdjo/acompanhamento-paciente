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
    const [notasTexto, setNotasTexto] = useState('');
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');
    const [savedMessage, setSavedMessage] = useState('');
    const [activeTab, setActiveTab] = useState('notas');

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
            setNotasTexto(sessaoData.notasTexto || '');
        } catch (err) {
            setError('Erro ao carregar sessão');
        } finally {
            setLoading(false);
        }
    };

    const handleSaveDrawing = async (canvasData) => {
        setSaving(true);
        setSavedMessage('');

        try {
            await updateSessao(pacienteId, sessaoId, canvasData, notasTexto);
            setSavedMessage('Anotações salvas com sucesso!');
            setTimeout(() => setSavedMessage(''), 3000);
        } catch (err) {
            setError('Erro ao salvar anotações');
        } finally {
            setSaving(false);
        }
    };

    const handleSaveNotes = async () => {
        setSaving(true);
        setSavedMessage('');

        try {
            await updateSessao(pacienteId, sessaoId, sessao?.anotacoes, notasTexto);
            setSavedMessage('Notas salvas com sucesso!');
            setTimeout(() => setSavedMessage(''), 3000);
        } catch (err) {
            setError('Erro ao salvar notas');
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

                <div className="sessao-tabs">
                    <button
                        className={`tab-button ${activeTab === 'notas' ? 'active' : ''}`}
                        onClick={() => setActiveTab('notas')}
                    >
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                            <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" />
                            <path d="M14 2v6h6" />
                            <path d="M16 13H8M16 17H8M10 9H8" />
                        </svg>
                        Notas de Texto
                    </button>
                    <button
                        className={`tab-button ${activeTab === 'desenho' ? 'active' : ''}`}
                        onClick={() => setActiveTab('desenho')}
                    >
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                            <path d="M12 19l7-7 3 3-7 7-3-3z" />
                            <path d="M18 13l-1.5-7.5L2 2l3.5 14.5L13 18l5-5z" />
                            <path d="M2 2l7.586 7.586" />
                            <circle cx="11" cy="11" r="2" />
                        </svg>
                        Desenho
                    </button>
                </div>

                <div className="sessao-content">
                    {activeTab === 'notas' && (
                        <div className="notes-section">
                            <textarea
                                className="notes-textarea"
                                placeholder="Digite suas notas sobre a sessão aqui..."
                                value={notasTexto}
                                onChange={(e) => setNotasTexto(e.target.value)}
                            />
                            <div className="notes-actions">
                                <span className="char-count text-muted">
                                    {notasTexto.length} caracteres
                                </span>
                                <button
                                    className="btn btn-primary"
                                    onClick={handleSaveNotes}
                                    disabled={saving}
                                >
                                    {saving ? 'Salvando...' : 'Salvar Notas'}
                                </button>
                            </div>
                        </div>
                    )}

                    {activeTab === 'desenho' && (
                        <div className="canvas-container">
                            <DrawingCanvas
                                initialData={sessao?.anotacoes}
                                onSave={handleSaveDrawing}
                            />
                        </div>
                    )}
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
