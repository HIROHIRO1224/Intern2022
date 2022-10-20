namespace PushHistory;
class THistory
{
    public THistory(string id,int userId,int roomId,string inOrOut){
        Id=id;
        UserId=userId;
        RoomId=roomId;
        InOrOut=inOrOut;
    }
    string Id{get;set;}
    int UserId{get;set;}
    int RoomId{get;set;}
    string InOrOut{get;set;}

}