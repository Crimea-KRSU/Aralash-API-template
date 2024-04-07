namespace Aralash.Domain.Constants;

/// <summary>
/// Команда пустышка.
/// <para/>
/// Определяет абсолютную защищенную операцию, которая наделяет юзера правами супер-админа
/// </summary>
public class SuperAdminCommand
{
    public const string Name = nameof(SuperAdminCommand);
    public const string Description = "Абсолютные права в системе";
}