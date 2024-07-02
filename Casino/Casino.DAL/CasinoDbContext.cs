using Microsoft.EntityFrameworkCore;
using Casino.Model;
namespace Casino.DAL
{
    public class CasinoDbContext : DbContext
    {
        public DbSet<Transactions> Transactions {  get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Result> Results{ get; set; }
        public DbSet<Game> Games{ get; set; }
        public DbSet<Bandit> Bandits { get; set; }
        public DbSet<Roulette> Roulettes{ get; set; }
      

        public CasinoDbContext(DbContextOptions<CasinoDbContext> dbContextOptions):base(dbContextOptions) { }
        public CasinoDbContext():base() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Game).Assembly);
        }
    }
}