/*
CREATE TABLE Grupo6_Comercios (
    IdComercio INT NOT NULL AUTO_INCREMENT,
    Identificacion VARCHAR(30) NOT NULL,
    TipoIdentificacion INT NOT NULL,
    Nombre VARCHAR(200) NOT NULL,
    TipoDeComercio INT NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    CorreoElectronico VARCHAR(200) NOT NULL,
    Direccion VARCHAR(500) NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
    FechaDeModificacion DATETIME NULL,
    Estado TINYINT(1) NOT NULL DEFAULT 1,
    PRIMARY KEY (IdComercio),
    UNIQUE (Identificacion)
);

CREATE TABLE Grupo6_Cajas (
    IdCaja INT AUTO_INCREMENT PRIMARY KEY,
    IdComercio INT NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    Estado TINYINT(1) NOT NULL DEFAULT 1,
    FechaDeRegistro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FechaDeModificacion DATETIME NULL,

    CONSTRAINT FK_Cajas_Comercios 
    FOREIGN KEY (IdComercio) REFERENCES Grupo6_Comercios(IdComercio),

    CONSTRAINT UQ_Telefono UNIQUE (Telefono)
);

CREATE TABLE Grupo6_SINPE (
    IdSinpe INT AUTO_INCREMENT PRIMARY KEY,
    IdCaja INT NOT NULL,
    TelefonoOrigen VARCHAR(20) NOT NULL,
    TelefonoDestino VARCHAR(20) NOT NULL,
    Monto DECIMAL(10,2) NOT NULL,
    Descripcion VARCHAR(255),
    Estado TINYINT(1) NOT NULL DEFAULT 0,
    FechaDeRegistro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_Sinpe_Caja 
    FOREIGN KEY (IdCaja) REFERENCES Grupo6_Cajas(IdCaja)
);

CREATE TABLE Grupo6_BitacoraEventos (
    IdEvento INT AUTO_INCREMENT PRIMARY KEY,
    Tabla VARCHAR(100),
    TipoEvento VARCHAR(50),
    Descripcion TEXT,
    StackTrace TEXT,
    DatosAnteriores TEXT,
    DatosPosteriores TEXT,
    FechaEvento DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Grupo6_Usuarios (
    IdUsuario INT NOT NULL AUTO_INCREMENT,
    IdComercio INT NOT NULL,
    IdNetUser CHAR(36) NULL,
    Nombres VARCHAR(100) NOT NULL,
    PrimerApellido VARCHAR(100) NOT NULL,
    SegundoApellido VARCHAR(100) NOT NULL,
    Identificacion VARCHAR(10) NOT NULL,
    CorreoElectronico VARCHAR(200) NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
    FechaDeModificacion DATETIME NULL,
    Estado TINYINT(1) NOT NULL DEFAULT 1,
    PRIMARY KEY (IdUsuario),
    UNIQUE (Identificacion),
    CONSTRAINT FK_Usuarios_Comercios
    FOREIGN KEY (IdComercio) REFERENCES Grupo6_Comercios(IdComercio)
);

CREATE TABLE Grupo6_Usuarios (
    IdUsuario INT NOT NULL AUTO_INCREMENT,
    IdComercio INT NOT NULL,
    IdNetUser CHAR(36) NULL,
    Nombres VARCHAR(100) NOT NULL,
    PrimerApellido VARCHAR(100) NOT NULL,
    SegundoApellido VARCHAR(100) NOT NULL,
    Identificacion VARCHAR(10) NOT NULL,
    CorreoElectronico VARCHAR(200) NOT NULL,
    FechaDeRegistro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FechaDeModificacion DATETIME NULL,
    Estado TINYINT(1) NOT NULL DEFAULT 1,
    PRIMARY KEY (IdUsuario),
    CONSTRAINT UQ_Grupo6_Usuarios_Identificacion UNIQUE (Identificacion),
    CONSTRAINT FK_Usuarios_Comercios
        FOREIGN KEY (IdComercio) REFERENCES Grupo6_Comercios(IdComercio)
);

CREATE TABLE Grupo6_ReporteMensual (
    IdReporte INT AUTO_INCREMENT PRIMARY KEY,
    IdComercio INT NOT NULL,
    CantidadDeCajas INT NOT NULL,
    MontoTotalRecaudado DECIMAL(15,2) NOT NULL,
    CantidadDeSINPES INT NOT NULL,
    MontoTotalComision DECIMAL(15,2) NOT NULL,
    FechaDelReporte DATETIME NOT NULL,
    UNIQUE KEY uk_comercio_mes (IdComercio, FechaDelReporte),
    FOREIGN KEY (IdComercio) REFERENCES Grupo6_Comercios(IdComercio)
);

//Indices
CREATE INDEX IDX_Cajas_IdComercio ON Grupo6_Cajas(IdComercio);
CREATE INDEX IDX_Sinpe_IdCaja ON Grupo6_SINPE(IdCaja);
CREATE INDEX IDX_Bitacora_Fecha ON Grupo6_BitacoraEventos(FechaEvento);
CREATE INDEX IDX_Usuarios_IdComercio ON Grupo6_Usuarios(IdComercio);
CREATE INDEX IDX_Usuarios_IdComercio ON Grupo6_Usuarios(IdComercio);
CREATE INDEX IDX_Usuarios_IdNetUser ON Grupo6_Usuarios(IdNetUser);

//Triggers
// Trigger para registrar eventos de inserción en la tabla Grupo6_Cajas
DELIMITER $$

CREATE TRIGGER TR_Cajas_Insert
AFTER INSERT ON Grupo6_Cajas
FOR EACH ROW
BEGIN
    INSERT INTO Grupo6_BitacoraEventos (
        Tabla, TipoEvento, Descripcion, DatosPosteriores
    )
    VALUES (
        'Grupo6_Cajas',
        'INSERT',
        'Nueva caja creada',
        CONCAT('IdCaja: ', NEW.IdCaja, ', Nombre: ', NEW.Nombre)
    );
END$$

DELIMITER ;

// Trigger para registrar eventos de actualización en la tabla Grupo6_Cajas
DELIMITER $$

CREATE TRIGGER TR_Cajas_Update
AFTER UPDATE ON Grupo6_Cajas
FOR EACH ROW
BEGIN
    INSERT INTO Grupo6_BitacoraEventos (
        Tabla, TipoEvento, Descripcion, DatosAnteriores, DatosPosteriores
    )
    VALUES (
        'Grupo6_Cajas',
        'UPDATE',
        'Caja actualizada',
        CONCAT('Nombre: ', OLD.Nombre, ', Telefono: ', OLD.Telefono),
        CONCAT('Nombre: ', NEW.Nombre, ', Telefono: ', NEW.Telefono)
    );
END$$

DELIMITER ;


USE u484426513_pac126;

CREATE TABLE Grupo6_ConfiguracionComercio (
    IdConfiguracion INT AUTO_INCREMENT PRIMARY KEY,
    IdComercio INT NOT NULL,
    TipoConfiguracion INT NOT NULL,
    Comision INT NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
    FechaDeModificacion DATETIME NULL,
    Estado BIT NOT NULL,

    CONSTRAINT FK_Config_Comercio
    FOREIGN KEY (IdComercio)
    REFERENCES Grupo6_Comercios(IdComercio),

    UNIQUE (IdComercio)
);
