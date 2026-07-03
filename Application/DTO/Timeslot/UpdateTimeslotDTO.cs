namespace Application.DTO.Timeslot
{
    public class UpdateTimeslotDTO
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
