import './PatientCard.css';

export default function PatientCard({ patient, onClick, onDelete }) {
    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString('pt-BR', {
            day: '2-digit',
            month: 'short',
            year: 'numeric'
        });
    };

    const handleDelete = (e) => {
        e.stopPropagation();
        if (confirm(`Deseja realmente excluir o paciente ${patient.nome}?`)) {
            onDelete(patient.id);
        }
    };

    return (
        <div className="patient-card card" onClick={() => onClick(patient.id)}>
            <div className="patient-card-avatar">
                {patient.nome.charAt(0).toUpperCase()}
            </div>
            <div className="patient-card-info">
                <h3 className="patient-card-name">{patient.nome}</h3>
                <span className="patient-card-date">
                    Cadastrado em {formatDate(patient.dataCadastro)}
                </span>
            </div>
            <button
                className="patient-card-delete btn btn-icon"
                onClick={handleDelete}
                aria-label="Excluir paciente"
            >
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <path d="M3 6h18M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2" />
                </svg>
            </button>
        </div>
    );
}
