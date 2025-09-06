CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `AmbitoAplicacion` (
        `id_ambito_aplicacion` int NOT NULL AUTO_INCREMENT,
        `nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `descripcion` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        CONSTRAINT `PK_AmbitoAplicacion` PRIMARY KEY (`id_ambito_aplicacion`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `CCMICategoria` (
        `id_ccmi_categoria` int NOT NULL AUTO_INCREMENT,
        `nombre` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `codigo` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `peso_categoria` decimal(5,2) NOT NULL,
        `orden` int NOT NULL,
        `activo` tinyint(1) NOT NULL,
        `fecha_creacion` datetime(6) NOT NULL,
        `fecha_actualizacion` datetime(6) NULL,
        CONSTRAINT `PK_CCMICategoria` PRIMARY KEY (`id_ccmi_categoria`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `Estado` (
        `id_estado` int NOT NULL AUTO_INCREMENT,
        `codigo` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `nombre` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `descripcion` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `color_hex` varchar(7) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        CONSTRAINT `PK_Estado` PRIMARY KEY (`id_estado`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `Pais` (
        `id_pais` int NOT NULL AUTO_INCREMENT,
        `nombre` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `codigo_iso` varchar(3) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        CONSTRAINT `PK_Pais` PRIMARY KEY (`id_pais`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `Rol` (
        `id_rol` int NOT NULL AUTO_INCREMENT,
        `nombre` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `descripcion` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        CONSTRAINT `PK_Rol` PRIMARY KEY (`id_rol`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `Sector` (
        `id_sector` int NOT NULL AUTO_INCREMENT,
        `nombre` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `descripcion` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        CONSTRAINT `PK_Sector` PRIMARY KEY (`id_sector`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `TipoElemento` (
        `id_tipo_elemento` int NOT NULL AUTO_INCREMENT,
        `nombre` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `descripcion` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        CONSTRAINT `PK_TipoElemento` PRIMARY KEY (`id_tipo_elemento`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `Usuario` (
        `id_usuario` int NOT NULL AUTO_INCREMENT,
        `nombre` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `apellido` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `correo` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `contrasena_hash` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `fecha_creacion` datetime(6) NOT NULL,
        `fecha_actualizacion` datetime(6) NULL,
        `id_pais` int NOT NULL,
        `id_estado` int NOT NULL,
        `id_rol` int NOT NULL,
        CONSTRAINT `PK_Usuario` PRIMARY KEY (`id_usuario`),
        CONSTRAINT `FK_Usuario_Estado_id_estado` FOREIGN KEY (`id_estado`) REFERENCES `Estado` (`id_estado`) ON DELETE CASCADE,
        CONSTRAINT `FK_Usuario_Pais_id_pais` FOREIGN KEY (`id_pais`) REFERENCES `Pais` (`id_pais`) ON DELETE CASCADE,
        CONSTRAINT `FK_Usuario_Rol_id_rol` FOREIGN KEY (`id_rol`) REFERENCES `Rol` (`id_rol`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `Empresa` (
        `id_empresa` int NOT NULL AUTO_INCREMENT,
        `nombre` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `representante` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `nit` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `logo` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `id_pais` int NOT NULL,
        `id_estado` int NOT NULL,
        `fecha_creacion` datetime(6) NOT NULL,
        `fecha_actualizacion` datetime(6) NULL,
        `id_sector` int NOT NULL,
        CONSTRAINT `PK_Empresa` PRIMARY KEY (`id_empresa`),
        CONSTRAINT `FK_Empresa_Estado_id_estado` FOREIGN KEY (`id_estado`) REFERENCES `Estado` (`id_estado`) ON DELETE CASCADE,
        CONSTRAINT `FK_Empresa_Pais_id_pais` FOREIGN KEY (`id_pais`) REFERENCES `Pais` (`id_pais`) ON DELETE CASCADE,
        CONSTRAINT `FK_Empresa_Sector_id_sector` FOREIGN KEY (`id_sector`) REFERENCES `Sector` (`id_sector`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `CMMIEvaluacion` (
        `id_evaluacion` int NOT NULL AUTO_INCREMENT,
        `id_empresa` int NOT NULL,
        `id_usuario_auditor` int NOT NULL,
        `fecha_inicio` datetime(6) NOT NULL,
        `fecha_cierre` datetime(6) NULL,
        `id_estado` int NOT NULL,
        `puntaje_global` decimal(6,2) NULL,
        `nivel_madurez` tinyint unsigned NULL,
        `observaciones` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `fecha_creacion` datetime(6) NOT NULL,
        `fecha_actualizacion` datetime(6) NULL,
        CONSTRAINT `PK_CMMIEvaluacion` PRIMARY KEY (`id_evaluacion`),
        CONSTRAINT `FK_CMMIEvaluacion_Empresa_id_empresa` FOREIGN KEY (`id_empresa`) REFERENCES `Empresa` (`id_empresa`) ON DELETE CASCADE,
        CONSTRAINT `FK_CMMIEvaluacion_Estado_id_estado` FOREIGN KEY (`id_estado`) REFERENCES `Estado` (`id_estado`) ON DELETE CASCADE,
        CONSTRAINT `FK_CMMIEvaluacion_Usuario_id_usuario_auditor` FOREIGN KEY (`id_usuario_auditor`) REFERENCES `Usuario` (`id_usuario`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `Legislacion` (
        `id_legislacion` int NOT NULL AUTO_INCREMENT,
        `id_empresa` int NOT NULL,
        `id_estado` int NOT NULL,
        `id_ambito_aplicacion` int NOT NULL,
        `id_usuario_creador` int NOT NULL,
        `titulo` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `subtitulo` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `alias` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `codigo_interno` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `fecha_vigencia` datetime(6) NULL,
        `archivo_pdf_url` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `fecha_creacion` datetime(6) NOT NULL,
        `fecha_actualizacion` datetime(6) NULL,
        `id_pais` int NOT NULL,
        CONSTRAINT `PK_Legislacion` PRIMARY KEY (`id_legislacion`),
        CONSTRAINT `FK_Legislacion_AmbitoAplicacion_id_ambito_aplicacion` FOREIGN KEY (`id_ambito_aplicacion`) REFERENCES `AmbitoAplicacion` (`id_ambito_aplicacion`) ON DELETE CASCADE,
        CONSTRAINT `FK_Legislacion_Empresa_id_empresa` FOREIGN KEY (`id_empresa`) REFERENCES `Empresa` (`id_empresa`) ON DELETE CASCADE,
        CONSTRAINT `FK_Legislacion_Estado_id_estado` FOREIGN KEY (`id_estado`) REFERENCES `Estado` (`id_estado`) ON DELETE CASCADE,
        CONSTRAINT `FK_Legislacion_Pais_id_pais` FOREIGN KEY (`id_pais`) REFERENCES `Pais` (`id_pais`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `UsuarioEmpresa` (
        `id_usuario_empresa` int NOT NULL AUTO_INCREMENT,
        `id_usuario` int NOT NULL,
        `id_empresa` int NOT NULL,
        `id_rol` int NOT NULL,
        `fecha_asignacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_UsuarioEmpresa` PRIMARY KEY (`id_usuario_empresa`),
        CONSTRAINT `FK_UsuarioEmpresa_Empresa_id_empresa` FOREIGN KEY (`id_empresa`) REFERENCES `Empresa` (`id_empresa`) ON DELETE CASCADE,
        CONSTRAINT `FK_UsuarioEmpresa_Rol_id_rol` FOREIGN KEY (`id_rol`) REFERENCES `Rol` (`id_rol`) ON DELETE CASCADE,
        CONSTRAINT `FK_UsuarioEmpresa_Usuario_id_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `Usuario` (`id_usuario`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `CMMIPregunta` (
        `id_cmmi_pregunta` int NOT NULL AUTO_INCREMENT,
        `id_ccmi_categoria` int NOT NULL,
        `codigo` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `texto` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
        `peso_pregunta` decimal(5,2) NOT NULL,
        `es_critica` tinyint(1) NOT NULL,
        `orden` int NOT NULL,
        `activo` tinyint(1) NOT NULL,
        `fecha_creacion` datetime(6) NOT NULL,
        `fecha_actualizacion` datetime(6) NULL,
        CONSTRAINT `PK_CMMIPregunta` PRIMARY KEY (`id_cmmi_pregunta`),
        CONSTRAINT `FK_CMMIPregunta_CCMICategoria_id_ccmi_categoria` FOREIGN KEY (`id_ccmi_categoria`) REFERENCES `CCMICategoria` (`id_ccmi_categoria`) ON DELETE CASCADE,
        CONSTRAINT `FK_CMMIPregunta_CMMIEvaluacion_id_ccmi_categoria` FOREIGN KEY (`id_ccmi_categoria`) REFERENCES `CMMIEvaluacion` (`id_evaluacion`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `CicloAuditoria` (
        `id_ciclo_auditoria` int NOT NULL AUTO_INCREMENT,
        `id_legislacion` int NOT NULL,
        `id_estado` int NOT NULL,
        `id_usuario_aprobador` int NOT NULL,
        `fecha_inicio` datetime(6) NOT NULL,
        `fecha_cierre` datetime(6) NULL,
        `total_segmentos` int NOT NULL,
        `total_aprobados` int NOT NULL,
        `nivel_cmmi` int NOT NULL,
        `porcentaje_aprobado` decimal(65,30) NULL,
        `motivo_cierre` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `resumen` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        CONSTRAINT `PK_CicloAuditoria` PRIMARY KEY (`id_ciclo_auditoria`),
        CONSTRAINT `FK_CicloAuditoria_Estado_id_estado` FOREIGN KEY (`id_estado`) REFERENCES `Estado` (`id_estado`) ON DELETE CASCADE,
        CONSTRAINT `FK_CicloAuditoria_Legislacion_id_legislacion` FOREIGN KEY (`id_legislacion`) REFERENCES `Legislacion` (`id_legislacion`) ON DELETE CASCADE,
        CONSTRAINT `FK_CicloAuditoria_Usuario_id_usuario_aprobador` FOREIGN KEY (`id_usuario_aprobador`) REFERENCES `Usuario` (`id_usuario`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `SegmentoLegislacion` (
        `id_segmento_legislacion` int NOT NULL AUTO_INCREMENT,
        `id_legislacion` int NOT NULL,
        `id_tipo_elemento` int NOT NULL,
        `id_segmento_padre` int NULL,
        `contenido` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `observaciones` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `orden` int NOT NULL,
        `tituloSegmento` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `contenido_url` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `contenido_bin` longblob NULL,
        `fecha_creacion` datetime(6) NOT NULL,
        `fecha_actualizacion` datetime(6) NULL,
        CONSTRAINT `PK_SegmentoLegislacion` PRIMARY KEY (`id_segmento_legislacion`),
        CONSTRAINT `FK_SegmentoLegislacion_Legislacion_id_legislacion` FOREIGN KEY (`id_legislacion`) REFERENCES `Legislacion` (`id_legislacion`) ON DELETE CASCADE,
        CONSTRAINT `FK_SegmentoLegislacion_SegmentoLegislacion_id_segmento_padre` FOREIGN KEY (`id_segmento_padre`) REFERENCES `SegmentoLegislacion` (`id_segmento_legislacion`),
        CONSTRAINT `FK_SegmentoLegislacion_TipoElemento_id_tipo_elemento` FOREIGN KEY (`id_tipo_elemento`) REFERENCES `TipoElemento` (`id_tipo_elemento`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `CMMIRespuesta` (
        `id_respuesta` int NOT NULL AUTO_INCREMENT,
        `id_evaluacion` int NOT NULL,
        `id_ccmi_pregunta` int NOT NULL,
        `valor` tinyint unsigned NOT NULL,
        `nota` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `evidencia_url` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `fecha_creacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_CMMIRespuesta` PRIMARY KEY (`id_respuesta`),
        CONSTRAINT `FK_CMMIRespuesta_CMMIEvaluacion_id_evaluacion` FOREIGN KEY (`id_evaluacion`) REFERENCES `CMMIEvaluacion` (`id_evaluacion`) ON DELETE CASCADE,
        CONSTRAINT `FK_CMMIRespuesta_CMMIPregunta_id_ccmi_pregunta` FOREIGN KEY (`id_ccmi_pregunta`) REFERENCES `CMMIPregunta` (`id_cmmi_pregunta`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `EvaluacionSegmento` (
        `id_evaluacion_segmento` int NOT NULL AUTO_INCREMENT,
        `id_ciclo_auditoria` int NOT NULL,
        `id_segmento_legislacion` int NOT NULL,
        `id_usuario_auditor` int NOT NULL,
        `id_estado` int NOT NULL,
        `comentario` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `fecha_evaluacion` datetime(6) NOT NULL,
        `fecha_actualizacion` datetime(6) NOT NULL,
        CONSTRAINT `PK_EvaluacionSegmento` PRIMARY KEY (`id_evaluacion_segmento`),
        CONSTRAINT `FK_EvaluacionSegmento_CicloAuditoria_id_ciclo_auditoria` FOREIGN KEY (`id_ciclo_auditoria`) REFERENCES `CicloAuditoria` (`id_ciclo_auditoria`) ON DELETE CASCADE,
        CONSTRAINT `FK_EvaluacionSegmento_Estado_id_estado` FOREIGN KEY (`id_estado`) REFERENCES `Estado` (`id_estado`) ON DELETE CASCADE,
        CONSTRAINT `FK_EvaluacionSegmento_SegmentoLegislacion_id_segmento_legislaci~` FOREIGN KEY (`id_segmento_legislacion`) REFERENCES `SegmentoLegislacion` (`id_segmento_legislacion`) ON DELETE CASCADE,
        CONSTRAINT `FK_EvaluacionSegmento_Usuario_id_usuario_auditor` FOREIGN KEY (`id_usuario_auditor`) REFERENCES `Usuario` (`id_usuario`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE TABLE `Evidencia` (
        `id_evidencia` int NOT NULL AUTO_INCREMENT,
        `id_evaluacion_segmento` int NOT NULL,
        `archivo_url` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `nombre_original` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `mime_type` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `tamano_bytes` int NULL,
        `sha256` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `tipo_documento` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `comentario_opcional` varchar(400) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
        `fecha_subida` datetime(6) NOT NULL,
        `fecha_actualizacion` datetime(6) NULL,
        `id_usuario` int NULL,
        CONSTRAINT `PK_Evidencia` PRIMARY KEY (`id_evidencia`),
        CONSTRAINT `FK_Evidencia_EvaluacionSegmento_id_evaluacion_segmento` FOREIGN KEY (`id_evaluacion_segmento`) REFERENCES `EvaluacionSegmento` (`id_evaluacion_segmento`) ON DELETE CASCADE,
        CONSTRAINT `FK_Evidencia_Usuario_id_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `Usuario` (`id_usuario`)
    ) CHARACTER SET=utf8mb4 COLLATE=utf8mb4_general_ci;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_CCMICategoria_nombre` ON `CCMICategoria` (`nombre`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_CicloAuditoria_id_estado` ON `CicloAuditoria` (`id_estado`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_CicloAuditoria_id_legislacion` ON `CicloAuditoria` (`id_legislacion`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_CicloAuditoria_id_usuario_aprobador` ON `CicloAuditoria` (`id_usuario_aprobador`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_CMMIEvaluacion_id_empresa` ON `CMMIEvaluacion` (`id_empresa`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_CMMIEvaluacion_id_estado` ON `CMMIEvaluacion` (`id_estado`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_CMMIEvaluacion_id_usuario_auditor` ON `CMMIEvaluacion` (`id_usuario_auditor`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_CMMIPregunta_id_ccmi_categoria` ON `CMMIPregunta` (`id_ccmi_categoria`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_CMMIRespuesta_id_ccmi_pregunta` ON `CMMIRespuesta` (`id_ccmi_pregunta`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_CMMIRespuesta_id_evaluacion` ON `CMMIRespuesta` (`id_evaluacion`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Empresa_id_estado` ON `Empresa` (`id_estado`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Empresa_id_pais` ON `Empresa` (`id_pais`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Empresa_id_sector` ON `Empresa` (`id_sector`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_EvaluacionSegmento_id_ciclo_auditoria` ON `EvaluacionSegmento` (`id_ciclo_auditoria`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_EvaluacionSegmento_id_estado` ON `EvaluacionSegmento` (`id_estado`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_EvaluacionSegmento_id_segmento_legislacion` ON `EvaluacionSegmento` (`id_segmento_legislacion`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_EvaluacionSegmento_id_usuario_auditor` ON `EvaluacionSegmento` (`id_usuario_auditor`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Evidencia_id_evaluacion_segmento` ON `Evidencia` (`id_evaluacion_segmento`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Evidencia_id_usuario` ON `Evidencia` (`id_usuario`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Legislacion_id_ambito_aplicacion` ON `Legislacion` (`id_ambito_aplicacion`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Legislacion_id_empresa` ON `Legislacion` (`id_empresa`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Legislacion_id_estado` ON `Legislacion` (`id_estado`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Legislacion_id_pais` ON `Legislacion` (`id_pais`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_SegmentoLegislacion_id_legislacion` ON `SegmentoLegislacion` (`id_legislacion`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_SegmentoLegislacion_id_segmento_padre` ON `SegmentoLegislacion` (`id_segmento_padre`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_SegmentoLegislacion_id_tipo_elemento` ON `SegmentoLegislacion` (`id_tipo_elemento`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Usuario_id_estado` ON `Usuario` (`id_estado`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Usuario_id_pais` ON `Usuario` (`id_pais`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_Usuario_id_rol` ON `Usuario` (`id_rol`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_UsuarioEmpresa_id_empresa` ON `UsuarioEmpresa` (`id_empresa`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_UsuarioEmpresa_id_rol` ON `UsuarioEmpresa` (`id_rol`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    CREATE INDEX `IX_UsuarioEmpresa_id_usuario` ON `UsuarioEmpresa` (`id_usuario`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010046_InitialCreate') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20250906010046_InitialCreate', '8.0.2');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20250906010758_InitialCreate2') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20250906010758_InitialCreate2', '8.0.2');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

