import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import PatientCard from '../components/PatientCard';
import { getPacientes, createPaciente, deletePaciente } from '../services/api';
import './PacientesPage.css';

export default function PacientesPage() {
    const navigate = useNavigate();
    const [pacientes, setPacientes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [showForm, setShowForm] = useState(false);
    const [search, setSearch] = useState('');
    const [formData, setFormData] = useState({
        nome: '',
        profissao: '',
        escolaridade: '',
        dataNascimento: '',
        genero: ''
    });

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const data = await getPacientes();
            setPacientes(data);
        } catch (err) {
            setError('Erro ao carregar pacientes');
        } finally {
            setLoading(false);
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleCreate = async (e) => {
        e.preventDefault();
        if (!formData.nome.trim()) return;

        try {
            const paciente = await createPaciente({
                ...formData,
                nome: formData.nome.trim(),
                dataNascimento: new Date(formData.dataNascimento).toISOString()
            });
            setPacientes([...pacientes, paciente]);
            setFormData({
                nome: '',
                profissao: '',
                escolaridade: '',
                dataNascimento: '',
                genero: ''
            });
            setShowForm(false);
        } catch (err) {
            setError('Erro ao criar paciente');
        }
    };

    const handleDelete = async (id, e) => {
        e.stopPropagation();
        if (!confirm('Deseja excluir este paciente?')) return;
        try {
            await deletePaciente(id);
            setPacientes(pacientes.filter(p => p.id !== id));
        } catch (err) {
            setError('Erro ao excluir paciente');
        }
    };

    const filteredPacientes = pacientes.filter(paciente =>
        paciente.nome.toLowerCase().includes(search.toLowerCase())
    );

    return (
        <>
            <Header />
            <main className="page">
                <div className="header-actions">
                    <h1>Meus Pacientes</h1>
                    <button className="btn btn-primary" onClick={() => setShowForm(!showForm)}>
                        {showForm ? 'Cancelar' : '+ Novo Paciente'}
                    </button>
                </div>

                {showForm && (
                    <form onSubmit={handleCreate} className="new-patient-form card fade-in">
                        <div className="form-grid">
                            <div className="input-group">
                                <label>Nome Completo</label>
                                <input
                                    type="text"
                                    name="nome"
                                    className="input"
                                    placeholder="Nome do paciente"
                                    value={formData.nome}
                                    onChange={handleInputChange}
                                    required
                                    autoFocus
                                />
                            </div>

                            <div className="input-group">
                                <label>Data de Nascimento</label>
                                <input
                                    type="date"
                                    name="dataNascimento"
                                    className="input"
                                    value={formData.dataNascimento}
                                    onChange={handleInputChange}
                                    required
                                />
                            </div>

                            <div className="input-group">
                                <label>Gênero</label>
                                <select
                                    name="genero"
                                    className="input"
                                    value={formData.genero}
                                    onChange={handleInputChange}
                                    required
                                >
                                    <option value="">Selecione...</option>
                                    <option value="Masculino">Masculino</option>
                                    <option value="Feminino">Feminino</option>
                                    <option value="Outro">Outro</option>
                                </select>
                            </div>

                            <div className="input-group">
                                <label>Profissão</label>
                                <input
                                    type="text"
                                    name="profissao"
                                    className="input"
                                    placeholder="Ex: Engenheiro"
                                    value={formData.profissao}
                                    onChange={handleInputChange}
                                />
                            </div>

                            <div className="input-group">
                                <label>Escolaridade</label>
                                <select
                                    name="escolaridade"
                                    className="input"
                                    value={formData.escolaridade}
                                    onChange={handleInputChange}
                                >
                                    <option value="">Selecione...</option>
                                    <option value="Fundamental Incompleto">Fundamental Incompleto</option>
                                    <option value="Fundamental Completo">Fundamental Completo</option>
                                    <option value="Médio Incompleto">Médio Incompleto</option>
                                    <option value="Médio Completo">Médio Completo</option>
                                    <option value="Superior Incompleto">Superior Incompleto</option>
                                    <option value="Superior Completo">Superior Completo</option>
                                    <option value="Pós-graduação">Pós-graduação</option>
                                </select>
                            </div>
                        </div>

                        <div className="form-actions">
                            <button type="button" className="btn btn-secondary" onClick={() => setShowForm(false)}>
                                Cancelar
                            </button>
                            <button type="submit" className="btn btn-primary">
                                Cadastrar Paciente
                            </button>
                        </div>
                    </form>
                )}

                <div className="search-box">
                    <input
                        type="text"
                        className="input"
                        placeholder="Buscar paciente..."
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                    />
                </div>

                {error && <div className="error-message">{error}</div>}

                {loading ? (
                    <div className="loading-container">
                        <div className="loader"></div>
                    </div>
                ) : filteredPacientes.length === 0 ? (
                    <div className="empty-state">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5">
                            <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
                            <circle cx="9" cy="7" r="4" />
                            <path d="M23 21v-2a4 4 0 0 0-3-3.87" />
                            <path d="M16 3.13a4 4 0 0 1 0 7.75" />
                        </svg>
                        <p>Nenhum paciente encontrado</p>
                        <p className="text-muted">Clique em "+ Novo Paciente" para adicionar</p>
                    </div>
                ) : (
                    <div className="patients-grid">
                        {filteredPacientes.map(paciente => (
                            <PatientCard
                                key={paciente.id}
                                patient={paciente}
                                onClick={(id) => navigate(`/pacientes/${id}`)}
                                onDelete={handleDelete}
                            />
                        ))}
                    </div>
                )}
            </main>
        </>
    );
}
