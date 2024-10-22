using Alper.Application.Commands.UserCmds;
using Application.Commands.UserCmds;
using Common.Utils;
using FluentValidation;

namespace WebApi.Validators.Employees
{
    public class CreateUserVld : AbstractValidator<CreateUserCmd>
    {
        public CreateUserVld()
        {
            //RuleFor(p => p.IslemYapanKullanici)
            //.NotEmpty().WithMessage("İşlem yapan kullanıcı e-posta adresi boş.")
            //.Must(CheckIf.ValidEMail).WithMessage("İşlem yapan kullanıcı e-posta adresi hatalı.");

            RuleFor(p => p.NewUserCmdDto.Email)
                .NotEmpty().WithMessage("E-posta boş.")
                .EmailAddress().WithMessage("E-posta hatalı");

            RuleFor(p => p.NewUserCmdDto.Name)
                .NotEmpty().WithMessage("Adı boş.")
                .MaximumLength(50).WithMessage("Adı en fazla 50 karakter uzunluğunda olabilir.");

            RuleFor(p => p.NewUserCmdDto.Surname)
                .NotEmpty().WithMessage("Soyadı boş.")
                .MaximumLength(50).WithMessage("Soyadı en fazla 50 karakter uzunluğunda olabilir.");

            RuleFor(p => p.NewUserCmdDto.PhoneNumber)
                .Must(CheckIf.ValidKullaniciTelefonu).WithMessage("Telefon numarası hatalı")
                .When(p => !string.IsNullOrEmpty(p.NewUserCmdDto.PhoneNumber));

            RuleFor(p => p.NewUserCmdDto.BirthDate)
                .LessThan(DateTime.Today).WithMessage("Doğum tarihi bugünden küçük olmalıdır.")
                .When(p => p.NewUserCmdDto.BirthDate.HasValue);

            RuleFor(p => p.NewUserCmdDto.StartJobDate)
                .GreaterThanOrEqualTo(p => p.NewUserCmdDto.BirthDate)
                .WithMessage("İşe başlama tarihi doğum tarihinden sonra olmalıdır.")
                .When(p => p.NewUserCmdDto.StartJobDate.HasValue && p.NewUserCmdDto.BirthDate.HasValue);
        }
    }
}
