using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarInsuranceManage.Migrations
{
    /// <inheritdoc />
    public partial class CarinsuranceDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "insurance_services",
                columns: table => new
                {
                    service_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    service_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    insurance_info_id = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    image_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance_services", x => x.service_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    full_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_logs = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    full_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.customer_id);
                    table.ForeignKey(
                        name: "FK_customers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "claims",
                columns: table => new
                {
                    claim_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customer_id = table.Column<int>(type: "int", nullable: true),
                    customer_full_name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    customer_email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    customer_phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claims", x => x.claim_id);
                    table.ForeignKey(
                        name: "FK_claims_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    comment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    parent_comment_id = table.Column<int>(type: "int", nullable: true),
                    comment_text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rating = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_comments_comments_parent_comment_id",
                        column: x => x.parent_comment_id,
                        principalTable: "comments",
                        principalColumn: "comment_id");
                    table.ForeignKey(
                        name: "FK_comments_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    full_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    subject = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    message = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_added = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_modified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contacts", x => x.id);
                    table.ForeignKey(
                        name: "FK_contacts_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customer_support_requests",
                columns: table => new
                {
                    support_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    support_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    support_description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    support_payment = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    support_status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    resolved_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    resolved_by = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_support_requests", x => x.support_id);
                    table.ForeignKey(
                        name: "FK_customer_support_requests_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customer_support_requests_users_resolved_by",
                        column: x => x.resolved_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "histories",
                columns: table => new
                {
                    history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customer_id = table.Column<int>(type: "int", nullable: true),
                    username = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    car_brand = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vehicle_line = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    year_of_manufacture = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    registration_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    number_plate = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    frame_number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    machine_number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    insurance_start_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    insurance_package = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    insurance_code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    insurance_price = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    insurance_end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_histories", x => x.history_id);
                    table.ForeignKey(
                        name: "FK_histories_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "insurances_info",
                columns: table => new
                {
                    insurance_info_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customer_id = table.Column<int>(type: "int", nullable: true),
                    username = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    car_brand = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vehicle_line = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    year_of_manufacture = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    registration_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    number_plate = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    frame_number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    machine_number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    insurance_start_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    insurance_package = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    insurance_code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    insurance_price = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    insurance_end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurances_info", x => x.insurance_info_id);
                    table.ForeignKey(
                        name: "FK_insurances_info_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    message_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    message_content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sent_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_read = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.notification_id);
                    table.ForeignKey(
                        name: "FK_notifications_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    insurance_info_id = table.Column<int>(type: "int", nullable: true),
                    bill_number = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    payment_amount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    transaction_id = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_payments_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_insurances_info_insurance_info_id",
                        column: x => x.insurance_info_id,
                        principalTable: "insurances_info",
                        principalColumn: "insurance_info_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "insurance_services",
                columns: new[] { "service_id", "created_at", "description", "image_url", "insurance_info_id", "is_active", "price", "service_name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(380), "Basic vehicle insurance", "customer-assets/uploads/product/moto.jpg", 1, true, 50.00m, "Moto Insurance", null },
                    { 2, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(384), "Premium vehicle insurance with more benefits", "customer-assets/uploads/product/car.jpg", 2, true, 50.00m, "Car Insurance", null },
                    { 3, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(386), "Comprehensive coverage for all types of damage", "customer-assets/uploads/product/truck.jpg", 3, true, 50.00m, "Truck Insurance", null }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "address", "avatar", "created_at", "email", "full_name", "password", "phone_number", "role", "user_logs", "username" },
                values: new object[,]
                {
                    { 1, "123 Admin Street", "admin_avatar.jpg", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(48), "admin@gmail.com", "Admin User", "admin123", "1234567890", "admin", "", "admin" },
                    { 2, "123 User Street", "user1_avatar.jpg", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(51), "vunnth2307024@gmail.com", "User One", "user123", "1234567891", "customer", "", "user1" },
                    { 3, "123 Another Street", "user2_avatar.jpg", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(53), "user2@example.com", "User Two", "user456", "1234567892", "customer", "", "user2" }
                });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "customer_id", "address", "full_name", "phone_number", "user_id" },
                values: new object[,]
                {
                    { 1, "123 Admin Street", "Admin User", "1234567890", 1 },
                    { 2, "123 User Street", "User One", "1234567891", 2 },
                    { 3, "123 Another Street", "User Two", "1234567892", 3 }
                });

            migrationBuilder.InsertData(
                table: "claims",
                columns: new[] { "claim_id", "created_at", "customer_email", "customer_full_name", "customer_id", "customer_phone", "description", "status" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(240), "admin@example.com", "Admin User", 1, "1234567890", "Claim for accident", 0 },
                    { 2, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(244), "user1@example.com", "User One", 2, "1234567891", "Claim for windshield damage", 2 },
                    { 3, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(246), "user2@example.com", "User Two", 3, "1234567892", "Claim for theft", 1 }
                });

            migrationBuilder.InsertData(
                table: "comments",
                columns: new[] { "comment_id", "comment_text", "created_at", "customer_id", "parent_comment_id", "rating", "status" },
                values: new object[,]
                {
                    { 1, "Great service!", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(405), 1, null, 5, "Active" },
                    { 3, "Service was okay.", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(411), 3, null, 3, "Active" }
                });

            migrationBuilder.InsertData(
                table: "contacts",
                columns: new[] { "id", "customer_id", "date_added", "date_modified", "email", "full_name", "message", "phone", "status", "subject" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(350), new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(350), "admin@example.com", "Admin User", "Can I upgrade my policy?", "1234567890", true, "Policy Details" },
                    { 2, 2, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(355), new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(355), "user1@example.com", "User One", "I need help with my claim.", "1234567891", true, "Claim Issue" },
                    { 3, 3, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(357), new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(357), "user2@example.com", "User Two", "I have a question about your services.", "1234567892", false, "General Inquiry" }
                });

            migrationBuilder.InsertData(
                table: "customer_support_requests",
                columns: new[] { "support_id", "created_at", "customer_id", "resolved_at", "resolved_by", "support_description", "support_payment", "support_status", "support_type" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(271), 1, null, null, "Help with insurance details.", "Free", "Pending", "General Inquiry" },
                    { 2, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(271), 2, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(273), 1, "Issue with a claim.", "Paid", "Resolved", "Claim Support" },
                    { 3, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(275), 3, null, null, "Renewal question.", "Free", "Closed", "Policy Inquiry" }
                });

            migrationBuilder.InsertData(
                table: "histories",
                columns: new[] { "history_id", "car_brand", "created_at", "customer_id", "email", "frame_number", "insurance_code", "insurance_end_date", "insurance_package", "insurance_price", "insurance_start_date", "machine_number", "number_plate", "phone", "registration_date", "username", "vehicle_line", "year_of_manufacture" },
                values: new object[,]
                {
                    { 1, "Toyota", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(438), 1, "admin@example.com", "FRAME001", "INS001", new DateTime(2025, 6, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(436), "Basic Plan", 500.00m, new DateTime(2024, 6, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(435), "MACHINE001", "ABC123", "1234567890", new DateTime(2024, 6, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(432), "admin", "Corolla", "2020" },
                    { 2, "Honda", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(444), 2, "user1@example.com", "FRAME002", "INS002", new DateTime(2024, 6, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(443), "Comprehensive Plan", 700.00m, new DateTime(2023, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(442), "MACHINE002", "XYZ456", "1234567891", new DateTime(2023, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(441), "user1", "Civic", "2019" },
                    { 3, "Ford", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(450), 3, "user2@example.com", "FRAME003", "INS003", new DateTime(2025, 9, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(449), "Premium Plan", 900.00m, new DateTime(2024, 9, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(448), "MACHINE003", "DEF789", "1234567892", new DateTime(2024, 9, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(447), "user2", "Focus", "2021" }
                });

            migrationBuilder.InsertData(
                table: "insurances_info",
                columns: new[] { "insurance_info_id", "car_brand", "created_at", "customer_id", "email", "frame_number", "insurance_code", "insurance_end_date", "insurance_package", "insurance_price", "insurance_start_date", "machine_number", "number_plate", "phone", "registration_date", "username", "vehicle_line", "year_of_manufacture" },
                values: new object[,]
                {
                    { 1, "Toyota", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(472), 1, "admin@example.com", "FRAME001", null, new DateTime(2025, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(470), "Basic Plan", 500.00m, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(470), "MACHINE001", "ABC123", "1234567890", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(469), "admin", "Corolla", "2020" },
                    { 2, "Honda", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(479), 2, "user1@example.com", "FRAME002", null, new DateTime(2025, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(477), "Comprehensive Plan", 700.00m, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(477), "MACHINE002", "XYZ456", "1234567891", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(476), "user1", "Civic", "2019" },
                    { 3, "Ford", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(483), 3, "user2@example.com", "FRAME003", null, new DateTime(2025, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(482), "Premium Plan", 900.00m, new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(482), "MACHINE003", "DEF789", "1234567892", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(481), "user2", "Focus", "2021" }
                });

            migrationBuilder.InsertData(
                table: "notifications",
                columns: new[] { "notification_id", "customer_id", "is_read", "message_content", "message_type", "sent_at" },
                values: new object[,]
                {
                    { 1, 1, false, "Policy renewal reminder.", "Reminder", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(296) },
                    { 2, 2, true, "Your claim has been processed.", "Claim Update", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(300) },
                    { 3, 3, false, "Special discounts for renewals!", "Promotion", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(302) }
                });

            migrationBuilder.InsertData(
                table: "comments",
                columns: new[] { "comment_id", "comment_text", "created_at", "customer_id", "parent_comment_id", "rating", "status" },
                values: new object[] { 2, "I agree, excellent support.", new DateTime(2024, 12, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(409), 2, 1, 4, "Active" });

            migrationBuilder.InsertData(
                table: "payments",
                columns: new[] { "payment_id", "bill_number", "customer_id", "insurance_info_id", "payment_amount", "payment_date", "payment_status", "transaction_id" },
                values: new object[,]
                {
                    { 1, "BILL001", 1, 1, 100.00m, new DateTime(2024, 11, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(196), "Completed", "TXN001" },
                    { 2, "BILL002", 2, 2, 200.00m, new DateTime(2024, 10, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(210), "Pending", "TXN002" },
                    { 3, "BILL003", 3, 3, 300.00m, new DateTime(2024, 9, 22, 19, 6, 21, 859, DateTimeKind.Local).AddTicks(213), "Failed", "TXN003" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_claims_customer_id",
                table: "claims",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_customer_id",
                table: "comments",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_comments_parent_comment_id",
                table: "comments",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_contacts_customer_id",
                table: "contacts",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_support_requests_customer_id",
                table: "customer_support_requests",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_customer_support_requests_resolved_by",
                table: "customer_support_requests",
                column: "resolved_by");

            migrationBuilder.CreateIndex(
                name: "IX_customers_user_id",
                table: "customers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_histories_customer_id",
                table: "histories",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurances_info_customer_id",
                table: "insurances_info",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_customer_id",
                table: "notifications",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_customer_id",
                table: "payments",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_insurance_info_id",
                table: "payments",
                column: "insurance_info_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "claims");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "customer_support_requests");

            migrationBuilder.DropTable(
                name: "histories");

            migrationBuilder.DropTable(
                name: "insurance_services");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "insurances_info");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
