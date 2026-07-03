namespace Application.DTO.Bookings
{
    public class TimeslotLockResponseDTO
    {
        public int LockId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
