namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

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
                new Friend { FirstName = "Станислав", LastName = "Казаков" },
                new Friend { FirstName = "Ольга", LastName = "Казакова" },
                new Friend { FirstName = "Роман", LastName = "Клещик" },
                new Friend { FirstName = "Артем", LastName = "Цуриков" }
                );
            context.ProgrammingLanguages.AddOrUpdate(
                l => l.Name,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "C++" },
                new ProgrammingLanguage { Name = "C" },
                new ProgrammingLanguage { Name = "Java" },
                new ProgrammingLanguage { Name = "Python" }
                );

            context.SaveChanges();

            context.FriendPhoneNumbers.AddOrUpdate(
                n => n.Number,
                new FriendPhoneNumber { Number = "+375 44 5856254", FriendId = context.Friends.First().Id });

            context.Meetings.AddOrUpdate(
                m => m.Title,
                new Meeting
                {
                    Title = "Смотреть футбол",
                    DateFrom = new DateTime(2020, 10, 25),
                    DateTo = new DateTime(2020, 10, 25),
                    Friends = new List<Friend>
                    {
                        context.Friends.Single(f => f.FirstName == "Роман" && f.LastName == "Клещик"),
                        context.Friends.Single(f => f.FirstName == "Артем" && f.LastName == "Цуриков")
                    }
                });
        }
    }
}
