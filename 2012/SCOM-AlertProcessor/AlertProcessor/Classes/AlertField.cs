using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlertProcessor.Classes
{
    public enum AlertField
    {
        OriginalDescription,
        AlertName,
        Severity,
        Priority,
        Category,
        RaisedDateTime,
        RepeatCount,
        AlertProcessed,
        LineNo,
        LinePos,
        ScomScript,
        ScriptArgs,
        WorkflowName,
        InstanceName,
        InstanceId,
        ManagementGroupName,
        Query,
        Result,
        Details,
        Output,
        ServerName,
        Source
    }
}
