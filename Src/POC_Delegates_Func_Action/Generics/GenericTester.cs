using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Delegates_Func_Action
{
    public class GenericTester
    {
        public static void CallTests()
        {
            var resultNo = GenericExample.Add(4, 6);
            var resultString = GenericExample.Add("Ramis ", "Jadoon");
        }
    }
}
