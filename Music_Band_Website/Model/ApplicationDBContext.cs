using Microsoft.EntityFrameworkCore;

namespace Music_Band_Website.Model
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Music_Type> Music_Types { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Event_Playlist> Event_Playlists { get; set; }
        public DbSet<Liked_Song> Liked_Songs { get; set; }
        public DbSet<Event_Tickets> Event_Tickets { get; set; }

        public ApplicationDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event_Tickets>()
        .HasKey(et => new { et.Id_Event, et.Id_User });

            modelBuilder.Entity<Song>()
                .HasOne(s => s.Music_Type)
                .WithMany()
                .HasForeignKey(s => s.Id_Type)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Music_Type>().ToTable("Music_Type");

            //C_P
            modelBuilder.Entity<Event_Playlist>()
                .HasKey(cp => new { cp.Id_Event, cp.Id_Song });

            modelBuilder.Entity<Event_Playlist>()
                .HasOne(cp => cp.Event)
                .WithMany()
                .HasForeignKey(cp => cp.Id_Event);

            modelBuilder.Entity<Event_Playlist>()
                .HasOne(cp => cp.Song)
                .WithMany()
                .HasForeignKey(cp => cp.Id_Song);

            //S_C
            modelBuilder.Entity<Liked_Song>()
                .HasKey(sc => new { sc.Id_Song, sc.Id_User });

            modelBuilder.Entity<Liked_Song>()
                .HasOne(sc => sc.Song)
                .WithMany()
                .HasForeignKey(sc => sc.Id_Song);

            modelBuilder.Entity<Liked_Song>()
                .HasOne(sc => sc.User)
                .WithMany()
                .HasForeignKey(sc => sc.Id_User);
        }
    }
}
