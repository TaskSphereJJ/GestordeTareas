-----Este trigger insertará el UsuarioID en la tabla Administradores si el CargoId es 1 (administrador) o en la tabla Colaboradores si el -----CargoId es 2 (colaborador).

CREATE TRIGGER TR_InsertarAdminColaborador
ON Usuarios
AFTER INSERT
AS
BEGIN
    DECLARE @UsuarioID INT;
    DECLARE @CargoID INT;

    SELECT @UsuarioID = ID, @CargoID = CargoId FROM inserted;

    IF @CargoID = 1
    BEGIN
        INSERT INTO Administradores (UsuarioID)
        VALUES (@UsuarioID);
    END
    ELSE IF @CargoID = 2
    BEGIN
        INSERT INTO Colaboradores (UsuarioID)
        VALUES (@UsuarioID);
    END
END;
