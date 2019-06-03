using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertiesX.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pair",
                columns: table => new
                {
                    TranslationsId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LanguageTag = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pair", x => new { x.TranslationsId, x.Id });
                    table.ForeignKey(
                        name: "FK_Pair_Translations_TranslationsId",
                        column: x => x.TranslationsId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyVersion",
                columns: table => new
                {
                    Version = table.Column<string>(nullable: false),
                    Tenant = table.Column<string>(maxLength: 64, nullable: true),
                    Updated = table.Column<DateTime>(nullable: false),
                    NameId = table.Column<int>(nullable: false),
                    DescriptionId = table.Column<int>(nullable: true),
                    Category = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyVersion", x => x.Version);
                    table.ForeignKey(
                        name: "FK_PropertyVersion_Translations_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyVersion_Translations_NameId",
                        column: x => x.NameId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfo",
                columns: table => new
                {
                    PropertyVersionVersion = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    Address_CityName = table.Column<string>(maxLength: 100, nullable: true),
                    Address_PostalCode = table.Column<string>(maxLength: 50, nullable: true),
                    Address_CountryName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfo", x => new { x.PropertyVersionVersion, x.Id });
                    table.ForeignKey(
                        name: "FK_ContactInfo_PropertyVersion_PropertyVersionVersion",
                        column: x => x.PropertyVersionVersion,
                        principalTable: "PropertyVersion",
                        principalColumn: "Version",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageLink",
                columns: table => new
                {
                    PropertyVersionVersion = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SortValue = table.Column<int>(nullable: false),
                    Href = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageLink", x => new { x.PropertyVersionVersion, x.Id });
                    table.ForeignKey(
                        name: "FK_ImageLink_PropertyVersion_PropertyVersionVersion",
                        column: x => x.PropertyVersionVersion,
                        principalTable: "PropertyVersion",
                        principalColumn: "Version",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentVersion = table.Column<string>(nullable: false),
                    Tenant = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_PropertyVersion_CurrentVersion",
                        column: x => x.CurrentVersion,
                        principalTable: "PropertyVersion",
                        principalColumn: "Version",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameId = table.Column<int>(nullable: false),
                    DescriptionId = table.Column<int>(nullable: true),
                    PropertyVersionVersion = table.Column<string>(nullable: true),
                    RoomTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomTypes_Translations_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoomTypes_Translations_NameId",
                        column: x => x.NameId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomTypes_PropertyVersion_PropertyVersionVersion",
                        column: x => x.PropertyVersionVersion,
                        principalTable: "PropertyVersion",
                        principalColumn: "Version",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoomTypes_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressLine",
                columns: table => new
                {
                    AddressContactInfoPropertyVersionVersion = table.Column<string>(nullable: false),
                    AddressContactInfoId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: false),
                    LineNo = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressLine", x => new { x.AddressContactInfoPropertyVersionVersion, x.AddressContactInfoId, x.Id });
                    table.ForeignKey(
                        name: "FK_AddressLine_ContactInfo_AddressContactInfoPropertyVersionVersion_AddressContactInfoId",
                        columns: x => new { x.AddressContactInfoPropertyVersionVersion, x.AddressContactInfoId },
                        principalTable: "ContactInfo",
                        principalColumns: new[] { "PropertyVersionVersion", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneInfo",
                columns: table => new
                {
                    ContactInfoPropertyVersionVersion = table.Column<string>(nullable: false),
                    ContactInfoId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(maxLength: 20, nullable: false),
                    Number = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneInfo", x => new { x.ContactInfoPropertyVersionVersion, x.ContactInfoId, x.Id });
                    table.ForeignKey(
                        name: "FK_PhoneInfo_ContactInfo_ContactInfoPropertyVersionVersion_ContactInfoId",
                        columns: x => new { x.ContactInfoPropertyVersionVersion, x.ContactInfoId },
                        principalTable: "ContactInfo",
                        principalColumns: new[] { "PropertyVersionVersion", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageLink1",
                columns: table => new
                {
                    RoomTypeId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SortValue = table.Column<int>(nullable: false),
                    Href = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageLink1", x => new { x.RoomTypeId, x.Id });
                    table.ForeignKey(
                        name: "FK_ImageLink1_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtaAmenity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: true),
                    Value = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtaAmenity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtaAmenity_RoomTypes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameId = table.Column<int>(nullable: true),
                    DescriptionId = table.Column<int>(nullable: true),
                    RoomTypeId = table.Column<int>(nullable: false),
                    PropertyVersionVersion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Translations_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_Translations_NameId",
                        column: x => x.NameId,
                        principalTable: "Translations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_PropertyVersion_PropertyVersionVersion",
                        column: x => x.PropertyVersionVersion,
                        principalTable: "PropertyVersion",
                        principalColumn: "Version",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomTag",
                columns: table => new
                {
                    RoomTypeId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Tag = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTag", x => new { x.RoomTypeId, x.Id });
                    table.ForeignKey(
                        name: "FK_RoomTag_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtaAmenity_ParentId",
                table: "OtaAmenity",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_CurrentVersion",
                table: "Properties",
                column: "CurrentVersion");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyVersion_DescriptionId",
                table: "PropertyVersion",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyVersion_NameId",
                table: "PropertyVersion",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DescriptionId",
                table: "Rooms",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_NameId",
                table: "Rooms",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_PropertyVersionVersion",
                table: "Rooms",
                column: "PropertyVersionVersion");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Rooms",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_DescriptionId",
                table: "RoomTypes",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_NameId",
                table: "RoomTypes",
                column: "NameId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_PropertyVersionVersion",
                table: "RoomTypes",
                column: "PropertyVersionVersion");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_RoomTypeId",
                table: "RoomTypes",
                column: "RoomTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressLine");

            migrationBuilder.DropTable(
                name: "ImageLink");

            migrationBuilder.DropTable(
                name: "ImageLink1");

            migrationBuilder.DropTable(
                name: "OtaAmenity");

            migrationBuilder.DropTable(
                name: "Pair");

            migrationBuilder.DropTable(
                name: "PhoneInfo");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "RoomTag");

            migrationBuilder.DropTable(
                name: "ContactInfo");

            migrationBuilder.DropTable(
                name: "RoomTypes");

            migrationBuilder.DropTable(
                name: "PropertyVersion");

            migrationBuilder.DropTable(
                name: "Translations");
        }
    }
}
