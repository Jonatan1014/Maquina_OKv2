--  Crear base de datos
-- CREATE DATABASE ControlCalibracion;
-- GO

-- Usar la base de datos creada
-- USE ControlCalibracion;
-- GO

-- Tabla Usuarios
CREATE TABLE Usuarios (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Estado VARCHAR(10) CHECK (Estado IN ('Activo', 'Inactivo')),
    Rol VARCHAR(10) CHECK (Rol IN ('User', 'Admin')),
    CorreoElectronico VARCHAR(100) UNIQUE NOT NULL,
    Contrasena VARCHAR(255) NOT NULL
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


-- Insertar usuarios
INSERT INTO Usuarios (Nombre, Apellido, Estado, Rol, CorreoElectronico, Contrasena) 
VALUES ('Juan', 'Perez','Inactivo','User' ,'juan@gmail.com', '123'),
       ('Maria', 'Gomez','Inactivo','User', 'maria@gmail.com', '123'),
       ('Jonatan', 'Cantillo','Activo','Admin', 'a', 'a');

-- Insertar normas
INSERT INTO Normas (CodigoNorma, Descripcion) 
VALUES ('NTC 1900', 'Norma para pruebas de nivelación de máquinas'),
       ('NTC 1938', 'Norma para pruebas de precisión en guías y husillo');

-- Insertar máquinas
INSERT INTO Maquinas (NombreMaquina, Descripcion) 
VALUES ('Torno XYZ', 'Torno de precisión para calibración de nivelación y planicidad'),
       ('Fresadora ABC', 'Fresadora de alta precisión para calibración de guías y husillo');

INSERT INTO Parametros (IdNorma, NombreParametro, LimiteInferior, LimiteSuperior, Unidad) 
VALUES 
(1, 'Nivelación de Máquina', 0.00, 0.03, 'mm'),
(1, 'Planicidad de la Mesa', 0.00, 0.03, 'mm'),
(1, 'Alabeo de la Mesa Rotante', 0.00, 0.05, 'mm'),
(1, 'Desalineamiento del Husillo', 0.00, 0.02, 'mm');

INSERT INTO Parametros (IdNorma, NombreParametro, LimiteInferior, LimiteSuperior, Unidad) 
VALUES 
(2, 'Nivelación de las Guías', 0.00, 0.02, 'mm'),
(2, 'Rectitud de las Guías en Plano Horizontal', 0.00, 0.02, 'mm'),
(2, 'Planicidad de la Mesa', 0.00, 0.01, 'mm'),
(2, 'Alabeo de la Nariz del Husillo', 0.00, 0.01, 'mm');

-- Insertar una prueba para la norma NTC 1900 en la máquina Torno XYZ, realizada por Juan
INSERT INTO Pruebas (IdMaquina, IdNorma, IdUsuario, Estado, ImagenPrueba) 
VALUES (1, 1, 1, 'Cumple', (SELECT * FROM OPENROWSET(BULK N'C:/1900/image_6.png', SINGLE_BLOB) AS ImageData));

-- Insertar una prueba para la norma NTC 1938 en la máquina Fresadora ABC, realizada por Maria
INSERT INTO Pruebas (IdMaquina, IdNorma, IdUsuario, Estado, ImagenPrueba) 
VALUES (2, 2, 2, 'No Cumple', (SELECT * FROM OPENROWSET(BULK N'C:/1938/image_14.png', SINGLE_BLOB) AS ImageData));


-- Resultados para la prueba 1 (Norma NTC 1900)
INSERT INTO ResultadosPrueba (IdPrueba, IdParametro, ValorMedido, EstadoParametro) 
VALUES 
(1, 1, 0.02, 'Cumple'), -- Nivelación de Máquina
(1, 2, 0.01, 'Cumple'), -- Planicidad de la Mesa
(1, 3, 0.04, 'Cumple'), -- Alabeo de la Mesa Rotante
(1, 4, 0.015, 'Cumple'); -- Desalineamiento del Husillo

-- Resultados para la prueba 2 (Norma NTC 1938)
INSERT INTO ResultadosPrueba (IdPrueba, IdParametro, ValorMedido, EstadoParametro) 
VALUES 
(2, 5, 0.03, 'No Cumple'), -- Nivelación de las Guías
(2, 6, 0.02, 'Cumple'), -- Rectitud de las Guías en Plano Horizontal
(2, 7, 0.015, 'No Cumple'), -- Planicidad de la Mesa
(2, 8, 0.01, 'Cumple'); -- Alabeo de la Nariz del Husillo

-- Insertar imágenes de referencia para parámetros de la norma NTC 1900
INSERT INTO ImagenesParametros (IdParametro, Imagen, Descripcion) 
VALUES 
(1, (SELECT * FROM OPENROWSET(BULK N'C:/1900/image_6.png', SINGLE_BLOB) AS ImageData), 'Nivelación de Máquina'),
(2, (SELECT * FROM OPENROWSET(BULK N'C:/1900/image_4.png', SINGLE_BLOB) AS ImageData), 'Planicidad de la Mesa'),
(3, (SELECT * FROM OPENROWSET(BULK N'C:/1900/image_1.png', SINGLE_BLOB) AS ImageData), 'Alabeo de la Mesa Rotante'),
(4, (SELECT * FROM OPENROWSET(BULK N'C:/1900/image_5.png', SINGLE_BLOB) AS ImageData), 'Desalineamiento del Husillo');

-- Insertar imágenes de referencia para parámetros de la norma NTC 1938
INSERT INTO ImagenesParametros (IdParametro, Imagen, Descripcion) 
VALUES 
(5, (SELECT * FROM OPENROWSET(BULK N'C:/1938/image_14.png', SINGLE_BLOB) AS ImageData), 'Nivelación de las Guías'),
(6, (SELECT * FROM OPENROWSET(BULK N'C:/1938/image_8.png', SINGLE_BLOB) AS ImageData), 'Rectitud de las Guías en Plano Horizontal'),
(7, (SELECT * FROM OPENROWSET(BULK N'C:/1938/image_3.png', SINGLE_BLOB) AS ImageData), 'Planicidad de la Mesa'),
(8, (SELECT * FROM OPENROWSET(BULK N'C:/1938/image_1.png', SINGLE_BLOB) AS ImageData), 'Alabeo de la Nariz del Husillo');
