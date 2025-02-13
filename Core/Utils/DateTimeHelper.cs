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
        DateTime now = DateTime.UtcNow;
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc);
    }
}
