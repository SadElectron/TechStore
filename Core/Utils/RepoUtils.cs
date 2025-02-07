using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class RepoUtils
    {

        public static bool IsPageAndCountCorrect(int page, int itemCount)
        {
            return (page < 1 || itemCount <= 0) ? 
                false : 
                true;
        }
    }
}
