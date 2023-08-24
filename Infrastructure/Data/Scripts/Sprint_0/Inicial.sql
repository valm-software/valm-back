CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

CREATE TABLE `Categoria` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombre` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
    CONSTRAINT `PK_Categoria` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `Marca` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombre` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
    CONSTRAINT `PK_Marca` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `ProdProductos` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombre` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
    `Descripcion` varchar(250) COLLATE utf8mb4_general_ci NULL,
    `ValorCosto` decimal(18,2) NOT NULL,
    `ValorVenta` decimal(18,2) NOT NULL,
    `ValorInicial` decimal(18,2) NOT NULL,
    `ValorCuota` decimal(18,2) NOT NULL,
    `NumCuotas` int NOT NULL,
    `ConfActivo` tinyint(1) NOT NULL,
    `ConfPorUtilidad` decimal(18,2) NOT NULL,
    `FechaRegistro` datetime(0) NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    `IdMarca` int NOT NULL,
    CONSTRAINT `PK_ProdProductos` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `UsrPrivilegios` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombre` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
    `Descripcion` varchar(250) COLLATE utf8mb4_general_ci NULL,
    CONSTRAINT `PK_UsrPrivilegios` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `UsrRoles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombre` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
    `Descripcion` varchar(250) COLLATE utf8mb4_general_ci NULL,
    CONSTRAINT `PK_UsrRoles` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `UsrUsuarios` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Usuario` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
    `Contraseña` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
    CONSTRAINT `PK_UsrUsuarios` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `Producto` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombre` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
    `Precio` decimal(18,2) NOT NULL,
    `FechaCreacion` datetime(6) NOT NULL,
    `MarcaId` int NOT NULL,
    `CategoriaId` int NOT NULL,
    CONSTRAINT `PK_Producto` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Producto_Categoria_CategoriaId` FOREIGN KEY (`CategoriaId`) REFERENCES `Categoria` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Producto_Marca_MarcaId` FOREIGN KEY (`MarcaId`) REFERENCES `Marca` (`Id`) ON DELETE CASCADE
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `UsrRolesPrivilegios` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `IdRol` int NOT NULL,
    `IdPrivilegio` int NOT NULL,
    CONSTRAINT `PK_UsrRolesPrivilegios` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UsrRolesPrivilegios_UsrPrivilegios_IdPrivilegio` FOREIGN KEY (`IdPrivilegio`) REFERENCES `UsrPrivilegios` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_UsrRolesPrivilegios_UsrRoles_IdRol` FOREIGN KEY (`IdRol`) REFERENCES `UsrRoles` (`Id`) ON DELETE CASCADE
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `UsrUsuariosRoles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `IdUsuario` int NOT NULL,
    `IdRol` int NOT NULL,
    CONSTRAINT `PK_UsrUsuariosRoles` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UsrUsuariosRoles_UsrRoles_IdRol` FOREIGN KEY (`IdRol`) REFERENCES `UsrRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_UsrUsuariosRoles_UsrUsuarios_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `UsrUsuarios` (`Id`) ON DELETE CASCADE
) COLLATE=utf8mb4_general_ci;

CREATE INDEX `IX_Producto_CategoriaId` ON `Producto` (`CategoriaId`);

CREATE INDEX `IX_Producto_MarcaId` ON `Producto` (`MarcaId`);

CREATE INDEX `IX_UsrRolesPrivilegios_IdPrivilegio` ON `UsrRolesPrivilegios` (`IdPrivilegio`);

CREATE INDEX `IX_UsrRolesPrivilegios_IdRol` ON `UsrRolesPrivilegios` (`IdRol`);

CREATE INDEX `IX_UsrUsuariosRoles_IdRol` ON `UsrUsuariosRoles` (`IdRol`);

CREATE INDEX `IX_UsrUsuariosRoles_IdUsuario` ON `UsrUsuariosRoles` (`IdUsuario`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230822084036_Inicial', '7.0.10');

COMMIT;

