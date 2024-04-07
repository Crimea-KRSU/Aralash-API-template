namespace Aralash.App.Auth.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username).Matches("^[a-zA-Z0-9_-]{4,32}$").WithMessage("Длина логина от 4 до 32 символов");
        RuleFor(x => x.Firstname).MaximumLength(50).WithMessage("В имени должно быть не более 50 символов");
        RuleFor(x => x.Lastname).MaximumLength(50).WithMessage("В фамилии должно быть не более 50 символов");
        RuleFor(x => x.Patronymic).MaximumLength(50).WithMessage("В отчестве должно быть не более 50 символов");
        
        RuleFor(x => x.Password)
            .MinimumLength(8).WithMessage("Пароль должен быть минимум 8 символов");

        RuleFor(x => x.Password)
            .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.");

        RuleFor(x => x.Password)
            .Matches("[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.");

        RuleFor(x => x.Password)
            .Matches("\\d").WithMessage("Пароль должен содержать хотя бы одну цифру.");

        // ту мач?
        // RuleFor(x => x.Password)
        //     .Matches("[@$!%*?&]").WithMessage("Включать хотя бы один из следующих специальных символов");
    }
}