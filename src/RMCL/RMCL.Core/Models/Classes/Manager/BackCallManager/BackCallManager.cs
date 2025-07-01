using System;
using System.Collections.Generic;
using RMCL.Base.Entry.BackCall;
using RMCL.Base.Enum.BackCall;

namespace RMCL.Core.Models.Classes.Manager.BackCallManager;

public class BackCallManager
{
    public static List<BackCallEntry> Calls = new();

    public static void RegisteredBackCall(Action backCall, BackCallType type)
    {
        Calls.Add(new BackCallEntry()
        {
            CallAction = backCall,
            Type = type
        });
    }

    public static void Call(BackCallType type)
    {
        Calls.ForEach(x =>
        {
            if (x.Type == type)
            {
                x.CallAction.Invoke();
            }
        });
    }
}