using FluentValidation;

public class ContactValidator : AbstractValidator<ContactVM>
{
    public ContactValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("This field cannot be empty").Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters and spaces.")
            .Length(2, 50).WithMessage("Name must be between 2 to 50 characters");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("This field cannot be empty")
            .EmailAddress().WithMessage("Invalid email format");


        RuleFor(user => user.Subject)            
            .NotNull().Length(2, 20);

        RuleFor(user => user.Message)
            .NotEmpty().WithMessage("This field cannot be empty");
    }
}
