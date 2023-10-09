using Application.Dto.Picture;
using FluentValidation;

namespace Application.Validators.Picture
{
    public class PictureDtoValidator : AbstractValidator<PictureDto>
    {
        public PictureDtoValidator()
        {
            #region Id

            RuleFor(x => x.Id).NotEmpty().WithMessage("Need map ID and appropriate permissions");

            #endregion Id

            #region Name

            RuleFor(x => x.Name).NotEmpty().WithMessage("The name of the picture must be given");
            RuleFor(x => x.Name).Length(3, 100).WithMessage("The length of the picture name must be more than 3 characters, but must not exceed 100.");

            #endregion Name

            #region Image

            RuleFor(x => x.Image).NotEmpty().WithMessage("The image does not contain any byte array.");

            #endregion Image
        }
    }
}