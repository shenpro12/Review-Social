﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using review.Data;

#nullable disable

namespace review.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("review.Common.Entities.AccountEntity", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int>("IsAdmin")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(60)");

                    b.HasKey("ID");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("review.Common.Entities.CategoryEntity", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("review.Common.Entities.DestinationEntity", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("Closed")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsAdmin")
                        .HasColumnType("int");

                    b.Property<string>("Lat")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Long")
                        .HasColumnType("varchar(50)");

                    b.Property<float?>("MaxPrice")
                        .HasColumnType("real");

                    b.Property<float?>("MinPrice")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("Open")
                        .HasColumnType("datetime2");

                    b.Property<string>("Phone")
                        .HasColumnType("varchar(12)");

                    b.Property<string>("ProfileID")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("ProvinceID")
                        .IsRequired()
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Thumb")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("varchar(60)");

                    b.HasKey("ID");

                    b.HasIndex("ProfileID");

                    b.HasIndex("ProvinceID");

                    b.ToTable("Destination");
                });

            modelBuilder.Entity("review.Common.Entities.ProfileEntity", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("AccountID")
                        .IsRequired()
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Identify")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .HasColumnType("varchar(12)");

                    b.HasKey("ID");

                    b.HasIndex("AccountID")
                        .IsUnique();

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("review.Common.Entities.ProfileFollowEntity", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("FollowerID")
                        .IsRequired()
                        .HasColumnType("varchar(60)");

                    b.Property<string>("FollowingID")
                        .IsRequired()
                        .HasColumnType("varchar(60)");

                    b.HasKey("ID");

                    b.HasIndex("FollowerID");

                    b.HasIndex("FollowingID");

                    b.ToTable("ProfileFollow");
                });

            modelBuilder.Entity("review.Common.Entities.ProvinceCategoryEntity", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProvinceID")
                        .IsRequired()
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Thumb")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("ID");

                    b.HasIndex("ProvinceID");

                    b.ToTable("ProvinceCategory");
                });

            modelBuilder.Entity("review.Common.Entities.ProvinceEntity", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("CategoryThumb")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Province");
                });

            modelBuilder.Entity("review.Common.Entities.RatingTypeEntity", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("RatingType");
                });

            modelBuilder.Entity("review.Common.Entities.RefeshTokenEntity", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("AccountID")
                        .IsRequired()
                        .HasColumnType("varchar(60)");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefeshToken")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.ToTable("RefeshToken");
                });

            modelBuilder.Entity("review.Common.Entities.DestinationEntity", b =>
                {
                    b.HasOne("review.Common.Entities.ProfileEntity", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileID");

                    b.HasOne("review.Common.Entities.ProvinceEntity", "Province")
                        .WithMany("DestinationEntitys")
                        .HasForeignKey("ProvinceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");

                    b.Navigation("Province");
                });

            modelBuilder.Entity("review.Common.Entities.ProfileEntity", b =>
                {
                    b.HasOne("review.Common.Entities.AccountEntity", "Account")
                        .WithOne("Profile")
                        .HasForeignKey("review.Common.Entities.ProfileEntity", "AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("review.Common.Entities.ProfileFollowEntity", b =>
                {
                    b.HasOne("review.Common.Entities.ProfileEntity", "Follower")
                        .WithMany("Followers")
                        .HasForeignKey("FollowerID")
                        .IsRequired()
                        .HasConstraintName("FK_ProfileFollow_Profile_FollowerID");

                    b.HasOne("review.Common.Entities.ProfileEntity", "Following")
                        .WithMany("Followings")
                        .HasForeignKey("FollowingID")
                        .IsRequired()
                        .HasConstraintName("FK_ProfileFollow_Profile_FollowingID");

                    b.Navigation("Follower");

                    b.Navigation("Following");
                });

            modelBuilder.Entity("review.Common.Entities.ProvinceCategoryEntity", b =>
                {
                    b.HasOne("review.Common.Entities.ProvinceEntity", "Province")
                        .WithMany("ProvinceCategorys")
                        .HasForeignKey("ProvinceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("review.Common.Entities.RefeshTokenEntity", b =>
                {
                    b.HasOne("review.Common.Entities.AccountEntity", "Account")
                        .WithMany("RefeshTokens")
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("review.Common.Entities.AccountEntity", b =>
                {
                    b.Navigation("Profile")
                        .IsRequired();

                    b.Navigation("RefeshTokens");
                });

            modelBuilder.Entity("review.Common.Entities.ProfileEntity", b =>
                {
                    b.Navigation("Followers");

                    b.Navigation("Followings");
                });

            modelBuilder.Entity("review.Common.Entities.ProvinceEntity", b =>
                {
                    b.Navigation("DestinationEntitys");

                    b.Navigation("ProvinceCategorys");
                });
#pragma warning restore 612, 618
        }
    }
}
