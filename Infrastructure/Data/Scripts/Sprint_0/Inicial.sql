CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

CREATE TABLE `AuthPoliticas` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `NombrePolitica` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
    `Modulo` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
    CONSTRAINT `PK_AuthPoliticas` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `AuthRoles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `NombreRol` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
    CONSTRAINT `PK_AuthRoles` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `AuthUsuarios` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Usuario` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
    `Password` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
    `Nombre` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
    `Correo` varchar(255) COLLATE utf8mb4_general_ci NULL,
    `DNI` varchar(255) COLLATE utf8mb4_general_ci NULL,
    CONSTRAINT `PK_AuthUsuarios` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

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

CREATE TABLE `AuthRolesPoliticas` (
    `RolId` int NOT NULL,
    `PoliticaId` int NOT NULL,
    CONSTRAINT `PK_AuthRolesPoliticas` PRIMARY KEY (`RolId`, `PoliticaId`),
    CONSTRAINT `FK_AuthRolesPoliticas_AuthPoliticas_PoliticaId` FOREIGN KEY (`PoliticaId`) REFERENCES `AuthPoliticas` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AuthRolesPoliticas_AuthRoles_RolId` FOREIGN KEY (`RolId`) REFERENCES `AuthRoles` (`Id`) ON DELETE CASCADE
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `AuthUsuariosRoles` (
    `UsuarioId` int NOT NULL,
    `RolId` int NOT NULL,
    CONSTRAINT `PK_AuthUsuariosRoles` PRIMARY KEY (`UsuarioId`, `RolId`),
    CONSTRAINT `FK_AuthUsuariosRoles_AuthRoles_RolId` FOREIGN KEY (`RolId`) REFERENCES `AuthRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AuthUsuariosRoles_AuthUsuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `AuthUsuarios` (`Id`) ON DELETE CASCADE
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

CREATE INDEX `IX_AuthRolesPoliticas_PoliticaId` ON `AuthRolesPoliticas` (`PoliticaId`);

CREATE INDEX `IX_AuthUsuariosRoles_RolId` ON `AuthUsuariosRoles` (`RolId`);

CREATE INDEX `IX_Producto_CategoriaId` ON `Producto` (`CategoriaId`);

CREATE INDEX `IX_Producto_MarcaId` ON `Producto` (`MarcaId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20231003151329_Inicial', '7.0.10');

COMMIT;

