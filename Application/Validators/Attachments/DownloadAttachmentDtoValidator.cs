using Application.Dto.Attachments;
using FluentValidation;

namespace Application.Validators.Attachments
{
    public class DownloadAttachmentDtoValidator : AbstractValidator<DownloadAttachmentDto>
    {
        public DownloadAttachmentDtoValidator()
        {
            #region Name

            RuleFor(x => x.Name).NotEmpty().WithMessage("The name of the attachment must be given");
            RuleFor(x => x.Name).Length(3, 100).WithMessage("The length of the picture name must be more than 3 characters, but must not exceed 100.");

            #endregion Name

            #region Content

            RuleFor(x => x.Content).NotEmpty().WithMessage("the contents of the attachment must not be empty");

            #endregion Content
        }
    }
}