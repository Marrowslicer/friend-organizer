using System.Collections.Generic;

using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        public IEnumerable<Friend> GetAll()
        {
            // TODO: Load data from real database
            yield return new Friend { FirstName = "Stas", LastName = "Kazakov" };
            yield return new Friend { FirstName = "Olga", LastName = "Kazakova" };
            yield return new Friend { FirstName = "Roma", LastName = "Kleschik" };
            yield return new Friend { FirstName = "Artem", LastName = "Curikov" };
        }
    }
}
