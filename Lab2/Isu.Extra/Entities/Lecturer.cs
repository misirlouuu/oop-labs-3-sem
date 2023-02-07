namespace Isu.Extra.Entities;

public class Lecturer
{
    public Lecturer(string subject, string name)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentNullException(subject);
        Subject = subject;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(name);
        Name = name;

        Id = Guid.NewGuid();
    }

    public string Subject { get; }
    public string Name { get; }
    public Guid Id { get; }
}