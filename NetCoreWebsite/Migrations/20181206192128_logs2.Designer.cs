﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetCoreWebsite.Data;

namespace NetCoreWebsite.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181206192128_logs2")]
    partial class logs2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetCoreWebsite.Data.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Modified");

                    b.Property<int?>("OwnerId");

                    b.Property<string>("Text");

                    b.HasKey("MessageId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("NetCoreWebsite.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LastSuccesfullLogin");

                    b.Property<bool>("Locked");

                    b.Property<int>("MaxFailedCount");

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.Property<string>("Salt");

                    b.Property<int>("SecondPasswordId");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NetCoreWebsite.Data.Models.UserLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int>("Step");

                    b.Property<bool>("Successfull");

                    b.Property<int?>("UserId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogs");
                });

            modelBuilder.Entity("NetCoreWebsite.Data.Models.UserMessage", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("MessageId");

                    b.HasKey("UserId", "MessageId");

                    b.HasIndex("MessageId");

                    b.ToTable("UserMessages");
                });

            modelBuilder.Entity("NetCoreWebsite.Data.Models.UserSecondPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hash");

                    b.Property<string>("Mask");

                    b.Property<bool>("Removed");

                    b.Property<int?>("userFK");

                    b.HasKey("Id");

                    b.HasIndex("userFK");

                    b.ToTable("UserSecondPasswords");
                });

            modelBuilder.Entity("NetCoreWebsite.Data.Models.Message", b =>
                {
                    b.HasOne("NetCoreWebsite.Data.Models.User", "Owner")
                        .WithMany("OwnedMessages")
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("NetCoreWebsite.Data.Models.UserLogs", b =>
                {
                    b.HasOne("NetCoreWebsite.Data.Models.User", "User")
                        .WithMany("LoginLogs")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NetCoreWebsite.Data.Models.UserMessage", b =>
                {
                    b.HasOne("NetCoreWebsite.Data.Models.Message", "Message")
                        .WithMany("Allowed")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetCoreWebsite.Data.Models.User", "User")
                        .WithMany("AllowedMessages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCoreWebsite.Data.Models.UserSecondPassword", b =>
                {
                    b.HasOne("NetCoreWebsite.Data.Models.User", "User")
                        .WithMany("SecondPassword")
                        .HasForeignKey("userFK");
                });
#pragma warning restore 612, 618
        }
    }
}
