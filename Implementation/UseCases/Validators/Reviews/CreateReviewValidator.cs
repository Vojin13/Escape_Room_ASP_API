using Application.DTO.Reviews;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.UseCases.Validators.Reviews
{
    public class CreateReviewValidator : BaseValidator<CreateReviewDTO>
    {
        public CreateReviewValidator(AppDbContext ctx)
        {
            RuleFor(x => x.Rating)
                .NotEmpty().WithMessage(Required)
                .InclusiveBetween((byte)1, (byte)5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .WithMessage("Comment cannot exceed 1000 characters.");

            RuleFor(x => x)
                .Must(dto => !ctx.Reviews.Any(r => r.UserId == dto.UserId && r.RoomId == dto.RoomId))
                .WithMessage("You have already reviewed this room.");
        }
    }
}
