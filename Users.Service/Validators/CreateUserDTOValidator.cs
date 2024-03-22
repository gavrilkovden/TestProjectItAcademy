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
            RuleFor(e => e.Login).MinimumLength(10).MaximumLength(50).NotEmpty();
            RuleFor(e => e.Password).MinimumLength(8).MaximumLength(50).NotEmpty();
        }
    }
}
