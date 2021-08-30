﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using triggan.BlogManager;

namespace triggan.BlogManager.Migrations
{
    [DbContext(typeof(TrigganContext))]
    partial class TrigganContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("triggan.BlogModel.Entity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.ToTable("Entity");
                });

            modelBuilder.Entity("triggan.BlogModel.Post", b =>
                {
                    b.HasBaseType("triggan.BlogModel.Entity");

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

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("triggan.BlogModel.Project", b =>
                {
                    b.HasBaseType("triggan.BlogModel.Entity");

                    b.Property<string>("CoverImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Excerpt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Public")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("triggan.BlogModel.User", b =>
                {
                    b.HasBaseType("triggan.BlogModel.Entity");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("triggan.BlogModel.Post", b =>
                {
                    b.HasOne("triggan.BlogModel.Entity", null)
                        .WithOne()
                        .HasForeignKey("triggan.BlogModel.Post", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("triggan.BlogModel.Project", b =>
                {
                    b.HasOne("triggan.BlogModel.Entity", null)
                        .WithOne()
                        .HasForeignKey("triggan.BlogModel.Project", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.OwnsMany("triggan.BlogModel.Update", "Updates", b1 =>
                        {
                            b1.Property<int>("ProjectId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("triggan.BlogModel.User", b =>
                {
                    b.HasOne("triggan.BlogModel.Entity", null)
                        .WithOne()
                        .HasForeignKey("triggan.BlogModel.User", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
