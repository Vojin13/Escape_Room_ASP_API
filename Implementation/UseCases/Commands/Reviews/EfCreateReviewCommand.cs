using Application.Commands.Reviews;
using Application.DTO.Reviews;
using Application.Exceptions;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Implementation.UseCases.Validators.Reviews;
using System;
using System.Linq;

namespace Implementation.UseCases.Commands.Reviews
{
    public class EfCreateReviewCommand : EfUseCase, ICreateReviewCommand
    {
        private readonly CreateReviewValidator _validator;

        public EfCreateReviewCommand(AppDbContext context, CreateReviewValidator validator) : base(context)
        {
            _validator = validator;
        }

        public string Name => "Create Review";

        public string Id => "create-review";

        public void Execute(CreateReviewDTO data)
        {
            _validator.ValidateAndThrow(data);

            var booking = _ctx.Bookings.FirstOrDefault(b => b.UserId == data.UserId
                                                          && b.RoomId == data.RoomId
                                                          && b.StatusId == (int)BookingStatus.Completed);

            if(booking == null)
            {
                throw new ConflictException("You can only review rooms you've completed.");
            }

            if(booking.BookingDate.Date >= DateTime.UtcNow.Date)
            {
                throw new ConflictException("You can only review a room after your booking date has passed.");
            }

            var review = new Review
            {
                UserId = data.UserId,
                RoomId = data.RoomId,
                Rating = data.Rating,
                Comment = data.Comment
            };

            _ctx.Reviews.Add(review);
            _ctx.SaveChanges();
        }
    }
}
