using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Data
{
    public class CinemaContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        
        public DbSet<Auditorium> Auditoria { get; set; }
        public DbSet<Seat> Seats { get; set; }
        
        public DbSet<Address> Addresses { get; set; }
        
        public DbSet<MovieParticipant> MovieParticipants { get; set; }
        
        public DbSet<Participant> Participants { get; set; }
        
        public DbSet<Ticket> Tickets { get; set; }

        public CinemaContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Seat>()
                .HasOne(x => x.Auditorium)
                .WithMany(x => x.Seats)
                .HasForeignKey(x => x.AuditoriumId)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .HasMany(x => x.Cinemas)
                .WithOne(x => x.Address);

            modelBuilder.Entity<Auditorium>()
                .HasMany(x => x.Seats)
                .WithOne(x => x.Auditorium);

            modelBuilder.Entity<Auditorium>()
                .HasMany(x => x.Projections)
                .WithOne(x => x.Auditorium);

            modelBuilder.Entity<Auditorium>()
                .HasOne(x => x.Cinema)
                .WithMany(x => x.Auditoria)
                .HasForeignKey(x => x.CinemaId);

            modelBuilder.Entity<Cinema>()
                .Property(x => x.Id).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Cinema>()
                .HasMany(x => x.Auditoria)
                .WithOne(x => x.Cinema);

            modelBuilder.Entity<Cinema>()
                .HasOne(x => x.Address)
                .WithMany(x => x.Cinemas)
                .HasForeignKey(x => x.AddressId);
            
            modelBuilder.Entity<Movie>()
                .HasMany(x => x.MovieParticipants)
                .WithOne(x => x.Movie);

            modelBuilder.Entity<Movie>()
                .HasMany(x => x.Projections)
                .WithOne(x => x.Movie);

            modelBuilder.Entity<MovieParticipant>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<MovieParticipant>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MovieParticipants)
                .HasForeignKey(x => x.MovieId);

            modelBuilder.Entity<MovieParticipant>()
                .HasOne(x => x.Participant)
                .WithMany(x => x.MovieParticipants)
                .HasForeignKey(x => x.ParticipantId);

            modelBuilder.Entity<Participant>()
                .HasMany(x => x.MovieParticipants)
                .WithOne(x => x.Participant);

            modelBuilder.Entity<Projection>()
                .HasOne(x => x.Auditorium)
                .WithMany(x => x.Projections)
                .HasForeignKey(x => x.AuditoriumId);

            modelBuilder.Entity<Projection>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.Projections)
                .HasForeignKey(x => x.MovieId);

            modelBuilder.Entity<Ticket>()
                .HasOne(x => x.Seat);

            modelBuilder.Entity<Ticket>()
                .HasOne(x => x.Projection)
                .WithMany(x => x.Tickets)
                .HasForeignKey(x => x.ProjectionId);

            modelBuilder.Entity<Ticket>()
                .HasOne(x => x.User)
                .WithMany(x => x.Tickets)
                .HasForeignKey(x => x.UserId);

        }
    }
}
