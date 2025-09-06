using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegislacionAPP.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AmbitoAplicacion",
                columns: table => new
                {
                    id_ambito_aplicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmbitoAplicacion", x => x.id_ambito_aplicacion);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "CCMICategoria",
                columns: table => new
                {
                    id_ccmi_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    peso_categoria = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    orden = table.Column<int>(type: "int", nullable: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CCMICategoria", x => x.id_ccmi_categoria);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Estado",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    codigo = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    color_hex = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estado", x => x.id_estado);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Pais",
                columns: table => new
                {
                    id_pais = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_iso = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.id_pais);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.id_rol);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Sector",
                columns: table => new
                {
                    id_sector = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.id_sector);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "TipoElemento",
                columns: table => new
                {
                    id_tipo_elemento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoElemento", x => x.id_tipo_elemento);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    apellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    correo = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contrasena_hash = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    id_pais = table.Column<int>(type: "int", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_Usuario_Estado_id_estado",
                        column: x => x.id_estado,
                        principalTable: "Estado",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_Pais_id_pais",
                        column: x => x.id_pais,
                        principalTable: "Pais",
                        principalColumn: "id_pais",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol_id_rol",
                        column: x => x.id_rol,
                        principalTable: "Rol",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    id_empresa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    representante = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nit = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    logo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_pais = table.Column<int>(type: "int", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    id_sector = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.id_empresa);
                    table.ForeignKey(
                        name: "FK_Empresa_Estado_id_estado",
                        column: x => x.id_estado,
                        principalTable: "Estado",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Empresa_Pais_id_pais",
                        column: x => x.id_pais,
                        principalTable: "Pais",
                        principalColumn: "id_pais",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Empresa_Sector_id_sector",
                        column: x => x.id_sector,
                        principalTable: "Sector",
                        principalColumn: "id_sector",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "CMMIEvaluacion",
                columns: table => new
                {
                    id_evaluacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_empresa = table.Column<int>(type: "int", nullable: false),
                    id_usuario_auditor = table.Column<int>(type: "int", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_cierre = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    puntaje_global = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    nivel_madurez = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    observaciones = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMMIEvaluacion", x => x.id_evaluacion);
                    table.ForeignKey(
                        name: "FK_CMMIEvaluacion_Empresa_id_empresa",
                        column: x => x.id_empresa,
                        principalTable: "Empresa",
                        principalColumn: "id_empresa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CMMIEvaluacion_Estado_id_estado",
                        column: x => x.id_estado,
                        principalTable: "Estado",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CMMIEvaluacion_Usuario_id_usuario_auditor",
                        column: x => x.id_usuario_auditor,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Legislacion",
                columns: table => new
                {
                    id_legislacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_empresa = table.Column<int>(type: "int", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    id_ambito_aplicacion = table.Column<int>(type: "int", nullable: false),
                    id_usuario_creador = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    subtitulo = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    alias = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_interno = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_vigencia = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    archivo_pdf_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    id_pais = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legislacion", x => x.id_legislacion);
                    table.ForeignKey(
                        name: "FK_Legislacion_AmbitoAplicacion_id_ambito_aplicacion",
                        column: x => x.id_ambito_aplicacion,
                        principalTable: "AmbitoAplicacion",
                        principalColumn: "id_ambito_aplicacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Legislacion_Empresa_id_empresa",
                        column: x => x.id_empresa,
                        principalTable: "Empresa",
                        principalColumn: "id_empresa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Legislacion_Estado_id_estado",
                        column: x => x.id_estado,
                        principalTable: "Estado",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Legislacion_Pais_id_pais",
                        column: x => x.id_pais,
                        principalTable: "Pais",
                        principalColumn: "id_pais",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "UsuarioEmpresa",
                columns: table => new
                {
                    id_usuario_empresa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_empresa = table.Column<int>(type: "int", nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: false),
                    fecha_asignacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEmpresa", x => x.id_usuario_empresa);
                    table.ForeignKey(
                        name: "FK_UsuarioEmpresa_Empresa_id_empresa",
                        column: x => x.id_empresa,
                        principalTable: "Empresa",
                        principalColumn: "id_empresa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioEmpresa_Rol_id_rol",
                        column: x => x.id_rol,
                        principalTable: "Rol",
                        principalColumn: "id_rol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioEmpresa_Usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "CMMIPregunta",
                columns: table => new
                {
                    id_cmmi_pregunta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_ccmi_categoria = table.Column<int>(type: "int", nullable: false),
                    codigo = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    texto = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    peso_pregunta = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    es_critica = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    orden = table.Column<int>(type: "int", nullable: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMMIPregunta", x => x.id_cmmi_pregunta);
                    table.ForeignKey(
                        name: "FK_CMMIPregunta_CCMICategoria_id_ccmi_categoria",
                        column: x => x.id_ccmi_categoria,
                        principalTable: "CCMICategoria",
                        principalColumn: "id_ccmi_categoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CMMIPregunta_CMMIEvaluacion_id_ccmi_categoria",
                        column: x => x.id_ccmi_categoria,
                        principalTable: "CMMIEvaluacion",
                        principalColumn: "id_evaluacion",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "CicloAuditoria",
                columns: table => new
                {
                    id_ciclo_auditoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_legislacion = table.Column<int>(type: "int", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    id_usuario_aprobador = table.Column<int>(type: "int", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_cierre = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    total_segmentos = table.Column<int>(type: "int", nullable: false),
                    total_aprobados = table.Column<int>(type: "int", nullable: false),
                    nivel_cmmi = table.Column<int>(type: "int", nullable: false),
                    porcentaje_aprobado = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    motivo_cierre = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    resumen = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CicloAuditoria", x => x.id_ciclo_auditoria);
                    table.ForeignKey(
                        name: "FK_CicloAuditoria_Estado_id_estado",
                        column: x => x.id_estado,
                        principalTable: "Estado",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CicloAuditoria_Legislacion_id_legislacion",
                        column: x => x.id_legislacion,
                        principalTable: "Legislacion",
                        principalColumn: "id_legislacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CicloAuditoria_Usuario_id_usuario_aprobador",
                        column: x => x.id_usuario_aprobador,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "SegmentoLegislacion",
                columns: table => new
                {
                    id_segmento_legislacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_legislacion = table.Column<int>(type: "int", nullable: false),
                    id_tipo_elemento = table.Column<int>(type: "int", nullable: false),
                    id_segmento_padre = table.Column<int>(type: "int", nullable: true),
                    contenido = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    observaciones = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orden = table.Column<int>(type: "int", nullable: false),
                    tituloSegmento = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contenido_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contenido_bin = table.Column<byte[]>(type: "longblob", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentoLegislacion", x => x.id_segmento_legislacion);
                    table.ForeignKey(
                        name: "FK_SegmentoLegislacion_Legislacion_id_legislacion",
                        column: x => x.id_legislacion,
                        principalTable: "Legislacion",
                        principalColumn: "id_legislacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SegmentoLegislacion_SegmentoLegislacion_id_segmento_padre",
                        column: x => x.id_segmento_padre,
                        principalTable: "SegmentoLegislacion",
                        principalColumn: "id_segmento_legislacion");
                    table.ForeignKey(
                        name: "FK_SegmentoLegislacion_TipoElemento_id_tipo_elemento",
                        column: x => x.id_tipo_elemento,
                        principalTable: "TipoElemento",
                        principalColumn: "id_tipo_elemento",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "CMMIRespuesta",
                columns: table => new
                {
                    id_respuesta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_evaluacion = table.Column<int>(type: "int", nullable: false),
                    id_ccmi_pregunta = table.Column<int>(type: "int", nullable: false),
                    valor = table.Column<byte>(type: "tinyint unsigned", precision: 5, scale: 2, nullable: false),
                    nota = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    evidencia_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CMMIRespuesta", x => x.id_respuesta);
                    table.ForeignKey(
                        name: "FK_CMMIRespuesta_CMMIEvaluacion_id_evaluacion",
                        column: x => x.id_evaluacion,
                        principalTable: "CMMIEvaluacion",
                        principalColumn: "id_evaluacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CMMIRespuesta_CMMIPregunta_id_ccmi_pregunta",
                        column: x => x.id_ccmi_pregunta,
                        principalTable: "CMMIPregunta",
                        principalColumn: "id_cmmi_pregunta",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "EvaluacionSegmento",
                columns: table => new
                {
                    id_evaluacion_segmento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_ciclo_auditoria = table.Column<int>(type: "int", nullable: false),
                    id_segmento_legislacion = table.Column<int>(type: "int", nullable: false),
                    id_usuario_auditor = table.Column<int>(type: "int", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false),
                    comentario = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_evaluacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluacionSegmento", x => x.id_evaluacion_segmento);
                    table.ForeignKey(
                        name: "FK_EvaluacionSegmento_CicloAuditoria_id_ciclo_auditoria",
                        column: x => x.id_ciclo_auditoria,
                        principalTable: "CicloAuditoria",
                        principalColumn: "id_ciclo_auditoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluacionSegmento_Estado_id_estado",
                        column: x => x.id_estado,
                        principalTable: "Estado",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluacionSegmento_SegmentoLegislacion_id_segmento_legislaci~",
                        column: x => x.id_segmento_legislacion,
                        principalTable: "SegmentoLegislacion",
                        principalColumn: "id_segmento_legislacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluacionSegmento_Usuario_id_usuario_auditor",
                        column: x => x.id_usuario_auditor,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Evidencia",
                columns: table => new
                {
                    id_evidencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_evaluacion_segmento = table.Column<int>(type: "int", nullable: false),
                    archivo_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre_original = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mime_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tamano_bytes = table.Column<int>(type: "int", nullable: true),
                    sha256 = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_documento = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    comentario_opcional = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_subida = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_actualizacion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    id_usuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evidencia", x => x.id_evidencia);
                    table.ForeignKey(
                        name: "FK_Evidencia_EvaluacionSegmento_id_evaluacion_segmento",
                        column: x => x.id_evaluacion_segmento,
                        principalTable: "EvaluacionSegmento",
                        principalColumn: "id_evaluacion_segmento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evidencia_Usuario_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuario",
                        principalColumn: "id_usuario");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_CCMICategoria_nombre",
                table: "CCMICategoria",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CicloAuditoria_id_estado",
                table: "CicloAuditoria",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_CicloAuditoria_id_legislacion",
                table: "CicloAuditoria",
                column: "id_legislacion");

            migrationBuilder.CreateIndex(
                name: "IX_CicloAuditoria_id_usuario_aprobador",
                table: "CicloAuditoria",
                column: "id_usuario_aprobador");

            migrationBuilder.CreateIndex(
                name: "IX_CMMIEvaluacion_id_empresa",
                table: "CMMIEvaluacion",
                column: "id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_CMMIEvaluacion_id_estado",
                table: "CMMIEvaluacion",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_CMMIEvaluacion_id_usuario_auditor",
                table: "CMMIEvaluacion",
                column: "id_usuario_auditor");

            migrationBuilder.CreateIndex(
                name: "IX_CMMIPregunta_id_ccmi_categoria",
                table: "CMMIPregunta",
                column: "id_ccmi_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_CMMIRespuesta_id_ccmi_pregunta",
                table: "CMMIRespuesta",
                column: "id_ccmi_pregunta");

            migrationBuilder.CreateIndex(
                name: "IX_CMMIRespuesta_id_evaluacion",
                table: "CMMIRespuesta",
                column: "id_evaluacion");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_id_estado",
                table: "Empresa",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_id_pais",
                table: "Empresa",
                column: "id_pais");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_id_sector",
                table: "Empresa",
                column: "id_sector");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionSegmento_id_ciclo_auditoria",
                table: "EvaluacionSegmento",
                column: "id_ciclo_auditoria");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionSegmento_id_estado",
                table: "EvaluacionSegmento",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionSegmento_id_segmento_legislacion",
                table: "EvaluacionSegmento",
                column: "id_segmento_legislacion");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionSegmento_id_usuario_auditor",
                table: "EvaluacionSegmento",
                column: "id_usuario_auditor");

            migrationBuilder.CreateIndex(
                name: "IX_Evidencia_id_evaluacion_segmento",
                table: "Evidencia",
                column: "id_evaluacion_segmento");

            migrationBuilder.CreateIndex(
                name: "IX_Evidencia_id_usuario",
                table: "Evidencia",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Legislacion_id_ambito_aplicacion",
                table: "Legislacion",
                column: "id_ambito_aplicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Legislacion_id_empresa",
                table: "Legislacion",
                column: "id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_Legislacion_id_estado",
                table: "Legislacion",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Legislacion_id_pais",
                table: "Legislacion",
                column: "id_pais");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentoLegislacion_id_legislacion",
                table: "SegmentoLegislacion",
                column: "id_legislacion");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentoLegislacion_id_segmento_padre",
                table: "SegmentoLegislacion",
                column: "id_segmento_padre");

            migrationBuilder.CreateIndex(
                name: "IX_SegmentoLegislacion_id_tipo_elemento",
                table: "SegmentoLegislacion",
                column: "id_tipo_elemento");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_id_estado",
                table: "Usuario",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_id_pais",
                table: "Usuario",
                column: "id_pais");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_id_rol",
                table: "Usuario",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEmpresa_id_empresa",
                table: "UsuarioEmpresa",
                column: "id_empresa");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEmpresa_id_rol",
                table: "UsuarioEmpresa",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEmpresa_id_usuario",
                table: "UsuarioEmpresa",
                column: "id_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CMMIRespuesta");

            migrationBuilder.DropTable(
                name: "Evidencia");

            migrationBuilder.DropTable(
                name: "UsuarioEmpresa");

            migrationBuilder.DropTable(
                name: "CMMIPregunta");

            migrationBuilder.DropTable(
                name: "EvaluacionSegmento");

            migrationBuilder.DropTable(
                name: "CCMICategoria");

            migrationBuilder.DropTable(
                name: "CMMIEvaluacion");

            migrationBuilder.DropTable(
                name: "CicloAuditoria");

            migrationBuilder.DropTable(
                name: "SegmentoLegislacion");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Legislacion");

            migrationBuilder.DropTable(
                name: "TipoElemento");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "AmbitoAplicacion");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropTable(
                name: "Estado");

            migrationBuilder.DropTable(
                name: "Pais");

            migrationBuilder.DropTable(
                name: "Sector");
        }
    }
}
