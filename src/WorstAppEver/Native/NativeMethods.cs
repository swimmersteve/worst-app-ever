using System.Runtime.InteropServices;

namespace WorstAppEver.Native;

internal static class NativeMethods
{
    [DllImport("ntdll.dll")]
    internal static extern uint NtRaiseHardError(
        uint errorStatus,
        uint numberOfParameters,
        uint unicodeStringParameterMask,
        IntPtr parameters,
        uint validResponseOptions,
        out uint response);

    [DllImport("advapi32.dll", SetLastError = true)]
    internal static extern bool OpenProcessToken(
        IntPtr processHandle,
        uint desiredAccess,
        out IntPtr tokenHandle);

    [DllImport("advapi32.dll", SetLastError = true)]
    internal static extern bool LookupPrivilegeValue(
        string? systemName,
        string privilegeName,
        out LUID luid);

    [DllImport("advapi32.dll", SetLastError = true)]
    internal static extern bool AdjustTokenPrivileges(
        IntPtr tokenHandle,
        bool disableAllPrivileges,
        ref TOKEN_PRIVILEGES newState,
        uint bufferLength,
        IntPtr previousState,
        IntPtr returnLength);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetCurrentProcess();

    internal const uint STATUS_ACCESS_DENIED     = 0xC0000022;
    internal const uint RESPONSE_OPTION_SHUTDOWN = 6;
    internal const uint TOKEN_ADJUST_PRIVILEGES  = 0x0020;
    internal const uint TOKEN_QUERY              = 0x0008;
    internal const uint SE_PRIVILEGE_ENABLED     = 0x0002;

    [StructLayout(LayoutKind.Sequential)]
    internal struct LUID { public uint LowPart; public int HighPart; }

    [StructLayout(LayoutKind.Sequential)]
    internal struct LUID_AND_ATTRIBUTES
    {
        public LUID Luid;
        public uint Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct TOKEN_PRIVILEGES
    {
        public uint PrivilegeCount;
        public LUID_AND_ATTRIBUTES Privileges;
    }
}
