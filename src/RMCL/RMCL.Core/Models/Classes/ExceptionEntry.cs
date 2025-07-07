using System;
using System.Collections.Generic;
using RMCL.Base.Entry.Java;
using RMCL.Base.Entry.User;
using RMCL.Config;
using RMCL.Core.Models.Classes.Manager.UserManager;
using RMCL.Modules.Enum;

namespace RMCL.Modules.Entry;

public class ExceptionEntry
{
    public ConfigRoot MainConfig { get; set; } = Config.Config.MainConfig;
    public List<JavaDetils> JavaRoot = JavaManager.JavaManager.JavaRoot.Javas;
    public List<UserEntry> UserConfig { get; set; } = PlayerManager.Player.Accounts;
    public ExceptionEnum ExceptionType { get; set; }
    public DateTime RecordTime { get; set; } = DateTime.Now;
    public string Exception { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;
    public string SystemVersion { get; set; } = string.Empty;
    public string SystemLanguage { get; set; } = string.Empty;
    public string SystemTimeZone { get; set; } = string.Empty;
    public string ExceptionSource { get; set; } = string.Empty;
}