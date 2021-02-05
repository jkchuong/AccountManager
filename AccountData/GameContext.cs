using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountData
{
    public partial class GameContext : DbContext
    {
        public static GameContext Instance { get; } = new GameContext();

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
