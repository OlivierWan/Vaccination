using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vaccination.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarVaccinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    MonthAge = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthDelay = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarVaccinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Nationality = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    SocialSecurityNumber = table.Column<string>(type: "TEXT", maxLength: 13, nullable: true),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVaccinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    VaccineCalendarId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VaccinationDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedOnUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVaccinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVaccinations_CalendarVaccinations_VaccineCalendarId",
                        column: x => x.VaccineCalendarId,
                        principalTable: "CalendarVaccinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserVaccinations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "143a221a-2256-4130-a4b9-90afc20abc98", "e2bd239a-ef11-4436-abc3-88a93d04401e", "ADMIN", "ADMIN" },
                    { "5baa1ca4-327b-4da1-8d95-f578c71d4a3d", "f3e5de56-d158-4e0a-afbd-083e450b87ad", "READ", "READ" },
                    { "8a362b86-e96f-4f83-95ff-1cc743c332d5", "028d602d-b6ed-49fd-a063-0a3e5ab7c202", "WRITE", "WRITE" },
                    { "9208a9e6-51fb-4f97-8ade-de0edf8a1da6", "e8e00f24-236c-4f6a-af70-eae3b67a5ad9", "USER", "USER" },
                    { "93678cd4-0a06-4d1e-a42b-61454d92fbac", "5cecb024-da7d-45a7-82d6-0b2463843362", "OWNER", "OWNER" },
                    { "e6a619e2-21e6-4db2-998c-89d20ceef4c2", "c92766b2-31c0-4da6-9afc-330a90b73cb1", "DELETE", "DELETE" }
                });

            migrationBuilder.InsertData(
                table: "CalendarVaccinations",
                columns: new[] { "Id", "CreatedBy", "CreatedOnUtc", "Description", "IsDeleted", "ModifiedBy", "ModifiedOnUtc", "MonthAge", "MonthDelay", "Name" },
                values: new object[,]
                {
                    { new Guid("0875b475-c6b2-439b-9c26-1417532993ce"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 10, 1, 15, 25, 28, 345, DateTimeKind.Utc).AddTicks(7965), "vaccination contre la gastro-entérite à\r\nrotavirus.\r\nMéningocoque B (1ère dose) : vaccination contre les infections\r\ninvasives à méningocoque B.", false, null, null, 3, 0, "Rotavirus (2ème dose)" },
                    { new Guid("237fc3a7-b764-41fc-a7c9-7e8f17a06205"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 10, 1, 15, 25, 28, 345, DateTimeKind.Utc).AddTicks(7963), "protège\r\ncontre :\r\n- la diphtérie,\r\n- le tétanos,\r\n- la coqueluche,\r\n- les infections invasives à Haemophilus Influenzae de type b\r\n(méningite, épiglottite et arthrite),\r\n- la poliomyélite,\r\n- l’hépatite B.\r\nRotavirus (1ère dose) : vaccination contre la gastro-entérite à rotavirus.\r\nPneumocoques (1ère dose) : vaccination contre les infections\r\ninvasives à pneumocoques.", false, null, null, 2, 0, "1ère dose du vaccin combiné (D, T, aP, Hib, IPV, Hep B)" },
                    { new Guid("3fde4331-77a3-4e9b-b5c6-f3e60d432505"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 10, 1, 15, 25, 28, 345, DateTimeKind.Utc).AddTicks(7970), "protège contre :\r\n- la diphtérie,\r\n- le tétanos,\r\n- la coqueluche,\r\n- les infections invasives à Haemophilus Influenzae de type b\r\n(méningite, épiglottite et arthrite),\r\n- la poliomyélite,\r\n- l’hépatite B.\r\nPneumocoques (3ème dose) : vaccination contre les infections\r\ninvasives à pneumocoques.", false, null, null, 11, 0, "3ème dose du vaccin combiné (D, T, aP, Hib, IPV, Hep B)" },
                    { new Guid("58f86995-e641-4f8c-8a1c-44fa05b8c878"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 10, 1, 15, 25, 28, 345, DateTimeKind.Utc).AddTicks(7974), "protège contre :\r\n- la rougeole,\r\n- les oreillons,\r\n- la rubéole,\r\n- la varicelle.\r\nMéningocoque B (3ème dose) : vaccination contre les infections\r\ninvasives à méningocoque B.", false, null, null, 12, 0, "1ère dose du vaccin combiné (RORV)" },
                    { new Guid("67d97ad7-b78e-410b-9eb2-c55afb637b0d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 10, 1, 15, 25, 28, 345, DateTimeKind.Utc).AddTicks(7957), "administration d’un traitement\r\npréventif (produit d’immunisation passive) qui protège contre la\r\nbronchiolite, de préférence avant la sortie de la maternité, en période\r\nde haute circulation du virus, de septembre à février.", false, null, null, 0, 6, "RSV (virus respiratoire syncytial)" },
                    { new Guid("722370b2-0761-4d7c-85d1-1f8862eb4c13"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 10, 1, 15, 25, 28, 345, DateTimeKind.Utc).AddTicks(7967), "2ème dose du vaccin combiné (D, T, aP, Hib, IPV, Hep B) qui\r\nprotège contre :\r\n- la diphtérie,\r\n- le tétanos,\r\n- la coqueluche,\r\n- les infections invasives à Haemophilus Influenzae de type b\r\n(méningite, épiglottite et arthrite),\r\n- la poliomyélite,\r\n- l’hépatite B.\r\nPneumocoques (2ème dose) : vaccination contre les infections\r\ninvasives à pneumocoques.\r\nRotavirus (3ème dose) : vaccination contre la gastro-entérite à\r\nrotavirus.", false, null, null, 4, 0, "2ème dose du vaccin combiné (D, T, aP, Hib, IPV, Hep B)" },
                    { new Guid("bb298758-3b3a-4e84-8c7a-5073028fd6a5"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 10, 1, 15, 25, 28, 345, DateTimeKind.Utc).AddTicks(7969), "vaccination contre les infections\r\ninvasives à méningocoque B.", false, null, null, 5, 0, "Méningocoque B (2ème dose)" },
                    { new Guid("e6353ef3-c358-47e8-b51b-52b1af9b99b4"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 10, 1, 15, 25, 28, 345, DateTimeKind.Utc).AddTicks(7976), "vaccination contre les infections\r\ninvasives à méningocoques A, C, W et Y", false, null, null, 13, 0, "Méningocoques ACWY (1ère dose)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserVaccinations_UserId",
                table: "UserVaccinations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVaccinations_VaccineCalendarId",
                table: "UserVaccinations",
                column: "VaccineCalendarId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "UserVaccinations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CalendarVaccinations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
