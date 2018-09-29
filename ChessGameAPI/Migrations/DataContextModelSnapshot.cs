﻿// <auto-generated />
using System;
using ChessGameAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChessGameAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChessGameAPI.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BlackUserId");

                    b.Property<bool>("CanBlackKingSideCastle");

                    b.Property<bool>("CanBlackQueenSideCastle");

                    b.Property<bool>("CanWhiteKingSideCastle");

                    b.Property<bool>("CanWhiteQueenSideCastle");

                    b.Property<DateTime?>("DateCompleted");

                    b.Property<DateTime>("DateCreated");

                    b.Property<int?>("WhiteUserId");

                    b.Property<int>("statusCode");

                    b.HasKey("Id");

                    b.HasIndex("BlackUserId");

                    b.HasIndex("WhiteUserId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Move", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AlgebraicNotation");

                    b.Property<int>("EndX");

                    b.Property<int>("EndY");

                    b.Property<int>("GameId");

                    b.Property<string>("PieceDiscriminator");

                    b.Property<int?>("PieceId");

                    b.Property<int>("StartX");

                    b.Property<int>("StartY");

                    b.Property<byte[]>("TimeStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PieceId");

                    b.HasIndex("UserId");

                    b.ToTable("Moves");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Caption");

                    b.Property<DateTime>("DateAdded");

                    b.Property<bool>("IsMain");

                    b.Property<string>("URL")
                        .IsRequired();

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Piece", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("GameId");

                    b.Property<int?>("OwnedById");

                    b.Property<int>("X");

                    b.Property<int>("Y");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("OwnedById");

                    b.ToTable("Pieces");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Piece");
                });

            modelBuilder.Entity("ChessGameAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateJoined");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<DateTime>("LastActive");

                    b.Property<string>("Name");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired();

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired();

                    b.Property<DateTimeOffset?>("utcOffset");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Pieces.Bishop", b =>
                {
                    b.HasBaseType("ChessGameAPI.Models.Piece");


                    b.ToTable("Bishop");

                    b.HasDiscriminator().HasValue("Bishop");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Pieces.King", b =>
                {
                    b.HasBaseType("ChessGameAPI.Models.Piece");


                    b.ToTable("King");

                    b.HasDiscriminator().HasValue("King");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Pieces.Knight", b =>
                {
                    b.HasBaseType("ChessGameAPI.Models.Piece");


                    b.ToTable("Knight");

                    b.HasDiscriminator().HasValue("Knight");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Pieces.Pawn", b =>
                {
                    b.HasBaseType("ChessGameAPI.Models.Piece");


                    b.ToTable("Pawn");

                    b.HasDiscriminator().HasValue("Pawn");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Pieces.Queen", b =>
                {
                    b.HasBaseType("ChessGameAPI.Models.Piece");


                    b.ToTable("Queen");

                    b.HasDiscriminator().HasValue("Queen");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Pieces.Rook", b =>
                {
                    b.HasBaseType("ChessGameAPI.Models.Piece");


                    b.ToTable("Rook");

                    b.HasDiscriminator().HasValue("Rook");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Game", b =>
                {
                    b.HasOne("ChessGameAPI.Models.User", "BlackUser")
                        .WithMany()
                        .HasForeignKey("BlackUserId");

                    b.HasOne("ChessGameAPI.Models.User", "WhiteUser")
                        .WithMany()
                        .HasForeignKey("WhiteUserId");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Move", b =>
                {
                    b.HasOne("ChessGameAPI.Models.Game", "Game")
                        .WithMany("Moves")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChessGameAPI.Models.Piece", "Piece")
                        .WithMany("Moves")
                        .HasForeignKey("PieceId");

                    b.HasOne("ChessGameAPI.Models.User", "User")
                        .WithMany("Moves")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ChessGameAPI.Models.Photo", b =>
                {
                    b.HasOne("ChessGameAPI.Models.User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ChessGameAPI.Models.Piece", b =>
                {
                    b.HasOne("ChessGameAPI.Models.Game", "Game")
                        .WithMany("Pieces")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChessGameAPI.Models.User", "OwnedBy")
                        .WithMany("Pieces")
                        .HasForeignKey("OwnedById");
                });
#pragma warning restore 612, 618
        }
    }
}
