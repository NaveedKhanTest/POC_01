using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Delegates_Func_Action
{
    class Program
    {
        static void Main(string[] args)
        {
            // basic exampe
            //TestDelegates.UseMyDelegates();

            // using Func and Delegates both togeather
            new TestDelegateAndFunc().CallTests();

            GenericTester.CallTests();

            Console.ReadKey();
        }
    }
}
