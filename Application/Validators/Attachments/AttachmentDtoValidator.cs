using Application.Dto.Attachments;
using FluentValidation;

namespace Application.Validators.Attachments
{
    public class AttachmentDtoValidator : AbstractValidator<AttachmentDto>
    {
        public AttachmentDtoValidator()
        {
            #region Id

            RuleFor(x => x.Id).NotEmpty().WithMessage("There was an error with the attachment ID");

            #endregion Id

            #region Name

            RuleFor(x => x.Name).NotEmpty().WithMessage("The name of the attachment must be given");
            RuleFor(x => x.Name).Length(3, 100).WithMessage("The length of the attachment name must be more than 3 characters, but must not exceed 100.");

            #endregion Name
        }
    }
}