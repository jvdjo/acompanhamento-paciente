@echo off
echo ========================================
echo    PSICONOTES - MODO RELEASE
echo ========================================
echo.

echo [1/4] Iniciando banco de dados PostgreSQL...
cd /d "%~dp0"
docker-compose up -d
timeout /t 3 /nobreak > nul

echo.
echo [2/4] Compilando Backend (Release)...
cd backend
dotnet build -c Release
if %ERRORLEVEL% neq 0 (
    echo ERRO: Falha ao compilar o backend
    pause
    exit /b 1
)

echo.
echo [3/4] Compilando Frontend (Production)...
cd ..\frontend
call npm run build
if %ERRORLEVEL% neq 0 (
    echo ERRO: Falha ao compilar o frontend
    pause
    exit /b 1
)

echo.
echo [4/4] Iniciando aplicacao...
cd ..
start "Backend - Production" cmd /k "cd backend\AcompanhamentoPaciente.Api && dotnet run --launch-profile Production -c Release"
timeout /t 5 /nobreak > nul
start "Frontend - Preview" cmd /k "cd frontend && npm run preview"

echo.
echo ========================================
echo    Aplicacao iniciada em modo RELEASE
echo ========================================
echo.
echo Backend:  http://localhost:5008
echo Frontend: http://localhost:4173
echo.
echo Pressione qualquer tecla para fechar este terminal...
pause > nul
