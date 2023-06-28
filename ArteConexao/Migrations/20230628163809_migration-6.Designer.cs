﻿// <auto-generated />
using System;
using ArteConexao.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ArteConexao.Migrations
{
    [DbContext(typeof(ArteConexaoDbContext))]
    [Migration("20230628163809_migration-6")]
    partial class migration6
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ArteConexao.Models.Carrinho", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Carrinhos");
                });

            modelBuilder.Entity("ArteConexao.Models.ItemCarrinho", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CarrinhoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImagemUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<Guid>("StandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ValorReserva")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CarrinhoId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("ItensCarrinho");
                });

            modelBuilder.Entity("ArteConexao.Models.ItemReserva", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<Guid>("ReservaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorReserva")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.HasIndex("ReservaId");

                    b.HasIndex("StandId");

                    b.ToTable("ItensReserva");
                });

            modelBuilder.Entity("ArteConexao.Models.Produto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Altura")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Categoria")
                        .HasColumnType("int");

                    b.Property<decimal>("Comprimento")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataInclusao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagemUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Largura")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaisOrigem")
                        .HasColumnType("int");

                    b.Property<int>("QuantidadeDisponivel")
                        .HasColumnType("int");

                    b.Property<int>("QuantidadeTotal")
                        .HasColumnType("int");

                    b.Property<Guid>("StandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UsuarioAlteracao")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UsuarioInclusao")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ValorAtual")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ValorReserva")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("ArteConexao.Models.Reserva", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataInclusao")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Reservas");
                });

            modelBuilder.Entity("ArteConexao.Models.Stand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataInclusao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Localizacao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UsuarioAlteracao")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UsuarioInclusao")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Stands");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("StandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("StandId");

                    b.ToTable("IdentityUser");
                });

            modelBuilder.Entity("ProdutoStand", b =>
                {
                    b.Property<Guid>("ProdutosId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StandsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ProdutosId", "StandsId");

                    b.HasIndex("StandsId");

                    b.ToTable("ProdutoStand");
                });

            modelBuilder.Entity("ArteConexao.Models.ItemCarrinho", b =>
                {
                    b.HasOne("ArteConexao.Models.Carrinho", null)
                        .WithMany("ItensCarrinho")
                        .HasForeignKey("CarrinhoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArteConexao.Models.Produto", null)
                        .WithMany("ItensCarrinho")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ArteConexao.Models.ItemReserva", b =>
                {
                    b.HasOne("ArteConexao.Models.Produto", null)
                        .WithMany("ItensReserva")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArteConexao.Models.Reserva", null)
                        .WithMany("ItensReserva")
                        .HasForeignKey("ReservaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArteConexao.Models.Stand", null)
                        .WithMany("ItensReserva")
                        .HasForeignKey("StandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.HasOne("ArteConexao.Models.Stand", null)
                        .WithMany("Usuarios")
                        .HasForeignKey("StandId");
                });

            modelBuilder.Entity("ProdutoStand", b =>
                {
                    b.HasOne("ArteConexao.Models.Produto", null)
                        .WithMany()
                        .HasForeignKey("ProdutosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArteConexao.Models.Stand", null)
                        .WithMany()
                        .HasForeignKey("StandsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ArteConexao.Models.Carrinho", b =>
                {
                    b.Navigation("ItensCarrinho");
                });

            modelBuilder.Entity("ArteConexao.Models.Produto", b =>
                {
                    b.Navigation("ItensCarrinho");

                    b.Navigation("ItensReserva");
                });

            modelBuilder.Entity("ArteConexao.Models.Reserva", b =>
                {
                    b.Navigation("ItensReserva");
                });

            modelBuilder.Entity("ArteConexao.Models.Stand", b =>
                {
                    b.Navigation("ItensReserva");

                    b.Navigation("Usuarios");
                });
#pragma warning restore 612, 618
        }
    }
}
