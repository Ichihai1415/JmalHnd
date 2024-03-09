using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JmalHnd.Util
{
    public class Conv
    {
        public static string JMAurl2code(string url)
        {
            return url.Split('_')[2];
        }


    }
}
