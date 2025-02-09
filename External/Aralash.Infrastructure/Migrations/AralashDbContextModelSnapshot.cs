﻿// <auto-generated />
using System;
using Aralash.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Aralash.Infrastructure.Migrations
{
    [DbContext(typeof(AralashDbContext))]
    partial class AralashDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Aralash.Domain.Entites.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = "absolute-1337",
                            Description = "Супер админ системы",
                            Name = "Супер админ"
                        });
                });

            modelBuilder.Entity("Aralash.Domain.Entites.RoleOperation", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("character varying(36)");

                    b.Property<string>("OperationId")
                        .HasColumnType("character varying(36)");

                    b.HasKey("RoleId", "OperationId");

                    b.HasIndex("OperationId");

                    b.ToTable("RoleOperations");
                });

            modelBuilder.Entity("Aralash.Domain.Entites.SecuredOperation", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("OperationName")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)");

                    b.HasKey("Id");

                    b.HasIndex("OperationName")
                        .IsUnique()
                        .HasDatabaseName("ix_operation_name");

                    b.ToTable("SecuredOperations");
                });

            modelBuilder.Entity("Aralash.Domain.Entites.Token", b =>
                {
                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("character varying(36)");

                    b.HasKey("RefreshToken", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Aralash.Domain.Entites.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Firstname")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Lastname")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Aralash.Domain.Entites.UserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("character varying(36)");

                    b.Property<string>("RoleId")
                        .HasColumnType("character varying(36)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Aralash.Domain.Entites.RoleOperation", b =>
                {
                    b.HasOne("Aralash.Domain.Entites.SecuredOperation", "Operation")
                        .WithMany("Roles")
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aralash.Domain.Entites.Role", "Role")
                        .WithMany("Operations")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Operation");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Aralash.Domain.Entites.Token", b =>
                {
                    b.HasOne("Aralash.Domain.Entites.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Aralash.Domain.Entites.UserRole", b =>
                {
                    b.HasOne("Aralash.Domain.Entites.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aralash.Domain.Entites.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Aralash.Domain.Entites.Role", b =>
                {
                    b.Navigation("Operations");
                });

            modelBuilder.Entity("Aralash.Domain.Entites.SecuredOperation", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Aralash.Domain.Entites.User", b =>
                {
                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
