using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;

namespace ChainCrims.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bonus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Descriacao = table.Column<string>(type: "text", nullable: true),
                    ValorBonus = table.Column<string>(type: "text", nullable: true),
                    DuracaoMax = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Carteira = table.Column<string>(type: "text", nullable: true),
                    HashSenha = table.Column<string>(type: "text", nullable: true),
                    Saldo = table.Column<double>(type: "double precision", nullable: true),
                    UltimoSaque = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Criacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UltimoLogin = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Banido = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roubos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Imagem = table.Column<string>(type: "text", nullable: true),
                    Descriacao = table.Column<string>(type: "text", nullable: true),
                    ChanceBase = table.Column<float>(type: "real", nullable: true),
                    PoderMin = table.Column<int>(type: "integer", nullable: true),
                    RecompensaMin = table.Column<float>(type: "real", nullable: true),
                    RecompensaMax = table.Column<float>(type: "real", nullable: true),
                    RespeitoMin = table.Column<float>(type: "real", nullable: true),
                    RespeitoMax = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roubos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuspeitoBonus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdBonus = table.Column<int>(type: "integer", nullable: true),
                    Duracao = table.Column<int>(type: "integer", nullable: true),
                    Criacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuspeitoBonus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tx = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: true),
                    Symbol = table.Column<string>(type: "text", nullable: true),
                    Receiver = table.Column<string>(type: "text", nullable: true),
                    Sender = table.Column<string>(type: "text", nullable: true),
                    BlockHeight = table.Column<long>(type: "bigint", nullable: true),
                    BlockTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Armas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdConta = table.Column<int>(type: "integer", nullable: true),
                    IdSuspeito = table.Column<int>(type: "integer", nullable: true),
                    IdBlockchain = table.Column<int>(type: "integer", nullable: true),
                    Imagem = table.Column<string>(type: "text", nullable: true),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Poder = table.Column<float>(type: "real", nullable: true),
                    Saude = table.Column<int>(type: "integer", nullable: true),
                    Banido = table.Column<bool>(type: "boolean", nullable: true),
                    Queimado = table.Column<bool>(type: "boolean", nullable: true),
                    Criacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Armas_Contas_IdConta",
                        column: x => x.IdConta,
                        principalTable: "Contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdConta = table.Column<int>(type: "integer", nullable: true),
                    Acao = table.Column<int>(type: "integer", nullable: true),
                    Criacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registros_Contas_IdConta",
                        column: x => x.IdConta,
                        principalTable: "Contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Suspeitos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdConta = table.Column<int>(type: "integer", nullable: true),
                    IdBlockchain = table.Column<int>(type: "integer", nullable: true),
                    IdArma = table.Column<int>(type: "integer", nullable: true),
                    Imagem = table.Column<string>(type: "text", nullable: true),
                    Nome = table.Column<string>(type: "text", nullable: true),
                    Titulo = table.Column<string>(type: "text", nullable: true),
                    Estamina = table.Column<int>(type: "integer", nullable: true),
                    Respeito = table.Column<double>(type: "double precision", nullable: true),
                    Nivel = table.Column<int>(type: "integer", nullable: true)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Poder = table.Column<float>(type: "real", nullable: true),
                    Saude = table.Column<float>(type: "real", nullable: true),
                    Habilidades = table.Column<List<int>>(type: "integer[]", nullable: true),
                    Banido = table.Column<bool>(type: "boolean", nullable: true),
                    Queimado = table.Column<bool>(type: "boolean", nullable: true),
                    Criacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UltimaLuta = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ProximaRecarga = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suspeitos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suspeitos_Armas_IdArma",
                        column: x => x.IdArma,
                        principalTable: "Armas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suspeitos_Contas_IdConta",
                        column: x => x.IdConta,
                        principalTable: "Contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistroArmas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdArma = table.Column<int>(type: "integer", nullable: true),
                    IdContaPara = table.Column<int>(type: "integer", nullable: true),
                    RegistroId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroArmas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroArmas_Registros_RegistroId",
                        column: x => x.RegistroId,
                        principalTable: "Registros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistroSuspeitos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdSuspeito = table.Column<int>(type: "integer", nullable: true),
                    IdContaPara = table.Column<int>(type: "integer", nullable: true),
                    RegistroId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroSuspeitos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroSuspeitos_Registros_RegistroId",
                        column: x => x.RegistroId,
                        principalTable: "Registros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoRoubos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdSuspeito = table.Column<int>(type: "integer", nullable: true),
                    IdRoubo = table.Column<int>(type: "integer", nullable: true),
                    Resultado = table.Column<string>(type: "text", nullable: true),
                    Respeito = table.Column<double>(type: "double precision", nullable: true),
                    Saldo = table.Column<double>(type: "double precision", nullable: true),
                    RouboId = table.Column<int>(type: "integer", nullable: true),
                    SuspeitoId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoRoubos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadoRoubos_Roubos_RouboId",
                        column: x => x.RouboId,
                        principalTable: "Roubos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultadoRoubos_Suspeitos_SuspeitoId",
                        column: x => x.SuspeitoId,
                        principalTable: "Suspeitos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Armas_IdConta",
                table: "Armas",
                column: "IdConta");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroArmas_RegistroId",
                table: "RegistroArmas",
                column: "RegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Registros_IdConta",
                table: "Registros",
                column: "IdConta");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroSuspeitos_RegistroId",
                table: "RegistroSuspeitos",
                column: "RegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoRoubos_RouboId",
                table: "ResultadoRoubos",
                column: "RouboId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoRoubos_SuspeitoId",
                table: "ResultadoRoubos",
                column: "SuspeitoId");

            migrationBuilder.CreateIndex(
                name: "IX_Suspeitos_IdArma",
                table: "Suspeitos",
                column: "IdArma",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suspeitos_IdConta",
                table: "Suspeitos",
                column: "IdConta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonus");

            migrationBuilder.DropTable(
                name: "RegistroArmas");

            migrationBuilder.DropTable(
                name: "RegistroSuspeitos");

            migrationBuilder.DropTable(
                name: "ResultadoRoubos");

            migrationBuilder.DropTable(
                name: "SuspeitoBonus");

            migrationBuilder.DropTable(
                name: "Transacoes");

            migrationBuilder.DropTable(
                name: "Registros");

            migrationBuilder.DropTable(
                name: "Roubos");

            migrationBuilder.DropTable(
                name: "Suspeitos");

            migrationBuilder.DropTable(
                name: "Armas");

            migrationBuilder.DropTable(
                name: "Contas");
        }
    }
}
