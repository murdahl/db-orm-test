using db.models;
using Microsoft.EntityFrameworkCore;

namespace db;

public class SqlServerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }

    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options) { }
}

public class PostgreSqlDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }

    public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) : base(options) { }
}
