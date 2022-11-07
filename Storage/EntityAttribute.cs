namespace Storage;

[AttributeUsage(AttributeTargets.Class)]
public class EntityAttribute : Attribute
{
    public readonly string Name;

    public EntityAttribute(string name)
    {
        Name = name;
    }
}

public class MissingEntityAttributeException : Exception
{
    internal MissingEntityAttributeException(string msg) : base(msg)
    {
    }
}