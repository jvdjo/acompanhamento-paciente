---
trigger: glob
---

# 🏛️ Manifesto de Engenharia de Software & Diretrizes Arquitetônicas

> **Propósito:** Este documento define a "Constituição Técnica" do projeto. Ele prioriza a longevidade do software, a manutenibilidade e a clareza sobre a conveniência. Todas as decisões de código devem ser validadas contra estes princípios, independentemente da linguagem ou framework utilizado.

---

## 1. Princípios Fundamentais (The Core)

### 1.1. Arquitetura Limpa (Clean Architecture)
O software deve ser estruturado em camadas concêntricas de dependência.
* **Regra de Ouro:** A dependência flui apenas de fora para dentro. O "Domínio" (Core Business) não deve saber nada sobre o "Banco de Dados" ou a "Web/UI".
* **Isolamento:** Frameworks, UI e Bancos de Dados são detalhes de implementação (Plugins). A lógica de negócio deve funcionar sem eles (testável unitariamente).

### 1.2. SOLID
A aplicação rigorosa dos princípios SOLID é obrigatória:
* **(S) Single Responsibility:** Uma classe/módulo deve ter apenas uma razão para mudar.
* **(O) Open/Closed:** Aberto para extensão, fechado para modificação.
* **(L) Liskov Substitution:** Subtipos devem ser substituíveis por seus tipos base.
* **(I) Interface Segregation:** Muitas interfaces específicas são melhores que uma geral.
* **(D) Dependency Inversion:** Dependa de abstrações, não de implementações concretas.

### 1.3. KISS & YAGNI
* **KISS (Keep It Simple, Stupid):** A complexidade é custo. A solução mais simples que resolve o problema é a correta.
* **YAGNI (You Aren't Gonna Need It):** Não implemente funcionalidades baseadas em "e se precisarmos no futuro". Implemente o que é necessário hoje.

---

## 2. Qualidade e Estilo de Código

### 2.1. Semântica e Nomenclatura
O código é lido muito mais vezes do que é escrito. Otimize para leitura humana.
* **Intenção:** O nome de uma variável ou função deve explicar *o que* ela faz e *por que* existe.
    * *Ruim:* `var d = 10; // dias`
    * *Bom:* `daysUntilExpiration = 10;`
* **Idioma:** O código deve ser escrito inteiramente em **Inglês** (variáveis, classes, comentários).
* **Funções:** Devem ser pequenas e fazer apenas uma coisa. Se o nome da função tem "And" (ex: `ValidateAndSave`), ela provavelmente deve ser dividida.

### 2.2. Tratamento de Erros e Exceções
* **Não use Exceptions para fluxo de controle:** Exceções devem ser para situações *excepcionais* (banco fora do ar, disco cheio). Para validações de negócio (ex: "email inválido"), utilize o **Notification Pattern** ou **Result Pattern**.
* **Falha Rápida (Fail Fast):** Valide as entradas no início do método (Guard Clauses). Se os dados forem inválidos, aborte imediatamente.

### 2.3. Comentários
* Evite comentários que explicam *o que* o código faz (o código deve ser autoexplicativo).
* Use comentários para explicar o *porquê* de uma decisão complexa ou não óbvia (Business Decisions).

---

## 3. Comunicação e API (Design de Contratos)

### 3.1. RESTful Maturity
Independentemente da tecnologia, a API deve ser previsível.
* **Recursos:** Use substantivos, não verbos (ex: `POST /users`, não `POST /createUser`).
* **Status Codes:** Utilize os códigos HTTP corretamente.
    * `200 OK` (Sucesso síncrono)
    * `201 Created` (Recurso criado)
    * `202 Accepted` (Processamento assíncrono iniciado)
    * `400 Bad Request` (Erro do cliente/validação)
    * `401/403` (Autenticação vs Autorização)
    * `500` (Erro interno não tratado)

### 3.2. Idempotência
Métodos de leitura (GET) e atualização/exclusão segura (PUT, DELETE) devem ser idempotentes. Repetir a mesma requisição múltiplas vezes deve ter o mesmo efeito que fazê-la uma única vez.

---

## 4. Persistência e Dados

### 4.1. Abstração de Dados
O código de negócio nunca deve chamar o banco de dados diretamente (SQL ou ORM). Utilize o padrão **Repository** para abstrair a coleção de dados e o padrão **Unit of Work** para transações atômicas.

### 4.2. Migrations e Versionamento
Toda alteração estrutural no banco de dados deve ser versionada via código (Database Migrations). Nunca altere o esquema manualmente em produção.

---

## 5. Segurança (Security by Design)

### 5.1. Princípio do Menor Privilégio
Serviços e usuários devem ter apenas as permissões estritamente necessárias para realizar sua tarefa.

### 5.2. Tratamento de Dados Sensíveis
* Nunca commite segredos (API Keys, senhas) no repositório. Use variáveis de ambiente.
* Sanitização de entrada é obrigatória para evitar Injection (SQL, XSS). Nunca confie no input do usuário.

---

## 6. Observabilidade

### 6.1. Logs Estruturados
Logs não são texto plano (`printf`). Logs são dados. Utilize logs estruturados (JSON) contendo contexto (Correlation ID, User ID, Timestamp) para facilitar a rastreabilidade em sistemas distribuídos.

---

## 7. Estratégia de Testes

### 7.1. Pirâmide de Testes
* **Unitários (Base):** Rápidos, isolados, testam lógica de domínio. Mockam I/O. (Maior volume).
* **Integração (Meio):** Testam a comunicação entre componentes (ex: API -> Banco de Dados).
* **E2E (Topo):** Testam fluxos críticos do usuário final. (Menor volume, mais lentos).

### 7.2. Teste o Comportamento, não a Implementação
Não escreva testes que quebram se você renomear uma variável privada. Teste a saída pública baseada em uma entrada.

---

## 8. Versionamento de Código (Git Flow)

* **Commits Atômicos:** Cada commit deve resolver uma única tarefa pequena.
* **Conventional Commits:** Siga o padrão:
    * `feat:` Nova funcionalidade.
    * `fix:` Correção de bug.
    * `docs:` Alteração apenas em documentação.
    * `chore:` Configurações de build, dependências, etc.
    * `refactor:` Alteração de código que não muda funcionalidade nem corrige bugs.