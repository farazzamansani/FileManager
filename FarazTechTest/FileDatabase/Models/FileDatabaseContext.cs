using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FileDatabase.Models;

public partial class FileDatabaseContext : DbContext
{
    public FileDatabaseContext()
    {
    }

    public FileDatabaseContext(DbContextOptions<FileDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Folder> Folders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=FileDatabase;Username=postgres;Password=password");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Fileid).HasName("files_pkey");

            entity.ToTable("files");

            entity.Property(e => e.Fileid).HasColumnName("fileid");
            entity.Property(e => e.Filedata).HasColumnName("filedata");
            entity.Property(e => e.Filetype)
                .HasMaxLength(50)
                .HasColumnName("filetype");
            entity.Property(e => e.Folderid).HasColumnName("folderid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UploadedDateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("uploadeddatetime");

            entity.HasOne(d => d.Folder).WithMany(p => p.Files)
                .HasForeignKey(d => d.Folderid)
                .HasConstraintName("files_folderid_fkey");
        });

        modelBuilder.Entity<Folder>(entity =>
        {
            entity.HasKey(e => e.Folderid).HasName("folders_pkey");

            entity.ToTable("folders");

            entity.Property(e => e.Folderid).HasColumnName("folderid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Parentfolderid).HasColumnName("parentfolderid");

            entity.HasOne(d => d.Parentfolder).WithMany(p => p.InverseParentfolder)
                .HasForeignKey(d => d.Parentfolderid)
                .HasConstraintName("folders_parentfolderid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
