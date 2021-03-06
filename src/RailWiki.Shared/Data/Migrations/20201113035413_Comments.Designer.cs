﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RailWiki.Shared.Data;

namespace RailWiki.Shared.Data.Migrations
{
    [DbContext(typeof(RailWikiDbContext))]
    [Migration("20201113035413_Comments")]
    partial class Comments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("RailWiki.Shared.Entities.CrossReference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SourceIdentifier")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("CrossReferences");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Geography.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Geography.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<float?>("Latitude")
                        .HasColumnType("real");

                    b.Property<float?>("Longitude")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("StateProvinceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StateProvinceId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Geography.StateProvince", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("StateProvinces");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CoverPhotoFileName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("CoverPhotoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("LocationId")
                        .HasColumnType("int");

                    b.Property<int>("PhotoCount")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CommentText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<string>("EntityType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FlagReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FlagUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FlaggedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AlbumId")
                        .HasColumnType("int");

                    b.Property<string>("Author")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("LocationName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("PhotoDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.PhotoCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("PhotoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PhotoId");

                    b.ToTable("PhotoCategories");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.PhotoLocomotive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("LocomotiveId")
                        .HasColumnType("int");

                    b.Property<int>("PhotoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LocomotiveId");

                    b.HasIndex("PhotoId");

                    b.ToTable("PhotoLocomotives");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.PhotoRollingStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("PhotoId")
                        .HasColumnType("int");

                    b.Property<int>("RollingStockId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.HasIndex("RollingStockId");

                    b.ToTable("PhotoRollingStocks");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.Locomotive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("BuildMonth")
                        .HasColumnType("int");

                    b.Property<int?>("BuildYear")
                        .HasColumnType("int");

                    b.Property<string>("BuiltAs")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FrameNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ModelNumber")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Notes")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ReportingMarks")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("RoadId")
                        .HasColumnType("int");

                    b.Property<string>("RoadNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("SerialNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Slug")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoadId");

                    b.HasIndex("TypeId");

                    b.ToTable("Locomotives");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.LocomotiveType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Family")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("LocomotiveCount")
                        .HasColumnType("int");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LocomotiveTypes");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.Road", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("LocomotiveCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("ReportingMarks")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("RoadTypeId")
                        .HasColumnType("int");

                    b.Property<int>("RollingStockCount")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("RoadTypeId");

                    b.ToTable("Roads");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.RoadAlternateName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ReportingMarks")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("RoadId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoadId");

                    b.ToTable("RoadAlternateNames");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.RoadType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("RoadTypes");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.RollingStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Details")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("DryCapacity")
                        .HasColumnType("int");

                    b.Property<string>("ExteriorDimensions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InteriorDimensions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LoadLimit")
                        .HasColumnType("int");

                    b.Property<int>("MaxGrossWeight")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Plate")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ReportingMarks")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("RoadId")
                        .HasColumnType("int");

                    b.Property<string>("RoadNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("RollingStockClassId")
                        .HasColumnType("int");

                    b.Property<int>("RollingStockTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Slug")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoadId");

                    b.HasIndex("RollingStockClassId");

                    b.HasIndex("RollingStockTypeId");

                    b.ToTable("RollingStockItems");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.RollingStockClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AARDesignation")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RollingStockTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RollingStockTypeId");

                    b.ToTable("RollingStockClasses");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.RollingStockType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AARDesignation")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("RollingStockTypes");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("ApprovedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("RegisteredOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SubjectId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Geography.Location", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Geography.StateProvince", "StateProvince")
                        .WithMany()
                        .HasForeignKey("StateProvinceId");

                    b.Navigation("StateProvince");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Geography.StateProvince", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Geography.Country", "Country")
                        .WithMany("StateProvinces")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.Album", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Geography.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("RailWiki.Shared.Entities.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.Comment", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.Photo", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Photos.Album", "Album")
                        .WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RailWiki.Shared.Entities.Geography.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("RailWiki.Shared.Entities.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Album");

                    b.Navigation("Location");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.PhotoCategory", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Photos.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RailWiki.Shared.Entities.Photos.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Photo");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.PhotoLocomotive", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Roster.Locomotive", "Locomotive")
                        .WithMany()
                        .HasForeignKey("LocomotiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RailWiki.Shared.Entities.Photos.Photo", "Photo")
                        .WithMany("Locomotives")
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Locomotive");

                    b.Navigation("Photo");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.PhotoRollingStock", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Photos.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RailWiki.Shared.Entities.Roster.RollingStock", "RollingStock")
                        .WithMany()
                        .HasForeignKey("RollingStockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Photo");

                    b.Navigation("RollingStock");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.Locomotive", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Roster.Road", "Road")
                        .WithMany()
                        .HasForeignKey("RoadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RailWiki.Shared.Entities.Roster.LocomotiveType", "Model")
                        .WithMany()
                        .HasForeignKey("TypeId");

                    b.Navigation("Model");

                    b.Navigation("Road");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.Road", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Roster.Road", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("RailWiki.Shared.Entities.Roster.RoadType", "RoadType")
                        .WithMany("Roads")
                        .HasForeignKey("RoadTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parent");

                    b.Navigation("RoadType");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.RoadAlternateName", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Roster.Road", "Road")
                        .WithMany("AlternateNames")
                        .HasForeignKey("RoadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Road");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.RollingStock", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Roster.Road", "Road")
                        .WithMany()
                        .HasForeignKey("RoadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RailWiki.Shared.Entities.Roster.RollingStockClass", "RollingStockClass")
                        .WithMany()
                        .HasForeignKey("RollingStockClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RailWiki.Shared.Entities.Roster.RollingStockType", "RollingStockType")
                        .WithMany()
                        .HasForeignKey("RollingStockTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Road");

                    b.Navigation("RollingStockClass");

                    b.Navigation("RollingStockType");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.RollingStockClass", b =>
                {
                    b.HasOne("RailWiki.Shared.Entities.Roster.RollingStockType", "RollingStockType")
                        .WithMany()
                        .HasForeignKey("RollingStockTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RollingStockType");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Geography.Country", b =>
                {
                    b.Navigation("StateProvinces");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Photos.Photo", b =>
                {
                    b.Navigation("Locomotives");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.Road", b =>
                {
                    b.Navigation("AlternateNames");
                });

            modelBuilder.Entity("RailWiki.Shared.Entities.Roster.RoadType", b =>
                {
                    b.Navigation("Roads");
                });
#pragma warning restore 612, 618
        }
    }
}
