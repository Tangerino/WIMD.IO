using System;

namespace WimdioApiProxy.v2.DataTransferObjects.Users
{
    [Flags]
    public enum Permission
    {
        Read = 1,
        Create = 2,
        Update = 4,
        Delete = 8,
    }
}
