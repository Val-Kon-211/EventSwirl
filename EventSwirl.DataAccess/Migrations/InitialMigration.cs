using FluentMigrator;

namespace EventSwirl.DataAccess.Migrations
{
    [Migration(0000000000, "Initial migration")]
    public class InitialMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Insert.IntoTable("EventCategories").Row(new { Name = "Sport", Description = "", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            Insert.IntoTable("EventCategories").Row(new { Name = "Food", Description = "", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            Insert.IntoTable("EventCategories").Row(new { Name = "Family", Description = "", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            Insert.IntoTable("EventCategories").Row(new { Name = "Music", Description = "", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            Insert.IntoTable("EventCategories").Row(new { Name = "Art", Description = "", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            Insert.IntoTable("EventCategories").Row(new { Name = "Technology", Description = "", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            Insert.IntoTable("EventCategories").Row(new { Name = "Study", Description = "", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            Insert.IntoTable("EventCategories").Row(new { Name = "Business", Description = "", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            Insert.IntoTable("EventCategories").Row(new { Name = "MasterClass", Description = "", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });

            Insert.IntoTable("UserRoles").Row(new { Name = "admin", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
            Insert.IntoTable("UserRoles").Row(new { Name = "manager", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now });
        }
    }
}
