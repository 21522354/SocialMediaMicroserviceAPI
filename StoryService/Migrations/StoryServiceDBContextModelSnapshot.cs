﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoryService.Data_Layer;

#nullable disable

namespace StoryService.Migrations
{
    [DbContext(typeof(StoryServiceDBContext))]
    partial class StoryServiceDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StoryService.Data_Layer.Models.Story", b =>
                {
                    b.Property<Guid>("StoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSaved")
                        .HasColumnType("bit");

                    b.Property<string>("Sound")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("StoryId");

                    b.ToTable("Stories");
                });

            modelBuilder.Entity("StoryService.Data_Layer.Models.UserAlreadySeenStory", b =>
                {
                    b.Property<Guid>("StoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("StoryId", "UserId");

                    b.ToTable("UserAlreadySeenStories");
                });

            modelBuilder.Entity("StoryService.Data_Layer.Models.UserAlreadySeenStory", b =>
                {
                    b.HasOne("StoryService.Data_Layer.Models.Story", "Story")
                        .WithMany("UserAlreadySeenStories")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Story");
                });

            modelBuilder.Entity("StoryService.Data_Layer.Models.Story", b =>
                {
                    b.Navigation("UserAlreadySeenStories");
                });
#pragma warning restore 612, 618
        }
    }
}
