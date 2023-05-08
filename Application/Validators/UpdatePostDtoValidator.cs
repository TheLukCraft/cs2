using Application.Dto.Post;
using FluentValidation;

namespace Application.Validators
{
    public class UpdatePostDtoValidator : AbstractValidator<UpdatePostDto>
    {
        public UpdatePostDtoValidator()
        {
            #region Content

            RuleFor(x => x.Content).NotEmpty().WithMessage("Post can not have an empty content");
            RuleFor(x => x.Content).Length(0, 2000).WithMessage("The content can be at most 2000 characters long.");

            #endregion Content
        }
    }
}