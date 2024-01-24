namespace db.models;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }

    // Navigation property
    public ICollection<Book> Books { get; set; } = [];
}
