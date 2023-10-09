using Application.Dto.Post;
using FluentValidation;

namespace Application.Validators.Post
{
    public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostDtoValidator()
        {
            #region Title

            RuleFor(x => x.Title).NotEmpty().WithMessage("Post can not have an empty title");
            RuleFor(x => x.Title).Length(5, 100).WithMessage("The title must be between 5 and 100 characters long.");

            #endregion Title

            #region Content

            RuleFor(x => x.Content).NotEmpty().WithMessage("Post can not have an empty content");
            RuleFor(x => x.Content).Length(0, 2000).WithMessage("The content can be at most 2000 characters long.");

            #endregion Content
        }
    }
}