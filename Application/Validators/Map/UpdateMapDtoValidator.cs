using Application.Dto.Map;
using FluentValidation;

namespace Application.Validators.Map
{
    public class UpdateMapDtoValidator : AbstractValidator<UpdateMapDto>
    {
        public UpdateMapDtoValidator()
        {
            #region ID

            RuleFor(x => x.Id).NotEmpty().WithMessage("Need map ID and appropriate permissions");

            #endregion ID

            #region Name

            RuleFor(x => x.Name).NotEmpty().WithMessage("The name of the map must be given");
            RuleFor(x => x.Name).Length(3, 30).WithMessage("The length of the map name must be more than 3 characters, but must not exceed 30.");

            #endregion Name

            #region Description

            RuleFor(x => x.Description).Length(0, 2000).WithMessage("The description can be at most 2000 characters long.");

            #endregion Description
        }
    }
}