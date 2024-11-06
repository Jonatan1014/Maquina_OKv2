-- -- Crear base de datos
-- CREATE DATABASE ControlCalibracion;
-- GO

-- -- Usar la base de datos creada
-- USE ControlCalibracion;
-- GO

-- Tabla Usuarios
CREATE TABLE Usuarios (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    CorreoElectronico VARCHAR(100) UNIQUE NOT NULL,
    Contraseña VARCHAR(255) NOT NULL
);
GO

-- Tabla Normas
CREATE TABLE Normas (
    IdNorma INT PRIMARY KEY IDENTITY(1,1),
    CodigoNorma VARCHAR(20) UNIQUE NOT NULL,
    Descripcion VARCHAR(255)
);
GO

-- Tabla Maquinas
CREATE TABLE Maquinas (
    IdMaquina INT PRIMARY KEY IDENTITY(1,1),
    NombreMaquina VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(255)
);
GO

-- Tabla Parametros
CREATE TABLE Parametros (
    IdParametro INT PRIMARY KEY IDENTITY(1,1),
    IdNorma INT NOT NULL,
    NombreParametro VARCHAR(50) NOT NULL,
    LimiteInferior DECIMAL(10, 2),
    LimiteSuperior DECIMAL(10, 2),
    Unidad VARCHAR(20),
    FOREIGN KEY (IdNorma) REFERENCES Normas(IdNorma)
);
GO

-- Tabla Pruebas
CREATE TABLE Pruebas (
    IdPrueba INT PRIMARY KEY IDENTITY(1,1),
    IdMaquina INT NOT NULL,
    IdNorma INT NOT NULL,
    IdUsuario INT NOT NULL,
    FechaPrueba DATETIME DEFAULT GETDATE(),
    Estado VARCHAR(10) CHECK (Estado IN ('Cumple', 'No Cumple')),
    ImagenPrueba VARBINARY(MAX),
    FOREIGN KEY (IdMaquina) REFERENCES Maquinas(IdMaquina),
    FOREIGN KEY (IdNorma) REFERENCES Normas(IdNorma),
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);
GO

-- Tabla ResultadosPrueba
CREATE TABLE ResultadosPrueba (
    IdResultado INT PRIMARY KEY IDENTITY(1,1),
    IdPrueba INT NOT NULL,
    IdParametro INT NOT NULL,
    ValorMedido DECIMAL(10, 2),
    EstadoParametro VARCHAR(10) CHECK (EstadoParametro IN ('Cumple', 'No Cumple')),
    FOREIGN KEY (IdPrueba) REFERENCES Pruebas(IdPrueba),
    FOREIGN KEY (IdParametro) REFERENCES Parametros(IdParametro)
);
GO

-- Tabla ImagenesParametros
CREATE TABLE ImagenesParametros (
    IdImagenParametro INT PRIMARY KEY IDENTITY(1,1),
    IdParametro INT NOT NULL,
    Imagen VARBINARY(MAX),
    Descripcion VARCHAR(255),
    FOREIGN KEY (IdParametro) REFERENCES Parametros(IdParametro)
);
GO

-- Tabla HistorialPruebas
CREATE TABLE HistorialPruebas (
    IdHistorial INT PRIMARY KEY IDENTITY(1,1),
    IdPrueba INT NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE(),
    EstadoAnterior VARCHAR(10) CHECK (EstadoAnterior IN ('Cumple', 'No Cumple')),
    EstadoNuevo VARCHAR(10) CHECK (EstadoNuevo IN ('Cumple', 'No Cumple')),
    FOREIGN KEY (IdPrueba) REFERENCES Pruebas(IdPrueba)
);
GO
