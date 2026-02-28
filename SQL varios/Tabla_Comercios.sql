
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