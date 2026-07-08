using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GiftCardSystem.Persistence.Migrations
{
    public partial class InitialCreateModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo)
                .HasAnnotation("ProductVersion", "5.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GiftCardSystem.Domain.Entities.Customer", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<string>("Email")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("character varying(100)");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("character varying(100)");

                b.Property<string>("PhoneNumber")
                    .HasMaxLength(20)
                    .HasColumnType("character varying(20)");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("timestamp without time zone");

                b.HasKey("Id");

                b.ToTable("Customers");
            });

            modelBuilder.Entity("GiftCardSystem.Domain.Entities.GiftCard", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<string>("Code")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("character varying(20)");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp without time zone");

                b.Property<decimal>("CurrentBalance")
                    .HasPrecision(18, 2)
                    .HasColumnType("numeric(18,2)");

                b.Property<Guid?>("CustomerId")
                    .HasColumnType("uuid");

                b.Property<DateTime?>("ExpiryDate")
                    .HasColumnType("timestamp without time zone");

                b.Property<decimal>("InitialBalance")
                    .HasPrecision(18, 2)
                    .HasColumnType("numeric(18,2)");

                b.Property<DateTime>("IssuedDate")
                    .HasColumnType("timestamp without time zone");

                b.Property<int>("Status")
                    .HasColumnType("integer");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("timestamp without time zone");

                b.HasKey("Id");

                b.HasIndex("Code")
                    .IsUnique();

                b.HasIndex("CustomerId");

                b.ToTable("GiftCards");
            });

            modelBuilder.Entity("GiftCardSystem.Domain.Entities.Redemption", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<decimal>("Amount")
                    .HasPrecision(18, 2)
                    .HasColumnType("numeric(18,2)");

                b.Property<string>("Description")
                    .HasMaxLength(500)
                    .HasColumnType("character varying(500)");

                b.Property<Guid>("GiftCardId")
                    .HasColumnType("uuid");

                b.Property<DateTime>("RedeemedDate")
                    .HasColumnType("timestamp without time zone");

                b.Property<int>("Status")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.HasIndex("GiftCardId");

                b.ToTable("Redemptions");
            });

            modelBuilder.Entity("GiftCardSystem.Domain.Entities.Transaction", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid");

                b.Property<decimal>("Amount")
                    .HasPrecision(18, 2)
                    .HasColumnType("numeric(18,2)");

                b.Property<string>("Description")
                    .HasMaxLength(500)
                    .HasColumnType("character varying(500)");

                b.Property<Guid>("GiftCardId")
                    .HasColumnType("uuid");

                b.Property<string>("Reference")
                    .HasMaxLength(100)
                    .HasColumnType("character varying(100)");

                b.Property<DateTime>("TransactionDate")
                    .HasColumnType("timestamp without time zone");

                b.Property<int>("Type")
                    .HasColumnType("integer");

                b.HasKey("Id");

                b.HasIndex("GiftCardId");

                b.ToTable("Transactions");
            });

            modelBuilder.Entity("GiftCardSystem.Domain.Entities.GiftCard", b =>
            {
                b.HasOne("GiftCardSystem.Domain.Entities.Customer", "Customer")
                    .WithMany("GiftCards")
                    .HasForeignKey("CustomerId")
                    .OnDelete(DeleteBehavior.SetNull);

                b.Navigation("Customer");
            });

            modelBuilder.Entity("GiftCardSystem.Domain.Entities.Redemption", b =>
            {
                b.HasOne("GiftCardSystem.Domain.Entities.GiftCard", "GiftCard")
                    .WithMany()
                    .HasForeignKey("GiftCardId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("GiftCard");
            });

            modelBuilder.Entity("GiftCardSystem.Domain.Entities.Transaction", b =>
            {
                b.HasOne("GiftCardSystem.Domain.Entities.GiftCard", "GiftCard")
                    .WithMany("Transactions")
                    .HasForeignKey("GiftCardId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("GiftCard");
            });

            modelBuilder.Entity("GiftCardSystem.Domain.Entities.Customer", b =>
            {
                b.Navigation("GiftCards");
            });

            modelBuilder.Entity("GiftCardSystem.Domain.Entities.GiftCard", b =>
            {
                b.Navigation("Transactions");
            });
#pragma warning restore 612, 618
        }
    }
}
