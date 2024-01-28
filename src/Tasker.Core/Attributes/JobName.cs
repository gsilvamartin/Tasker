namespace Tasker.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class JobName : Attribute
{
    public string Name { get; }

    public JobName(string name)
    {
        Name = name;
    }
}