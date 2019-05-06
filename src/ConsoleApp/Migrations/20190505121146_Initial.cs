using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bpm_byte_array",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    value = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_byte_array", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bpm_group",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bpm_user",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bpm_package",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tenant_id = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    owner_id = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    version = table.Column<int>(nullable: false),
                    concurrency_stamp = table.Column<string>(nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    modified = table.Column<DateTime>(nullable: false),
                    source_id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_package", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_package_bpm_byte_array_source_id",
                        column: x => x.source_id,
                        principalTable: "bpm_byte_array",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bpm_user_group",
                columns: table => new
                {
                    user_id = table.Column<string>(nullable: false),
                    group_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_user_group", x => new { x.user_id, x.group_id });
                    table.ForeignKey(
                        name: "FK_bpm_user_group_bpm_group_group_id",
                        column: x => x.group_id,
                        principalTable: "bpm_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bpm_user_group_bpm_user_user_id",
                        column: x => x.user_id,
                        principalTable: "bpm_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bpm_deployment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 50, nullable: true),
                    tenant_id = table.Column<string>(nullable: true),
                    category = table.Column<string>(maxLength: 64, nullable: true),
                    model_id = table.Column<long>(nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<string>(maxLength: 32, nullable: true),
                    package_id = table.Column<int>(nullable: true),
                    memo = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_deployment", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_deployment_bpm_byte_array_model_id",
                        column: x => x.model_id,
                        principalTable: "bpm_byte_array",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bpm_deployment_bpm_package_package_id",
                        column: x => x.package_id,
                        principalTable: "bpm_package",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bpm_proc_def",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tenant_id = table.Column<string>(maxLength: 32, nullable: true),
                    category = table.Column<string>(maxLength: 50, nullable: true),
                    deployment_id = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    key = table.Column<string>(maxLength: 64, nullable: false),
                    version = table.Column<int>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    modified = table.Column<DateTime>(nullable: false),
                    has_diagram = table.Column<bool>(nullable: false),
                    valid_from = table.Column<DateTime>(nullable: true),
                    valid_to = table.Column<DateTime>(nullable: true),
                    concurrency_stamp = table.Column<string>(nullable: true),
                    state = table.Column<int>(nullable: false),
                    version_tag = table.Column<string>(maxLength: 255, nullable: true),
                    description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_proc_def", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_proc_def_bpm_deployment_deployment_id",
                        column: x => x.deployment_id,
                        principalTable: "bpm_deployment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bpm_act_data",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 64, nullable: false),
                    type = table.Column<string>(maxLength: 128, nullable: false),
                    byte_array_id = table.Column<long>(nullable: true),
                    text = table.Column<string>(nullable: true),
                    text2 = table.Column<string>(nullable: true),
                    long_val = table.Column<long>(nullable: true),
                    double_val = table.Column<double>(nullable: true),
                    act_inst_id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_act_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_act_data_bpm_byte_array_byte_array_id",
                        column: x => x.byte_array_id,
                        principalTable: "bpm_byte_array",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bpm_identity_link",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(nullable: true),
                    group_id = table.Column<string>(nullable: true),
                    type = table.Column<string>(maxLength: 50, nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    proc_def_id = table.Column<int>(nullable: true),
                    proc_inst_id = table.Column<long>(nullable: true),
                    task_id = table.Column<long>(nullable: true),
                    act_inst_id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_identity_link", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_identity_link_bpm_proc_def_proc_def_id",
                        column: x => x.proc_def_id,
                        principalTable: "bpm_proc_def",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bpm_proc_inst",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    state = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    start_time = table.Column<DateTime>(nullable: true),
                    last_state_time = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(maxLength: 255, nullable: true),
                    concurrency_stamp = table.Column<string>(nullable: true),
                    tenant_id = table.Column<string>(nullable: true),
                    key = table.Column<string>(maxLength: 32, nullable: true),
                    initiator = table.Column<string>(maxLength: 32, nullable: true),
                    end_reason = table.Column<string>(maxLength: 255, nullable: true),
                    super_id = table.Column<long>(nullable: true),
                    proc_def_id = table.Column<int>(nullable: false),
                    caller_id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_proc_inst", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_proc_inst_bpm_proc_def_proc_def_id",
                        column: x => x.proc_def_id,
                        principalTable: "bpm_proc_def",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bpm_act_inst",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    state = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    start_time = table.Column<DateTime>(nullable: true),
                    last_state_time = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    concurrency_stamp = table.Column<string>(maxLength: 50, nullable: true),
                    proc_inst_id = table.Column<long>(nullable: true),
                    parent_id = table.Column<long>(nullable: true),
                    sub_proc_inst_id = table.Column<long>(nullable: true),
                    activity_id = table.Column<string>(maxLength: 64, nullable: false),
                    activity_type = table.Column<string>(maxLength: 16, nullable: false),
                    is_mi_root = table.Column<bool>(nullable: false),
                    token_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_act_inst", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_act_inst_bpm_act_inst_parent_id",
                        column: x => x.parent_id,
                        principalTable: "bpm_act_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bpm_act_inst_bpm_proc_inst_proc_inst_id",
                        column: x => x.proc_inst_id,
                        principalTable: "bpm_proc_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bpm_act_inst_bpm_proc_inst_sub_proc_inst_id",
                        column: x => x.sub_proc_inst_id,
                        principalTable: "bpm_proc_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bpm_token",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    activity_id = table.Column<string>(maxLength: 64, nullable: true),
                    parent_id = table.Column<long>(nullable: true),
                    is_scope = table.Column<bool>(nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    act_inst_id = table.Column<long>(nullable: true),
                    proc_inst_id = table.Column<long>(nullable: false),
                    sub_proc_inst_id = table.Column<long>(nullable: true),
                    transition_id = table.Column<string>(maxLength: 64, nullable: true),
                    is_suspended = table.Column<bool>(nullable: false),
                    is_mi_root = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_token", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_token_bpm_act_inst_act_inst_id",
                        column: x => x.act_inst_id,
                        principalTable: "bpm_act_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_bpm_token_bpm_token_parent_id",
                        column: x => x.parent_id,
                        principalTable: "bpm_token",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bpm_token_bpm_proc_inst_proc_inst_id",
                        column: x => x.proc_inst_id,
                        principalTable: "bpm_proc_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bpm_token_bpm_proc_inst_sub_proc_inst_id",
                        column: x => x.sub_proc_inst_id,
                        principalTable: "bpm_proc_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bpm_event_subscr",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    event_type = table.Column<string>(maxLength: 50, nullable: false),
                    event_name = table.Column<string>(maxLength: 50, nullable: false),
                    activity_id = table.Column<string>(nullable: true),
                    proc_def_id = table.Column<int>(nullable: true),
                    proc_inst_id = table.Column<long>(nullable: true),
                    token_id = table.Column<long>(nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    tenant_id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_event_subscr", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_event_subscr_bpm_proc_def_proc_def_id",
                        column: x => x.proc_def_id,
                        principalTable: "bpm_proc_def",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bpm_event_subscr_bpm_proc_inst_proc_inst_id",
                        column: x => x.proc_inst_id,
                        principalTable: "bpm_proc_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bpm_event_subscr_bpm_token_token_id",
                        column: x => x.token_id,
                        principalTable: "bpm_token",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bpm_scheduled_job",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    key = table.Column<string>(nullable: true),
                    retries = table.Column<int>(nullable: false),
                    type = table.Column<string>(maxLength: 50, nullable: false),
                    handler = table.Column<string>(nullable: true),
                    due_date = table.Column<DateTime>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: true),
                    proc_def_id = table.Column<int>(nullable: true),
                    activity_id = table.Column<string>(nullable: true),
                    proc_inst_id = table.Column<long>(nullable: true),
                    message = table.Column<string>(nullable: true),
                    stack_trace = table.Column<string>(nullable: true),
                    token_id = table.Column<long>(nullable: true),
                    tenant_id = table.Column<string>(nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    options = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_scheduled_job", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_scheduled_job_bpm_proc_def_proc_def_id",
                        column: x => x.proc_def_id,
                        principalTable: "bpm_proc_def",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bpm_scheduled_job_bpm_proc_inst_proc_inst_id",
                        column: x => x.proc_inst_id,
                        principalTable: "bpm_proc_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bpm_scheduled_job_bpm_token_token_id",
                        column: x => x.token_id,
                        principalTable: "bpm_token",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bpm_task",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    proc_inst_id = table.Column<long>(nullable: true),
                    act_inst_id = table.Column<long>(nullable: true),
                    state = table.Column<int>(nullable: false),
                    last_state_time = table.Column<DateTime>(nullable: false),
                    token_id = table.Column<long>(nullable: true),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    priority = table.Column<short>(nullable: false),
                    activity_id = table.Column<string>(maxLength: 64, nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    claimed_time = table.Column<DateTime>(nullable: true),
                    assignee = table.Column<string>(maxLength: 32, nullable: true),
                    due_date = table.Column<DateTime>(nullable: true),
                    modified = table.Column<DateTime>(nullable: false),
                    concurrency_stamp = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_task", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_task_bpm_act_inst_act_inst_id",
                        column: x => x.act_inst_id,
                        principalTable: "bpm_act_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bpm_task_bpm_proc_inst_proc_inst_id",
                        column: x => x.proc_inst_id,
                        principalTable: "bpm_proc_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bpm_task_bpm_token_token_id",
                        column: x => x.token_id,
                        principalTable: "bpm_token",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "bpm_comment",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<string>(maxLength: 32, nullable: true),
                    created = table.Column<DateTime>(nullable: false),
                    body = table.Column<string>(maxLength: 512, nullable: false),
                    proc_def_id = table.Column<int>(nullable: true),
                    proc_inst_id = table.Column<long>(nullable: true),
                    task_id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_comment", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_comment_bpm_proc_def_proc_def_id",
                        column: x => x.proc_def_id,
                        principalTable: "bpm_proc_def",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bpm_comment_bpm_proc_inst_proc_inst_id",
                        column: x => x.proc_inst_id,
                        principalTable: "bpm_proc_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bpm_comment_bpm_task_task_id",
                        column: x => x.task_id,
                        principalTable: "bpm_task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bpm_proc_data",
                columns: table => new
                {
                    proc_inst_id = table.Column<long>(nullable: true),
                    id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 64, nullable: false),
                    type = table.Column<string>(maxLength: 128, nullable: false),
                    byte_array_id = table.Column<long>(nullable: true),
                    text = table.Column<string>(maxLength: 4000, nullable: true),
                    text2 = table.Column<string>(maxLength: 4000, nullable: true),
                    long_val = table.Column<long>(nullable: true),
                    double_val = table.Column<double>(nullable: true),
                    task_id = table.Column<long>(nullable: true),
                    token_id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bpm_proc_data", x => x.id);
                    table.ForeignKey(
                        name: "FK_bpm_proc_data_bpm_byte_array_byte_array_id",
                        column: x => x.byte_array_id,
                        principalTable: "bpm_byte_array",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bpm_proc_data_bpm_proc_inst_proc_inst_id",
                        column: x => x.proc_inst_id,
                        principalTable: "bpm_proc_inst",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bpm_proc_data_bpm_task_task_id",
                        column: x => x.task_id,
                        principalTable: "bpm_task",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bpm_proc_data_bpm_token_token_id",
                        column: x => x.token_id,
                        principalTable: "bpm_token",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bpm_act_data_act_inst_id",
                table: "bpm_act_data",
                column: "act_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_act_data_byte_array_id",
                table: "bpm_act_data",
                column: "byte_array_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_act_inst_parent_id",
                table: "bpm_act_inst",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_act_inst_proc_inst_id",
                table: "bpm_act_inst",
                column: "proc_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_act_inst_sub_proc_inst_id",
                table: "bpm_act_inst",
                column: "sub_proc_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_comment_proc_def_id",
                table: "bpm_comment",
                column: "proc_def_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_comment_proc_inst_id",
                table: "bpm_comment",
                column: "proc_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_comment_task_id",
                table: "bpm_comment",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_deployment_model_id",
                table: "bpm_deployment",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_deployment_package_id",
                table: "bpm_deployment",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_event_subscr_proc_def_id",
                table: "bpm_event_subscr",
                column: "proc_def_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_event_subscr_proc_inst_id",
                table: "bpm_event_subscr",
                column: "proc_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_event_subscr_token_id",
                table: "bpm_event_subscr",
                column: "token_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_identity_link_act_inst_id",
                table: "bpm_identity_link",
                column: "act_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_identity_link_proc_def_id",
                table: "bpm_identity_link",
                column: "proc_def_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_identity_link_proc_inst_id",
                table: "bpm_identity_link",
                column: "proc_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_identity_link_task_id",
                table: "bpm_identity_link",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_package_source_id",
                table: "bpm_package",
                column: "source_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_proc_data_byte_array_id",
                table: "bpm_proc_data",
                column: "byte_array_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_proc_data_proc_inst_id",
                table: "bpm_proc_data",
                column: "proc_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_proc_data_task_id",
                table: "bpm_proc_data",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_proc_data_token_id",
                table: "bpm_proc_data",
                column: "token_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_proc_def_deployment_id",
                table: "bpm_proc_def",
                column: "deployment_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_proc_def_key_version",
                table: "bpm_proc_def",
                columns: new[] { "key", "version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bpm_proc_inst_caller_id",
                table: "bpm_proc_inst",
                column: "caller_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_proc_inst_proc_def_id",
                table: "bpm_proc_inst",
                column: "proc_def_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_proc_inst_super_id",
                table: "bpm_proc_inst",
                column: "super_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_scheduled_job_proc_def_id",
                table: "bpm_scheduled_job",
                column: "proc_def_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_scheduled_job_proc_inst_id",
                table: "bpm_scheduled_job",
                column: "proc_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_scheduled_job_token_id",
                table: "bpm_scheduled_job",
                column: "token_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_task_act_inst_id",
                table: "bpm_task",
                column: "act_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_task_proc_inst_id",
                table: "bpm_task",
                column: "proc_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_task_token_id",
                table: "bpm_task",
                column: "token_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_token_act_inst_id",
                table: "bpm_token",
                column: "act_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_token_parent_id",
                table: "bpm_token",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_token_proc_inst_id",
                table: "bpm_token",
                column: "proc_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_token_sub_proc_inst_id",
                table: "bpm_token",
                column: "sub_proc_inst_id");

            migrationBuilder.CreateIndex(
                name: "IX_bpm_user_group_group_id",
                table: "bpm_user_group",
                column: "group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_bpm_act_data_bpm_act_inst_act_inst_id",
                table: "bpm_act_data",
                column: "act_inst_id",
                principalTable: "bpm_act_inst",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bpm_identity_link_bpm_act_inst_act_inst_id",
                table: "bpm_identity_link",
                column: "act_inst_id",
                principalTable: "bpm_act_inst",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bpm_identity_link_bpm_proc_inst_proc_inst_id",
                table: "bpm_identity_link",
                column: "proc_inst_id",
                principalTable: "bpm_proc_inst",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bpm_identity_link_bpm_task_task_id",
                table: "bpm_identity_link",
                column: "task_id",
                principalTable: "bpm_task",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bpm_proc_inst_bpm_act_inst_caller_id",
                table: "bpm_proc_inst",
                column: "caller_id",
                principalTable: "bpm_act_inst",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bpm_proc_inst_bpm_token_super_id",
                table: "bpm_proc_inst",
                column: "super_id",
                principalTable: "bpm_token",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bpm_proc_inst_bpm_act_inst_caller_id",
                table: "bpm_proc_inst");

            migrationBuilder.DropForeignKey(
                name: "FK_bpm_token_bpm_act_inst_act_inst_id",
                table: "bpm_token");

            migrationBuilder.DropForeignKey(
                name: "FK_bpm_deployment_bpm_byte_array_model_id",
                table: "bpm_deployment");

            migrationBuilder.DropForeignKey(
                name: "FK_bpm_package_bpm_byte_array_source_id",
                table: "bpm_package");

            migrationBuilder.DropForeignKey(
                name: "FK_bpm_token_bpm_proc_inst_proc_inst_id",
                table: "bpm_token");

            migrationBuilder.DropForeignKey(
                name: "FK_bpm_token_bpm_proc_inst_sub_proc_inst_id",
                table: "bpm_token");

            migrationBuilder.DropTable(
                name: "bpm_act_data");

            migrationBuilder.DropTable(
                name: "bpm_comment");

            migrationBuilder.DropTable(
                name: "bpm_event_subscr");

            migrationBuilder.DropTable(
                name: "bpm_identity_link");

            migrationBuilder.DropTable(
                name: "bpm_proc_data");

            migrationBuilder.DropTable(
                name: "bpm_scheduled_job");

            migrationBuilder.DropTable(
                name: "bpm_user_group");

            migrationBuilder.DropTable(
                name: "bpm_task");

            migrationBuilder.DropTable(
                name: "bpm_group");

            migrationBuilder.DropTable(
                name: "bpm_user");

            migrationBuilder.DropTable(
                name: "bpm_act_inst");

            migrationBuilder.DropTable(
                name: "bpm_byte_array");

            migrationBuilder.DropTable(
                name: "bpm_proc_inst");

            migrationBuilder.DropTable(
                name: "bpm_proc_def");

            migrationBuilder.DropTable(
                name: "bpm_token");

            migrationBuilder.DropTable(
                name: "bpm_deployment");

            migrationBuilder.DropTable(
                name: "bpm_package");
        }
    }
}
