﻿// <auto-generated />
using AccountData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AccountData.Migrations
{
    [DbContext(typeof(GameContext))]
    partial class GameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("AccountData.Theme", b =>
                {
                    b.Property<int>("ThemeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("PrimaryColour")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondaryColour")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ThemeId");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("AccountData.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("AggressiveOn")
                        .HasColumnType("bit");

                    b.Property<int>("Losses")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SaveExist")
                        .HasColumnType("bit");

                    b.Property<int>("ThemeId")
                        .HasColumnType("int");

                    b.Property<int>("Wins")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("ThemeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AccountData.User", b =>
                {
                    b.HasOne("AccountData.Theme", "Theme")
                        .WithMany()
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Theme");
                });
#pragma warning restore 612, 618
        }
    }
}
