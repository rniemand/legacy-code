using System.ServiceModel;

namespace Rn.WebManLib.Interfaces
{
    [ServiceContract]
    interface IServices
    {
        [OperationContract]
        string HelloWorld();
    }
}
