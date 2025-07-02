using RMCL.Base.Enum;

namespace RMCL.Base.Entry.Style;

public class ColorEntry
{
    public string ThemeColors { get; set; } = "#2D7D9AFF";
    public ColorType ColorType { get; set; } = ColorType.System;
    public ThemeType Theme { get; set; } = ThemeType.Dark;
}

public enum ThemeType
{
    Dark,
    Light
}