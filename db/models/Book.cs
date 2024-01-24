namespace db.models;

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int UserId { get; set; } // Foreign key property

    // Navigation property
    public User? User { get; set; }
}
