﻿// <auto-generated />
using System;
using LinkManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LinkManager.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20190709130221_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("LinkManager.Categoria", b =>
                {
                    b.Property<int?>("IdCategoria")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descrizione");

                    b.Property<string>("Nome");

                    b.HasKey("IdCategoria");

                    b.ToTable("Categorie");
                });

            modelBuilder.Entity("LinkManager.Link", b =>
                {
                    b.Property<int>("IdLink")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descrizione");

                    b.Property<int?>("IdCategoria");

                    b.Property<string>("Titolo");

                    b.Property<string>("URL");

                    b.HasKey("IdLink");

                    b.HasIndex("IdCategoria");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("LinkManager.Link", b =>
                {
                    b.HasOne("LinkManager.Categoria", "Categoria")
                        .WithMany("Links")
                        .HasForeignKey("IdCategoria");
                });
#pragma warning restore 612, 618
        }
    }
}
