using MainAPI.Models;
using MainAPI.Models.Comment.Spyder;
using MainAPI.Models.Petition.Spyder;
using MainAPI.Models.Spyder;
using MainAPI.Models.Spyder.Feature;
using MainAPI.Models.Spyder.Hall;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainAPI.Data
{
    public class MainAPIContext : DbContext
    {
        public MainAPIContext() { }
        public MainAPIContext(DbContextOptions<MainAPIContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>().Property(o => o.Spy).HasPrecision(14, 2);
            modelBuilder.Entity<Wallet>().Property(o => o.Gem).HasPrecision(14, 2);
            modelBuilder.Entity<Wallet>().Property(o => o.LegTwoPercentage).HasPrecision(14, 2);
            modelBuilder.Entity<Wallet>().Property(o => o.LegOnePercentage).HasPrecision(14, 2);
            modelBuilder.Entity<Wallet>().Property(o => o.Bonus).HasPrecision(14, 2);
            modelBuilder.Entity<Wallet>().Property(o => o.ActivationCost).HasPrecision(14, 2);
            modelBuilder.Entity<Wallet>().Property(o => o.Ref).HasPrecision(14, 2);
            modelBuilder.Entity<Petition>().Property(o => o.PetitionCost).HasPrecision(14, 2);
            modelBuilder.Entity<Transaction>().Property(o => o.Amount).HasPrecision(14, 2);
            modelBuilder.Entity<Transaction>().Property(o => o.NetworkFee).HasPrecision(14, 2);
            modelBuilder.Entity<Hall>().Property(o => o.Cost).HasPrecision(14, 2);
            modelBuilder.Entity<Payment>().Property(o => o.Amount).HasPrecision(14, 2);
            modelBuilder.Entity<Withdrawal>().Property(o => o.Amount).HasPrecision(14, 2);
            modelBuilder.Entity<Area>().Property(o => o.ActivationCost).HasPrecision(14, 2);
            modelBuilder.Entity<Area>().Property(o => o.FirstLegPercentage).HasPrecision(14, 2);
            modelBuilder.Entity<Area>().Property(o => o.SecondLegPercentage).HasPrecision(14, 2);
            modelBuilder.Entity<Sos>().Property(o => o.Latitude).HasPrecision(14, 2);
            modelBuilder.Entity<Sos>().Property(o => o.Longitude).HasPrecision(14, 2);
            modelBuilder.Entity<SosCluster>().Property(o => o.Longitude).HasPrecision(14, 2);
            modelBuilder.Entity<SosCluster>().Property(o => o.Latitude).HasPrecision(14, 2);
            modelBuilder.Entity<SosCluster>().Property(o => o.DistanceAwayFromSos).HasPrecision(14, 2);
            modelBuilder.Entity<LocationCenter>().Property(o => o.Latitude).HasPrecision(14, 2);
            modelBuilder.Entity<LocationCenter>().Property(o => o.Longitude).HasPrecision(14, 2);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<Petition> Petitions { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Hall> Halls { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<FlagReport> FlagReports { get; set; }
        public virtual DbSet<Death> Deaths { get; set; }
        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<FeatureGroup> FeatureGroups { get; set; }
        public virtual DbSet<FeatureType> FeatureTypes { get; set; }
        public virtual DbSet<Marriage> Marriages { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        public virtual DbSet<Missing> Missings { get; set; }
        public virtual DbSet<Confession> Confessions { get; set; }
        public virtual DbSet<Inbox> Inboxes { get; set; }
        public virtual DbSet<LogInMonitor> LogInMonitors { get; set; }
        public virtual DbSet<SystemLog> SystemLogs { get; set; } 
        public virtual DbSet<MajorFunctionLog> MajorFunctionLogs { get; set; } 

        public virtual DbSet<StatusEnum> General_StatusEnums { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Sos> Sos { get; set; }
        public virtual DbSet<SosCluster> SosClusters { get; set; }
        public virtual DbSet<LocationCenter> LocationCenters { get; set; }
        public virtual DbSet<Withdrawal> Withdrawals { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Params> Params { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }


    }
}
