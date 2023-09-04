START TRANSACTION;

ALTER TABLE `Producto` RENAME COLUMN `Nombre` TO `Nombre_test`;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230904141007_test1', '7.0.10');

COMMIT;

