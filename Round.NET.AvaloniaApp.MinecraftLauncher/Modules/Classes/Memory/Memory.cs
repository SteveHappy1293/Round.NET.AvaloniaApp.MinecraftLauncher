using System;
using System.Diagnostics;
using System.Management;

namespace Round.NET.AvaloniaApp.MinecraftLauncher.Modules.Memory;

public class Memory
{
    public static ulong GetTotalMemoryInBytes()
    {
        // 使用 WMI 查询总物理内存
        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem"))
        {
            foreach (ManagementObject obj in searcher.Get())
            {
                return Convert.ToUInt64(obj["TotalPhysicalMemory"]);
            }
        }
        throw new Exception("无法获取总内存信息");
    }

    public static ulong GetAvailableMemoryInBytes()
    {
        // 使用 WMI 查询可用物理内存
        using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT FreePhysicalMemory FROM Win32_OperatingSystem"))
        {
            foreach (ManagementObject obj in searcher.Get())
            {
                return Convert.ToUInt64(obj["FreePhysicalMemory"]) * 1024; // 转换为字节
            }
        }
        throw new Exception("无法获取可用内存信息");
    }
}