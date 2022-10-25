using Amazon.CDK.AWS.Lambda;
using Constructs;
namespace Intern202201AwsCdk
{
    public class ShowUserInRoomFunction
    {
        const string handler="ShowUserInRoom::ShowUserInRoom.Function::FunctionHandler";
        const string code="./lambda/ShowUserInRoom/src/ShowUserInRoom/bin/Debug/net6.0/publish";
        public Function function{get;}

        public ShowUserInRoomFunction(Construct construct){
            this.function=new MyFunction(construct,"ShowUserInRoom",handler,code).function;
        }
    }
}
