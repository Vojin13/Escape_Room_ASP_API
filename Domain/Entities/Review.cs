namespace Domain.Entities
{
    public class Review : BaseEntity
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }

        public virtual User User { get; set; }
        public virtual Room Room { get; set; }
    }
}
