﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using testEsp32.Models;

namespace testEsp32.Data
{
    public partial class EspconnecttestContext : DbContext
    {
        public EspconnecttestContext()
        {
        }

        public EspconnecttestContext(DbContextOptions<EspconnecttestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Boards> Boards { get; set; }
        public virtual DbSet<Outputs> Outputs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:huytm.database.windows.net,1433;Initial Catalog=esp32;Persist Security Info=False;User ID=huytm;Password=Abc12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Boards>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Board).HasColumnName("board");

                entity.Property(e => e.LastRequest)
                    .IsRequired()
                    .HasColumnName("lastRequest")
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Outputs>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Board).HasColumnName("board");

                entity.Property(e => e.Gpio).HasColumnName("gpio");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State).HasColumnName("state");

                entity.HasOne(d => d.BoardNavigation)
                    .WithMany(p => p.Outputs)
                    .HasForeignKey(d => d.Board)
                    .HasConstraintName("FK_Outputs_Boards");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}