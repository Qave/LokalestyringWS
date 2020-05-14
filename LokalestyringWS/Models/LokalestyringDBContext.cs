namespace LokalestyringWS.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LokalestyringDBContext : DbContext
    {
        public LokalestyringDBContext()
            : base("name=LokalestyringDBContext")
        {
            base.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Roomtype> Roomtypes { get; set; }
        public virtual DbSet<TavleBooking> TavleBookings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AllBookingsView> AllBookingsViews { get; set; }
        public virtual DbSet<RoomsView> RoomsViews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasMany(e => e.TavleBookings)
                .WithRequired(e => e.Booking)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Building>()
                .Property(e => e.Building_Letter)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Building>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Building>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.Building)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.No)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.Room)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roomtype>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<Roomtype>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.Roomtype)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AllBookingsView>()
                .Property(e => e.RoomName)
                .IsUnicode(false);

            modelBuilder.Entity<AllBookingsView>()
                .Property(e => e.No)
                .IsUnicode(false);

            modelBuilder.Entity<AllBookingsView>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<AllBookingsView>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<AllBookingsView>()
                .Property(e => e.Building_Letter)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AllBookingsView>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<AllBookingsView>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<AllBookingsView>()
                .Property(e => e.User_Name)
                .IsUnicode(false);

            modelBuilder.Entity<AllBookingsView>()
                .Property(e => e.User_Email)
                .IsUnicode(false);
            modelBuilder.Entity<RoomsView>()
                .Property(e => e.RoomName)
                .IsUnicode(false);

            modelBuilder.Entity<RoomsView>()
                .Property(e => e.No)
                .IsUnicode(false);

            modelBuilder.Entity<RoomsView>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<RoomsView>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<RoomsView>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<RoomsView>()
                .Property(e => e.Building_Letter)
                .IsFixedLength()
                .IsUnicode(false);

        }
    }
}
