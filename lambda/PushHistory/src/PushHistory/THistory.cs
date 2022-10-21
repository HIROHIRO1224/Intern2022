namespace PushHistory;
public class THistory
{
    public THistory(string id, int userId, int roomId, string inOrOut)
    {
        Id = id;
        UserId = userId;
        RoomId = roomId;
        InOrOut = inOrOut;
    }
    public string Id { get; set; }
    public int UserId { get; set; }
    public int RoomId { get; set; }
    public string InOrOut { get; set; }

}