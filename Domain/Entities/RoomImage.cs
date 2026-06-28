namespace Domain.Entities
{
    public class RoomImage : BaseEntity
    {
        public string Path { get; set; }
        public string MimeType { get; set; }
        public int Size { get; set; }
        public bool IsPrimary { get; set; }
        public int SortOrder { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
    }
}
