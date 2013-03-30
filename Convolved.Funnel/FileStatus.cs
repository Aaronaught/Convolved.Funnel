using System;

namespace Convolved.Funnel
{
    public enum FileStatus
    {
        OK,
        Error,
        Skipped     // Should skipping be support? Under what expected/unexpected conditions?
    }
}