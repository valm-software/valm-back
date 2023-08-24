using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Marca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marca", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "ProdProductos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci"),
                    Descripcion = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, collation: "utf8mb4_general_ci"),
                    ValorCosto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorVenta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorCuota = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumCuotas = table.Column<int>(type: "int", nullable: false),
                    ConfActivo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConfPorUtilidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(0)", precision: 0, nullable: false, defaultValueSql: "(CURRENT_TIMESTAMP)"),
                    IdMarca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdProductos", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "UsrPrivilegios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci"),
                    Descripcion = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, collation: "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsrPrivilegios", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "UsrRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci"),
                    Descripcion = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true, collation: "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsrRoles", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "UsrUsuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Usuario = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci"),
                    Contraseña = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsrUsuarios", x => x.Id);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, collation: "utf8mb4_general_ci"),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MarcaId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producto_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Producto_Marca_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marca",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "UsrRolesPrivilegios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    IdPrivilegio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsrRolesPrivilegios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsrRolesPrivilegios_UsrPrivilegios_IdPrivilegio",
                        column: x => x.IdPrivilegio,
                        principalTable: "UsrPrivilegios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsrRolesPrivilegios_UsrRoles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "UsrRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateTable(
                name: "UsrUsuariosRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsrUsuariosRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsrUsuariosRoles_UsrRoles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "UsrRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsrUsuariosRoles_UsrUsuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "UsrUsuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CategoriaId",
                table: "Producto",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_MarcaId",
                table: "Producto",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsrRolesPrivilegios_IdPrivilegio",
                table: "UsrRolesPrivilegios",
                column: "IdPrivilegio");

            migrationBuilder.CreateIndex(
                name: "IX_UsrRolesPrivilegios_IdRol",
                table: "UsrRolesPrivilegios",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_UsrUsuariosRoles_IdRol",
                table: "UsrUsuariosRoles",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_UsrUsuariosRoles_IdUsuario",
                table: "UsrUsuariosRoles",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdProductos");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "UsrRolesPrivilegios");

            migrationBuilder.DropTable(
                name: "UsrUsuariosRoles");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Marca");

            migrationBuilder.DropTable(
                name: "UsrPrivilegios");

            migrationBuilder.DropTable(
                name: "UsrRoles");

            migrationBuilder.DropTable(
                name: "UsrUsuarios");
        }
    }
}
