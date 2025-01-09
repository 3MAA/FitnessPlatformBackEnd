using System;
using System.Collections.Generic;
using FitnessPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessPlatform.Context;

public partial class FitnessDbContext : DbContext
{
    public FitnessDbContext()
    {
    }

    public FitnessDbContext(DbContextOptions<FitnessDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<NutritionalPlan> NutritionalPlans { get; set; }

    public virtual DbSet<Objective> Objectives { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

    public virtual DbSet<SubscriptionWorkout> SubscriptionWorkouts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserWorkout> UserWorkouts { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ASUS;Initial Catalog=FitnessPlatformDB;Integrated Security=True;Encrypt=false;Persist Security Info=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__Discount__E43F6D96989FDC9D");

            //entity.Property(e => e.DiscountId).ValueGeneratedNever();
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.UserIdNavigation).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Discounts__UserI__4316F928");
        });

        modelBuilder.Entity<NutritionalPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__Nutritio__755C22B727179788");

            entity.ToTable("NutritionalPlan");

            entity.Property(e => e.PlanId).ValueGeneratedNever();
            entity.Property(e => e.FoodStuff)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FoodStuffCalories).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Grammage).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.MealCalories).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.MealType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PlanType)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Objective>(entity =>
        {
            entity.HasKey(e => e.ObjectiveId).HasName("PK__Objectiv__8C5633AD6112FF2E");

            entity.Property(e => e.ObjectiveType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Progress).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.TargetValue).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.UserIdNavigation).WithMany(p => p.Objectives)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Objective__UserI__3D5E1FD2");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B249DFA8EF3C6");

            entity.Property(e => e.SubscriptionId).ValueGeneratedNever();
            entity.Property(e => e.Price).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.SubscriptionType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Term)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.UserIdNavigation).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Subscript__UserI__403A8C7D");
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SubscriptionPlan");

            entity.Property(e => e.PlanPrice).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.Plan).WithMany()
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK__Subscript__PlanI__45F365D3");

            entity.HasOne(d => d.Subscription).WithMany()
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("FK__Subscript__Subsc__44FF419A");
        });

        modelBuilder.Entity<SubscriptionWorkout>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SubscriptionWorkout");

            entity.Property(e => e.WorkoutPrice).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.Subscription).WithMany()
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("FK__Subscript__Subsc__47DBAE45");

            entity.HasOne(d => d.Workout).WithMany()
                .HasForeignKey(d => d.WorkoutId)
                .HasConstraintName("FK__Subscript__Worko__48CFD27E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C20CDA3AF");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserHeight).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserWeight).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("PK__Workouts__E1C42A01CE1317B5");

            entity.Property(e => e.CaloriesBurned).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.ContentPath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DifficultyLevel)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.WorkoutDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.WorkoutDuration)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.WorkoutType)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
