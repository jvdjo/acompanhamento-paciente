INSERT INTO "Psicologos" ("Id", "Nome", "Email", "Password") 
VALUES (1, 'Dr. Admin', 'admin@clinica.com', 'admin123')
ON CONFLICT ("Id") DO NOTHING;
