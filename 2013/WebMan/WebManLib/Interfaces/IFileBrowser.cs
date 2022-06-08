using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rn.WebManLib.Externals.FileBrowser;

namespace Rn.WebManLib.Interfaces
{
    [ServiceContract]
    interface IFileBrowser
    {
        [OperationContract]
        string HelloWorld();

        [OperationContract]
        List<SoapDriveInfo> ListDrives();

        [OperationContract]
        List<SoapDirectoryInfo> ListFolders(string path);
    }
}
