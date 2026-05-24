CREATE DATABASE FoodDeliveryDB;

USE FoodDeliveryDB;

CREATE TABLE Cliente (
    ClienteId INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(150) UNIQUE NOT NULL,
    Telefone VARCHAR(20),
    Endereco VARCHAR(255),
    Senha VARCHAR(20) NOT NULL,
    Cpf INT UNIQUE NOT NULL,
    DataCadastro DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Restaurante (
    RestauranteId INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(150) NOT NULL,
    CNPJ VARCHAR(20) UNIQUE NOT NULL,
    Endereco VARCHAR(255),
    Telefone VARCHAR(20),
    Senha VARCHAR(20) NOT NULL,
    Ativo BOOL DEFAULT TRUE
);

CREATE TABLE Prato (
    PratoId INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(150) NOT NULL,
    Descricao VARCHAR(255),
    Preco DECIMAL(10,2) NOT NULL,
    RestauranteId INT NOT NULL,
    Disponivel BOOL DEFAULT TRUE,
    FOREIGN KEY (RestauranteId) REFERENCES Restaurante(RestauranteId)
);

CREATE TABLE Entregador (
    EntregadorId INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Telefone VARCHAR(20),
    Veiculo VARCHAR(50),
    Senha VARCHAR(20) NOT NULL,
    Cpf INT UNIQUE NOT NULL,
    Ativo BOOL DEFAULT TRUE
);

CREATE TABLE Pedido (
    PedidoId INT PRIMARY KEY AUTO_INCREMENT,
    ClienteId INT NOT NULL,
    RestauranteId INT NOT NULL,
    EntregadorId INT NULL,
    DataPedido DATETIME DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(50) DEFAULT 'Pendente',
    ValorTotal DECIMAL(10,2),

    FOREIGN KEY (ClienteId) REFERENCES Cliente(ClienteId),
    FOREIGN KEY (RestauranteId) REFERENCES Restaurante(RestauranteId),
    FOREIGN KEY (EntregadorId) REFERENCES Entregador(EntregadorId)
);

CREATE TABLE Admin (
AdminId INT PRIMARY KEY AUTO_INCREMENT,	
Nome VARCHAR(100) NOT NULL,
Senha VARCHAR(20) NOT NULL,
Cpf INT UNIQUE NOT NULL
);

INSERT INTO Admin  (Nome, Senha, Cpf) VALUES ('Admin', 'Admin', 12345678);
INSERT INTO Entregador (Nome, Senha, Cpf) VALUES ('Entregador', 'Entregador', 12345678);
INSERT INTO Cliente  (Nome, Senha, Email, Cpf) VALUES ('Cliente', 'Cliente', 'Cliente', 12345678);
INSERT INTO Restaurante  (Nome, Senha, CNPJ) VALUES ('Restaurante', 'Restaurante', 12345678);

CREATE TABLE PedidoItem (
    PedidoItemId INT PRIMARY KEY AUTO_INCREMENT,
    PedidoId INT NOT NULL,
    PratoId INT NOT NULL,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(10,2) NOT NULL,

    FOREIGN KEY (PedidoId) REFERENCES Pedido(PedidoId),
    FOREIGN KEY (PratoId) REFERENCES Prato(PratoId)
);