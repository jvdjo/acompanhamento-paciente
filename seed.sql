INSERT INTO "Psicologos" ("Id", "Nome", "Email", "PasswordHash") 
VALUES (1, 'Dr. Admin', 'admin@clinica.com', '$2a$11$K5FxKqW0qKPe5jMB9sqxmu.XD6JT3.yk3EfJqrGUcZqnOBG9WFxVe')
ON CONFLICT ("Id") DO NOTHING;
