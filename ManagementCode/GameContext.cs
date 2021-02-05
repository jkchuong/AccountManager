using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ManagementCode
{
    public partial class GameContext : DbContext
    {
        public static GameContext Instance { get; set; } = new GameContext();

        public DbSet<User> Users { get; set; }
        public DbSet<Theme> Themes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = Game;");
            }
        }
    }
}
