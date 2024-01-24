using db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreMultiDB.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly SqlServerDbContext _sqlServerDbContext;
    private readonly PostgreSqlDbContext _postgreSqlDbContext;

    public UsersController(
        SqlServerDbContext sqlServerDbContext,
        PostgreSqlDbContext postgreSqlDbContext
    )
    {
        _sqlServerDbContext = sqlServerDbContext;
        _postgreSqlDbContext = postgreSqlDbContext;
    }

    [HttpGet("search/{name}")]
    public ActionResult Search(string name)
    {
        var userFromSqlServer = _sqlServerDbContext.Users.FirstOrDefault(u => u.Name == name);

        var userFromPostgreSql = _postgreSqlDbContext.Users.FirstOrDefault(
            u => EF.Functions.ILike(u.Name, name)
        );

        return Ok(
            new { UserFromSqlServer = userFromSqlServer, UserFromPostgreSql = userFromPostgreSql }
        );
    }
}
