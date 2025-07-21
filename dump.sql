CREATE TABLE Usuario (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    email VARCHAR(255) NOT NULL UNIQUE,
    senha VARCHAR(255) NOT NULL,
    tipo_usuario int NOT NULL,
);

CREATE TABLE Aluno
(
	id_aluno INT PRIMARY KEY IDENTITY(1,1),
	nome varchar(200),
	dt_nacimento datetime,
	cpf varchar(11) NOT NULL UNIQUE,
	id_usuario INT NOT NULL,
	dt_inclusao datetime null,
	id_usuario_inclusao int null,
	dt_alteracao datetime null,
	id_usuario_alteracao int null
	FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
);

CREATE TABLE Turma 
(
	id_turma INT PRIMARY KEY IDENTITY(1,1),
	nome_turma varchar (50) not null,
	descricao_turma varchar(250) null,
	dt_inclusao datetime null,
	id_usuario_inclusao int null,
	dt_alteracao datetime null,
	id_usuario_alteracao int null
);

CREATE TABLE Matricula 
(
	id_matricula INT PRIMARY KEY IDENTITY(1,1),
	id_aluno INT  NOT NULL,
	id_turma INT  NOT NULL,
	FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno),
	FOREIGN KEY (id_turma) REFERENCES Turma(id_turma),
	CONSTRAINT UQ_Matricula_Aluno_Turma UNIQUE (id_aluno, id_turma)

);

-- Inserir o usuário no banco de dados
INSERT INTO Usuario (email, senha, tipo_usuario)
VALUES ('admin@dominio.com', 'iWWWAwpxnjkYPeMPTX/OKA==:wedYSUot3QqH1bYYJ2YYkztOSrtHbRb+K/TamQUtHsQ=', 1);
 -- tipo_usuario 1 é o exemplo de "Admin"
 -- senha 'senhaAdmin123@'