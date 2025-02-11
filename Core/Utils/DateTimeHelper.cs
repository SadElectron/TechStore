using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils;

public static class DateTimeHelper
{
    public static DateTime GetUtcNow()
    {
        return DateTime.UtcNow.AddTicks(-DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond);
    }
}
