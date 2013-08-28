namespace Croom.Model
{
    public class MeetingRoomStatus
    {
        public bool IsOccupied { get; set; }
        public Reservation CurrentReservation { get; set; }
    }
}
