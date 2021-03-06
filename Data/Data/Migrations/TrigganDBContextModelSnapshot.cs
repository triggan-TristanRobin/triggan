﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Migrations
{
    [DbContext(typeof(TrigganContext))]
    partial class TrigganDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Model.Entity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.ToTable("Entity");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Entity");
                });

            modelBuilder.Entity("Model.Message", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<string>("EMail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Model.Post", b =>
                {
                    b.HasBaseType("Model.Entity");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Excerpt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Public")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Post");
                });

            modelBuilder.Entity("Model.Project", b =>
                {
                    b.HasBaseType("Model.Entity");

                    b.Property<string>("CoverImagePath")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Project_CoverImagePath");

                    b.Property<string>("Excerpt")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Project_Excerpt");

                    b.Property<bool>("Public")
                        .HasColumnType("bit")
                        .HasColumnName("Project_Public");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("Project_PublicationDate");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Project_Title");

                    b.Property<int>("Views")
                        .HasColumnType("int")
                        .HasColumnName("Project_Views");

                    b.HasDiscriminator().HasValue("Project");
                });

            modelBuilder.Entity("Model.Project", b =>
                {
                    b.OwnsMany("Model.Update", "Updates", b1 =>
                        {
                            b1.Property<string>("ProjectId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .UseIdentityColumn();

                            b1.Property<string>("Content")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("datetime2");

                            b1.Property<DateTime>("PublicationDate")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Title")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ProjectId", "Id");

                            b1.ToTable("Update");

                            b1.WithOwner()
                                .HasForeignKey("ProjectId");
                        });

                    b.Navigation("Updates");
                });
#pragma warning restore 612, 618
        }
    }
}
