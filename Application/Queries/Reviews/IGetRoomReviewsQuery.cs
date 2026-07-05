using Application.DTO;
using Application.DTO.Reviews;
using Application.DTO.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Reviews
{
    public interface IGetRoomReviewsQuery : IQuery<ReviewSearchDTO, PagedResponse<ReviewDTO>>
    {
    }
}
