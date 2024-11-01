﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dotnet_github_webhook;

#nullable disable

namespace dotnet_github_webhook.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20241101103535_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("dotnet_github_webhook.Models.OptionChange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("FileName")
                        .HasMaxLength(64)
                        .HasColumnType("VARCHAR2")
                        .HasColumnName("FileName");

                    b.Property<string>("From")
                        .HasMaxLength(64)
                        .HasColumnType("VARCHAR2")
                        .HasColumnName("From");

                    b.Property<string>("To")
                        .HasMaxLength(64)
                        .HasColumnType("VARCHAR2")
                        .HasColumnName("To");

                    b.HasKey("Id");

                    b.ToTable("OptionChange");
                });

            modelBuilder.Entity("dotnet_github_webhook.Models.OptionVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("Id");

                    b.Property<string>("Tag")
                        .HasMaxLength(64)
                        .HasColumnType("VARCHAR2")
                        .HasColumnName("Tag");

                    b.HasKey("Id");

                    b.ToTable("OptionVersion");
                });
#pragma warning restore 612, 618
        }
    }
}