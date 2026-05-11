using WorstAppEver.Native;

namespace WorstAppEver.Services;

public static class BsodService
{
    public static void TriggerBsod()
    {
        EnableShutdownPrivilege();
        NativeMethods.NtRaiseHardError(
            errorStatus:                NativeMethods.STATUS_ACCESS_DENIED,
            numberOfParameters:         0,
            unicodeStringParameterMask: 0,
            parameters:                 IntPtr.Zero,
            validResponseOptions:       NativeMethods.RESPONSE_OPTION_SHUTDOWN,
            response:                   out _);
    }

    private static void EnableShutdownPrivilege()
    {
        NativeMethods.OpenProcessToken(
            NativeMethods.GetCurrentProcess(),
            NativeMethods.TOKEN_ADJUST_PRIVILEGES | NativeMethods.TOKEN_QUERY,
            out IntPtr token);

        NativeMethods.LookupPrivilegeValue(null, "SeShutdownPrivilege", out var luid);

        var tp = new NativeMethods.TOKEN_PRIVILEGES
        {
            PrivilegeCount = 1,
            Privileges = new NativeMethods.LUID_AND_ATTRIBUTES
            {
                Luid = luid,
                Attributes = NativeMethods.SE_PRIVILEGE_ENABLED
            }
        };

        NativeMethods.AdjustTokenPrivileges(token, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
    }
}
