

namespace SearchUser;

class MyResponseBody
{
    public int Id;

    public string? Name; 

    public MyResponseBody(int id,string name){
        this.Id=id;
        this.Name=name;
    }
}