namespace FriendOrganizer.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    using FriendOrganizer.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizerDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.Friends.AddOrUpdate(
                f => f.FirstName,
                new Friend { FirstName = "Stas", LastName = "Kazakov" },
                new Friend { FirstName = "Olga", LastName = "Kazakova" },
                new Friend { FirstName = "Roma", LastName = "Kleschik" },
                new Friend { FirstName = "Artem", LastName = "Curikov" }
                );
            context.ProgrammingLanguages.AddOrUpdate(
                l => l.Name,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "C++" },
                new ProgrammingLanguage { Name = "C" },
                new ProgrammingLanguage { Name = "Java" },
                new ProgrammingLanguage { Name = "Python" }
                );
        }
    }
}
