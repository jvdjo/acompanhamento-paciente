import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import { getPaciente, getSessoes, createSessao, deleteSessao } from '../services/api';
import './PacientePage.css';

export default function PacientePage() {
    const { id } = useParams();
    const navigate = useNavigate();

    const [paciente, setPaciente] = useState(null);
    const [sessoes, setSessoes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [selectedDate, setSelectedDate] = useState(new Date().toISOString().split('T')[0]);

    useEffect(() => {
        loadData();
    }, [id]);

    const loadData = async () => {
        try {
            const [pacienteData, sessoesData] = await Promise.all([
                getPaciente(id),
                getSessoes(id)
            ]);
            setPaciente(pacienteData);
            setSessoes(sessoesData);
        } catch (err) {
            setError('Erro ao carregar dados do paciente');
        } finally {
            setLoading(false);
        }
    };

    const handleCreateSessao = async () => {
        try {
            const sessao = await createSessao(id, new Date(selectedDate).toISOString());
            navigate(`/pacientes/${id}/sessoes/${sessao.id}`);
        } catch (err) {
            setError('Erro ao criar sessão');
        }
    };

    const handleDeleteSessao = async (sessaoId, e) => {
        e.stopPropagation();
        if (!confirm('Deseja excluir esta sessão?')) return;

        try {
            await deleteSessao(id, sessaoId);
            setSessoes(sessoes.filter(s => s.id !== sessaoId));
        } catch (err) {
            setError('Erro ao excluir sessão');
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

    if (!paciente) {
        return (
            <>
                <Header />
                <main className="page">
                    <div className="error-message">Paciente não encontrado</div>
                </main>
            </>
        );
    }

    return (
        <>
            <Header />
            <main className="page">
                <button className="back-button" onClick={() => navigate('/pacientes')}>
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                        <path d="M19 12H5M12 19l-7-7 7-7" />
                    </svg>
                    Voltar
                </button>

                <div className="patient-header card">
                    <div className="patient-avatar-large">
                        {paciente.nome.charAt(0).toUpperCase()}
                    </div>
                    <div className="patient-info">
                        <h1 className="patient-name-large">{paciente.nome}</h1>
                        <p className="text-muted">
                            Cadastrado em {formatDate(paciente.dataCadastro)}
                        </p>

                        <div className="patient-details-grid">
                            <div className="detail-item">
                                <span className="detail-label">Idade</span>
                                <span className="detail-value">{paciente.idade} anos</span>
                            </div>
                            <div className="detail-item">
                                <span className="detail-label">Gênero</span>
                                <span className="detail-value">{paciente.genero || 'Não informado'}</span>
                            </div>
                            <div className="detail-item">
                                <span className="detail-label">Profissão</span>
                                <span className="detail-value">{paciente.profissao || 'Não informada'}</span>
                            </div>
                            <div className="detail-item">
                                <span className="detail-label">Escolaridade</span>
                                <span className="detail-value">{paciente.escolaridade || 'Não informada'}</span>
                            </div>
                        </div>
                    </div>
                </div>

                <section className="sessions-section">
                    <div className="section-header">
                        <h2>Sessões</h2>
                        <div className="new-session-form">
                            <input
                                type="date"
                                className="input date-input"
                                value={selectedDate}
                                onChange={(e) => setSelectedDate(e.target.value)}
                            />
                            <button className="btn btn-primary" onClick={handleCreateSessao}>
                                + Nova Sessão
                            </button>
                        </div>
                    </div>

                    {error && <div className="error-message">{error}</div>}

                    {sessoes.length === 0 ? (
                        <div className="empty-sessions">
                            <p className="text-muted">Nenhuma sessão registrada</p>
                        </div>
                    ) : (
                        <div className="sessions-list">
                            {sessoes.map(sessao => (
                                <div
                                    key={sessao.id}
                                    className="session-card card"
                                    onClick={() => navigate(`/pacientes/${id}/sessoes/${sessao.id}`)}
                                >
                                    <div className="session-info">
                                        <div className="session-date-badge">
                                            {new Date(sessao.data).getDate()}
                                        </div>
                                        <div>
                                            <p className="session-date">{formatDate(sessao.data)}</p>
                                            <p className="session-status text-muted">
                                                {sessao.anotacoes ? 'Com anotações' : 'Sem anotações'}
                                            </p>
                                        </div>
                                    </div>
                                    <button
                                        className="btn btn-icon session-delete"
                                        onClick={(e) => handleDeleteSessao(sessao.id, e)}
                                    >
                                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                                            <path d="M3 6h18M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2" />
                                        </svg>
                                    </button>
                                </div>
                            ))}
                        </div>
                    )}
                </section>
            </main>
        </>
    );
}
