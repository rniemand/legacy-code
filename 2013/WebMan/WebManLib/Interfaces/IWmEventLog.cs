using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Rn.WebManLib.Externals.WmEventLog;

namespace Rn.WebManLib.Interfaces
{
    [ServiceContract]
    interface IWmEventLog
    {
        [OperationContract]
        List<SoapEventLog> ListEventLogs(WmAuthApi authInfo);

        [OperationContract]
        int GetEventCount(WmAuthApi authInfo, string eventlogName);

        [OperationContract]
        SoapEventLog GetEventLogInfo(WmAuthApi authInfo, string eventlogName);

        [OperationContract]
        List<SoapEventLogEntry> GetEventLogEntries(WmAuthApi authInfo, string eventlogName, int numOfEntries, bool includeMessageInfo = false);

        [OperationContract]
        SoapEventLogEntry GetEvent(WmAuthApi authInfo, string eventlogName, int eventId);

    }
}
