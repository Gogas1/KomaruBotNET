﻿// <auto-generated />
using KomaruBotASPNET.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KomaruBotASPNET.Migrations
{
    [DbContext(typeof(KomaruDbContext))]
    partial class KomaruDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KomaruBotASPNET.Models.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GifId")
                        .HasColumnType("int");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GifId");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("KomaruBotASPNET.Models.KomaruGif", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FileType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelegramId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("KomaruGifs");
                });

            modelBuilder.Entity("KomaruBotASPNET.Models.MyUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint");

                    b.Property<int>("UserState")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KomaruBotASPNET.Models.UserInputState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserInputStates");
                });

            modelBuilder.Entity("KomaruBotASPNET.Models.Keyword", b =>
                {
                    b.HasOne("KomaruBotASPNET.Models.KomaruGif", "Gif")
                        .WithMany("Keywords")
                        .HasForeignKey("GifId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gif");
                });

            modelBuilder.Entity("KomaruBotASPNET.Models.UserInputState", b =>
                {
                    b.HasOne("KomaruBotASPNET.Models.MyUser", "User")
                        .WithOne("InputState")
                        .HasForeignKey("KomaruBotASPNET.Models.UserInputState", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("KomaruBotASPNET.Models.StateFlows.AddKomaruFlow", "AddKomaruFlow", b1 =>
                        {
                            b1.Property<int>("UserInputStateId")
                                .HasColumnType("int");

                            b1.Property<string>("FileId")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("FileType")
                                .HasColumnType("int");

                            b1.Property<string>("Keywords")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserInputStateId");

                            b1.ToTable("UserInputStates");

                            b1.WithOwner()
                                .HasForeignKey("UserInputStateId");
                        });

                    b.Navigation("AddKomaruFlow")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("KomaruBotASPNET.Models.KomaruGif", b =>
                {
                    b.Navigation("Keywords");
                });

            modelBuilder.Entity("KomaruBotASPNET.Models.MyUser", b =>
                {
                    b.Navigation("InputState")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
