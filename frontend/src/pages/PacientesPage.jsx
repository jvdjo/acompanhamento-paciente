import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import PatientCard from '../components/PatientCard';
import { getPacientes, createPaciente, deletePaciente } from '../services/api';
import './PacientesPage.css';

export default function PacientesPage() {
    const [pacientes, setPacientes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [showForm, setShowForm] = useState(false);
    const [newName, setNewName] = useState('');
    const [search, setSearch] = useState('');

    const navigate = useNavigate();

    useEffect(() => {
        loadPacientes();
    }, []);

    const loadPacientes = async () => {
        try {
            const data = await getPacientes();
            setPacientes(data);
        } catch (err) {
            setError('Erro ao carregar pacientes');
        } finally {
            setLoading(false);
        }
    };

    const handleCreate = async (e) => {
        e.preventDefault();
        if (!newName.trim()) return;

        try {
            const paciente = await createPaciente(newName.trim());
            setPacientes([...pacientes, paciente]);
            setNewName('');
            setShowForm(false);
        } catch (err) {
            setError('Erro ao criar paciente');
        }
    };

    const handleDelete = async (id) => {
        try {
            await deletePaciente(id);
            setPacientes(pacientes.filter(p => p.id !== id));
        } catch (err) {
            setError('Erro ao excluir paciente');
        }
    };

    const filteredPacientes = pacientes.filter(p =>
        p.nome.toLowerCase().includes(search.toLowerCase())
    );

    return (
        <>
            <Header />
            <main className="page">
                <div className="page-header">
                    <h1 className="page-title">Meus Pacientes</h1>
                    <button className="btn btn-primary" onClick={() => setShowForm(!showForm)}>
                        {showForm ? 'Cancelar' : '+ Novo Paciente'}
                    </button>
                </div>

                {showForm && (
                    <form onSubmit={handleCreate} className="new-patient-form card fade-in">
                        <div className="input-group">
                            <input
                                type="text"
                                className="input"
                                placeholder="Nome do paciente"
                                value={newName}
                                onChange={(e) => setNewName(e.target.value)}
                                autoFocus
                            />
                        </div>
                        <button type="submit" className="btn btn-primary">
                            Cadastrar
                        </button>
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
