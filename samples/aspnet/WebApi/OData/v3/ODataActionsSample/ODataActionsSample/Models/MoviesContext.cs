using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ODataActionsSample.Models
{
    public class MoviesContext : DbContext
    {
        static MoviesContext()
        {
            Database.SetInitializer(new MoviesInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Set the TimeStamp property to be an optimistic concurrency token.
            // EF will use this to detect concurrency conflicts.

            modelBuilder.Entity<Movie>()
                .Property(m => m.TimeStamp)
                .IsConcurrencyToken()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Movie> Movies { get; set; }
    }

    public class MoviesInitializer : DropCreateDatabaseAlways<MoviesContext>
    {
        protected override void Seed(MoviesContext context)
        {
            List<Movie> movies = new List<Movie>()
            {
                new Movie() { Title = "Maximum Payback", Year = 1990 },
                new Movie() { Title = "Inferno of Retribution", Year = 2005 },
                new Movie() { Title = "Fatal Vengeance 2", Year = 2012 },
                new Movie() { Title = "Sudden Danger", Year = 2012 },
                new Movie() { Title = "Deadly Honor IV", Year = 1977 }
            };
            movies.ForEach(m => context.Movies.Add(m));
            context.SaveChanges();
        }
    }
}