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

CREATE INDEX `IX_Producto_CategoriaId` ON `Producto` (`CategoriaId`);

CREATE INDEX `IX_Producto_MarcaId` ON `Producto` (`MarcaId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230930114636_Inicial', '7.0.10');

COMMIT;

