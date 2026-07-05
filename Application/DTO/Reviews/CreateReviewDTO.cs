namespace Application.DTO.Reviews
{
    public class CreateReviewDTO
    {
        public int RoomId { get; set; }
        public byte Rating { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
    }
}
