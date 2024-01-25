using db;
using db.models;
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

        // var userFromPostgreSql = _postgreSqlDbContext.Users.FirstOrDefault(u => u.Name == name);
        var userFromPostgreSql = _postgreSqlDbContext.Users.FirstOrDefault(
            u => EF.Functions.ILike(u.Name, name)
        );

        return Ok(
            new { UserFromSqlServer = userFromSqlServer, UserFromPostgreSql = userFromPostgreSql }
        );
    }

    [HttpPost("create")]
    public ActionResult Create(string name)
    {
        var user = new User() { Name = name };
        _sqlServerDbContext.Users.Add(user);
        _sqlServerDbContext.SaveChanges();

        _postgreSqlDbContext.Users.Add(user);
        _postgreSqlDbContext.SaveChanges();

        return Ok();
    }

    [HttpPost("book")]
    public ActionResult Book(string userName, string bookName)
    {
        var userFromSqlServer = _sqlServerDbContext.Users.FirstOrDefault(u => u.Name == userName);

        if (userFromSqlServer != null)
        {
            var book = new Book() { Title = bookName, User = userFromSqlServer };
            _sqlServerDbContext.Books.Add(book);
            _sqlServerDbContext.SaveChanges();
        }

        var userFromPostgreSql = _postgreSqlDbContext.Users.FirstOrDefault(
            u => EF.Functions.ILike(u.Name, userName)
        );

        if (userFromPostgreSql != null)
        {
            var book = new Book() { Title = bookName, User = userFromPostgreSql };
            _postgreSqlDbContext.Books.Add(book);
            _postgreSqlDbContext.SaveChanges();
        }

        return Ok();
    }

    [HttpGet("all")]
    public ActionResult All()
    {
        var usersFromSqlServer = _sqlServerDbContext.Users.ToList();
        var usersFromPostgreSql = _postgreSqlDbContext.Users.ToList();

        return Ok(
            new
            {
                UsersFromSqlServer = usersFromSqlServer,
                UsersFromPostgreSql = usersFromPostgreSql
            }
        );
    }

    [HttpDelete("delete/{id}")]
    public ActionResult Delete(int id)
    {
        var userFromSqlServer = _sqlServerDbContext.Users.FirstOrDefault(u => u.Id == id);

        if (userFromSqlServer != null)
        {
            _sqlServerDbContext.Users.Remove(userFromSqlServer);
            _sqlServerDbContext.SaveChanges();
        }

        var userFromPostgreSql = _postgreSqlDbContext.Users.FirstOrDefault(u => u.Id == id);

        if (userFromPostgreSql != null)
        {
            _postgreSqlDbContext.Users.Remove(userFromPostgreSql);
            _postgreSqlDbContext.SaveChanges();
        }

        return Ok();
    }
}
