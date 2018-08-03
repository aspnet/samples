namespace TouristAttractions.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    using TouristAttractions.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TourismContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TouristAttractions.Models.TourismContext context)
        {
            context.TouristAttractions.AddOrUpdate(a => a.Name,
                new TouristAttraction
                {
                    Name = "Space Needle, Seattle",
                    Location = DbGeography.FromText("POINT(-122.348670959473 47.619930267334)")
                });

            context.TouristAttractions.AddOrUpdate(a => a.Name,
                new TouristAttraction
                {
                    Name = "Pike Place Market, Seattle",
                    Location = DbGeography.FromText("POINT(-122.341697692871 47.6094245910645)")
                });

            context.TouristAttractions.AddOrUpdate(a => a.Name,
                new TouristAttraction
                {
                    Name = "Statue of Liberty, NY",
                    Location = DbGeography.FromText("POINT(-74.0439682006836 40.6886405944824)")
                });
        }
    }
}
