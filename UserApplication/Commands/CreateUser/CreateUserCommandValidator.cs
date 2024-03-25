using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApplication.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(e => e.Login).MinimumLength(10).MaximumLength(50).NotEmpty();
            RuleFor(e => e.Password).MinimumLength(8).MaximumLength(50).NotEmpty();
        }
    }
}
