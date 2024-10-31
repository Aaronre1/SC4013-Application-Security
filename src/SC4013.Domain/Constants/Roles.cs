namespace SC4013.Domain.Constants;

public abstract class Roles
{
    /// <summary>
    /// Can perform all actions
    /// </summary>
    public const string Admin = nameof(Admin);

    /// <summary>
    /// Allows assignment of tasks to other users
    /// </summary>
    public const string Leader = nameof(Leader);

    /// <summary>
    /// Can create and manage tasks, cannot delete entire list
    /// </summary>
    public const string Manager = nameof(Manager);
}