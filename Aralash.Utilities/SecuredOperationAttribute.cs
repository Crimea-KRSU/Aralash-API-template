namespace Aralash.Utilities;

[AttributeUsage(AttributeTargets.Class)]
public class SecuredOperationAttribute : Attribute
{
    public string? Description { get; }

    public SecuredOperationAttribute(string? description)
    {
        Description = description;
    }
}