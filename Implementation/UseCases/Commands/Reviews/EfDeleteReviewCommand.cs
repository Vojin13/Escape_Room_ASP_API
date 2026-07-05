using Application.Commands.Reviews;
using Application.Exceptions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.UseCases.Commands.Reviews
{
    public class EfDeleteReviewCommand : EfUseCase, IDeleteReviewCommand
    {
        public EfDeleteReviewCommand(AppDbContext context) : base(context)
        {
        }

        public string Name => "Delete Review";

        public string Id => "delete-review";

        public void Execute(int data)
        {
            var review = _ctx.Reviews.Find(data);

            if(review == null)
            {
                throw new NotFoundException("Review", data);
            }

            _ctx.Reviews.Remove(review);
            _ctx.SaveChanges();
        }
    }
}
