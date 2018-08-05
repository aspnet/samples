namespace TouristAttractions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TouristAttractions",
                c => new
                    {
                        TouristAttractionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.Geography(),
                    })
                .PrimaryKey(t => t.TouristAttractionId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        Author = c.String(),
                        Comments = c.String(),
                        Rating = c.Int(nullable: false),
                        TouristAttractionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.TouristAttractions", t => t.TouristAttractionId, cascadeDelete: true)
                .Index(t => t.TouristAttractionId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Reviews", new[] { "TouristAttractionId" });
            DropForeignKey("dbo.Reviews", "TouristAttractionId", "dbo.TouristAttractions");
            DropTable("dbo.Reviews");
            DropTable("dbo.TouristAttractions");
        }
    }
}
