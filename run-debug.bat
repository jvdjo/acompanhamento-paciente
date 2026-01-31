@echo off
echo ========================================
echo    PSICONOTES - MODO DEBUG
echo ========================================
echo.

echo [1/3] Iniciando banco de dados PostgreSQL...
cd /d "%~dp0"
docker-compose up -d
timeout /t 3 /nobreak > nul

echo.
echo [2/3] Iniciando Backend (Development)...
start "Backend - Development" cmd /k "cd backend\AcompanhamentoPaciente.Api && dotnet run --launch-profile Development"
timeout /t 5 /nobreak > nul

echo.
echo [3/3] Iniciando Frontend (Development)...
start "Frontend - Development" cmd /k "cd frontend && npm run dev"

echo.
echo ========================================
echo    Aplicacao iniciada em modo DEBUG
echo ========================================
echo.
echo Backend:  http://localhost:5008
echo Frontend: http://localhost:5173
echo.
echo Pressione qualquer tecla para fechar este terminal...
pause > nul
