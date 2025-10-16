using System;
using Feedback.Api.Data;
using Feedback.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Feedback.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("Feedback.Api.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AzureAdObjectId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("AzureAdObjectId")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Feedback.Api.Models.FeedbackEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("longtext")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("TopicId");

                    b.ToTable("FeedbackEntries");
                });

            modelBuilder.Entity("Feedback.Api.Models.FeedbackTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("FeedbackTopics");

                    b.HasData(
                        new FeedbackTopic { Id = 1, Name = "Culture" },
                        new FeedbackTopic { Id = 2, Name = "Career Growth" },
                        new FeedbackTopic { Id = 3, Name = "Compensation" },
                        new FeedbackTopic { Id = 4, Name = "Work Environment" });
                });

            modelBuilder.Entity("Feedback.Api.Models.Employee", b =>
                {
                    b.HasMany("Feedback.Api.Models.FeedbackEntry", "Feedback")
                        .WithOne("Employee")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feedback");
                });

            modelBuilder.Entity("Feedback.Api.Models.FeedbackEntry", b =>
                {
                    b.HasOne("Feedback.Api.Models.Employee", "Employee")
                        .WithMany("Feedback")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Feedback.Api.Models.FeedbackTopic", "Topic")
                        .WithMany("Feedback")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Feedback.Api.Models.FeedbackTopic", b =>
                {
                    b.Navigation("Feedback");
                });
#pragma warning restore 612, 618
        }
    }
}
