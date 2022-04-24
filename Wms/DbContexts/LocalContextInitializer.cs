using Wms.Models.Data.LocalShared;

namespace Wms.DbContexts;

public static class LocalContextInitializer
{
    public static void Initialize(LocalContext context)
    {
        InitializeLocalRole(context);

        InitializeLocalUser(context);
    }

    private static void InitializeLocalRole(LocalContext context)
    {
        if (context.LocalRoles.Any())
        {
            return;   // DB has been seeded
        }

        var localRoles = new LocalRole[]
        {
            new LocalRole { Name = "Application:Administrator"},
            new LocalRole { Name = "Application:User"},
        };

        context.LocalRoles.AddRange(localRoles);

        context.SaveChanges();
    }

    private static void InitializeLocalUser(LocalContext context)
    {
        if (context.LocalUsers.Any())
        {
            return;   // DB has been seeded
        }

        var administratorRoles = context.LocalRoles.Where(r => r.Name == "Application:Administrator").ToList();
        var userRoles = context.LocalRoles.Where(r => r.Name == "Application:User").ToList();


        var localUsers = new LocalUser[]
        {
            new LocalUser{Username = "zhixian", Password = "password", Roles=administratorRoles},
            new LocalUser{Username = "mino", Password = "password", Roles=userRoles},
            new LocalUser{Username = "anand", Password = "password", Roles=userRoles},
            new LocalUser{Username = "alonso", Password = "password", Roles=userRoles},
            new LocalUser{Username = "meredith", Password = "password", Roles=userRoles},
            new LocalUser{Username = "yan", Password = "password", Roles=userRoles},
            new LocalUser{Username = "peggy", Password = "password", Roles=userRoles},
            new LocalUser{Username = "laura", Password = "password", Roles=userRoles},
            new LocalUser{Username = "norman", Password = "password", Roles=userRoles},
        };

        context.LocalUsers.AddRange(localUsers);

        context.SaveChanges();
    }
}
