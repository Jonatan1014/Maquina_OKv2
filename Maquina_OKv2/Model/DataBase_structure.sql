-- Crear base de datos
-- CREATE DATABASE maquina_bd;
-- GO

-- Usar la base de datos creada
-- USE maquina_bd;
-- GO

-- Tabla Usuarios
CREATE TABLE Usuarios (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Edad INT CHECK (Edad > 0),
    Cargo VARCHAR(50) NOT NULL,
    CorreoElectronico VARCHAR(100) UNIQUE NOT NULL,
    Contraseña VARCHAR(255) NOT NULL
);
GO

-- Tabla Normas
CREATE TABLE Normas (
    IdNorma INT PRIMARY KEY IDENTITY(1,1),
    CodigoNorma VARCHAR(20) UNIQUE NOT NULL,
    Descripcion VARCHAR(255),
    RutaImagen VARCHAR(255) -- Ruta a la imagen ilustrativa de la norma
);
GO

-- Tabla Maquinas
CREATE TABLE Maquinas (
    IdMaquina INT PRIMARY KEY IDENTITY(1,1),
    NombreMaquina VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(255)
);
GO

-- Tabla Pruebas
CREATE TABLE Pruebas (
    IdPrueba INT PRIMARY KEY IDENTITY(1,1),
    IdMaquina INT NOT NULL,
    IdNorma INT NOT NULL,
    IdUsuario INT NOT NULL,
    NivelacionFrente DECIMAL(5, 2),
    NivelacionCostado DECIMAL(5, 2),
    PlanicidadMesa DECIMAL(5, 2),
    AlabeoMesa DECIMAL(5, 2),
    Estado VARCHAR(10) CHECK (Estado IN ('Cumple', 'No Cumple')), -- Estado restringido a 'Cumple' y 'No Cumple'
    FechaPrueba DATETIME DEFAULT GETDATE(),
    RutaImagenPrueba VARCHAR(255), -- Ruta a la imagen específica de la prueba
    FOREIGN KEY (IdMaquina) REFERENCES Maquinas(IdMaquina),
    FOREIGN KEY (IdNorma) REFERENCES Normas(IdNorma),
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);
GO

-- Tabla Instrumentos
CREATE TABLE Instrumentos (
    IdInstrumento INT PRIMARY KEY IDENTITY(1,1),
    NombreInstrumento VARCHAR(100) NOT NULL,
    DescripcionUso VARCHAR(255)
);
GO

-- Tabla PruebaInstrumento (relación entre Pruebas e Instrumentos)
CREATE TABLE PruebaInstrumento (
    IdPruebaInstrumento INT PRIMARY KEY IDENTITY(1,1),
    IdPrueba INT NOT NULL,
    IdInstrumento INT NOT NULL,
    FOREIGN KEY (IdPrueba) REFERENCES Pruebas(IdPrueba),
    FOREIGN KEY (IdInstrumento) REFERENCES Instrumentos(IdInstrumento)
);
GO

-- Tabla HistorialPruebas
CREATE TABLE HistorialPruebas (
    IdHistorial INT PRIMARY KEY IDENTITY(1,1),
    IdPrueba INT NOT NULL,
    FechaModificacion DATETIME DEFAULT GETDATE(),
    EstadoAnterior VARCHAR(20),
    EstadoNuevo VARCHAR(20),
    FOREIGN KEY (IdPrueba) REFERENCES Pruebas(IdPrueba)
);
GO
