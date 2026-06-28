namespace Domain.Entities
{
    public class Difficulty : BaseEntity
    {
        public string Name { get; set; }

        public virtual HashSet<Room> Rooms { get; set; } = new HashSet<Room>();
    }
}
