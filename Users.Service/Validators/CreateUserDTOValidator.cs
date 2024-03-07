using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Service.DTO;

namespace Users.Service.Validators
{
    public class CreateUserDTOValidator: AbstractValidator<CreateUserDTO>
    {
        public CreateUserDTOValidator()
        {
            RuleFor(e => e.UserName).NotEmpty().WithMessage("UserName error");
            RuleFor(e => e.UserName).MinimumLength(3).MaximumLength(10).Must(e => e.StartsWith("user"));
        }
    }
}
