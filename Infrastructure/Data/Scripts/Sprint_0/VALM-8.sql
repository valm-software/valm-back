START TRANSACTION;

CREATE TABLE `Auth_Permisos` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombre` longtext COLLATE utf8mb4_general_ci NULL,
    `Modulo` longtext COLLATE utf8mb4_general_ci NULL,
    CONSTRAINT `PK_Auth_Permisos` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `Auth_Roles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombre` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
    CONSTRAINT `PK_Auth_Roles` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `Auth_Usuarios` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Usuario` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
    `Password` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
    `Nombre` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
    `Correo` varchar(255) COLLATE utf8mb4_general_ci NULL,
    `Dni` varchar(255) COLLATE utf8mb4_general_ci NULL,
    CONSTRAINT `PK_Auth_Usuarios` PRIMARY KEY (`Id`)
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `Auth_RolesPermisos` (
    `RolId` int NOT NULL,
    `PermisoId` int NOT NULL,
    CONSTRAINT `PK_Auth_RolesPermisos` PRIMARY KEY (`RolId`, `PermisoId`),
    CONSTRAINT `FK_Auth_RolesPermisos_Auth_Roles_RolId` FOREIGN KEY (`RolId`) REFERENCES `Auth_Roles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Auth_RolesPermisos_Auth_Permisos_PermisoId` FOREIGN KEY (`PermisoId`) REFERENCES `Auth_Permisos` (`Id`) ON DELETE CASCADE
) COLLATE=utf8mb4_general_ci;

CREATE TABLE `Auth_UsuariosRoles` (
    `UsuarioId` int NOT NULL,
    `RolId` int NOT NULL,
    CONSTRAINT `PK_Auth_UsuariosRoles` PRIMARY KEY (`UsuarioId`, `RolId`),
    CONSTRAINT `FK_Auth_UsuariosRoles_Auth_Roles_RolId` FOREIGN KEY (`RolId`) REFERENCES `Auth_Roles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Auth_UsuariosRoles_Auth_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `Auth_Usuarios` (`Id`) ON DELETE CASCADE
) COLLATE=utf8mb4_general_ci;

CREATE INDEX `IX_Auth_RolesPermisos_PermisoId` ON `Auth_RolesPermisos` (`PermisoId`);

CREATE INDEX `IX_Auth_UsuariosRoles_RolId` ON `Auth_UsuariosRoles` (`RolId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230930114826_VALM-8', '7.0.10');

COMMIT;

