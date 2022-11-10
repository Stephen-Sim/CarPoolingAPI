namespace CarPoolingAPI.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int PassengerId { get; set; }
        public Request Request { get; set; }
        public Passenger Passenger { get; set; }
        public ICollection<Chat> Chats { get; set; }
    }
}
