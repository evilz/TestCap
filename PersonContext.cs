using Microsoft.EntityFrameworkCore;

namespace Test.CAP;

public class PersonContext : DbContext
{
    public PersonContext(DbContextOptions<PersonContext> options) : base(options) { }

    public DbSet<Person> People { get; set; }
}

public class Person
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
}