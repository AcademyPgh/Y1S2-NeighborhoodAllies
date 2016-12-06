using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using ImpactMap.Models;

namespace ImpactMap.Utils
{
    public class Utility
    {
        private Models.ImpactMapDbContext db = new Models.ImpactMapDbContext();

        public int UserID(System.Security.Principal.IPrincipal User1)
        {
            Guid userGuid;
            if (Guid.TryParse(User1.Identity.GetUserId(), out userGuid))
            {
                var user = db.users.Where(u => u.userModelGuid == userGuid).SingleOrDefault();
                if (user == null)
                {
                    // valid guid but no user in the table? that means a user should be created!
                    User newUser = new Models.User();
                    newUser.userModelGuid = userGuid;
                    newUser.userModelName = User1.Identity.Name;
                    db.users.Add(newUser);
                    db.SaveChanges();
                    return newUser.ID;
                }
                else
                {
                    return user.ID;
                }
            }
            else
            {
                return 1; // default user, will be impossible to hit once I turn on authentication (supposedly)
            }
        }
    }
}