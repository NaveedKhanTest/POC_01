using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Delegates_Func_Action
{
    //public delegate bool MyDelegate(string firstName, int age);

    // https://www.tutorialspoint.com/csharp/csharp_delegates.htm 

    public class TestDelegateAndFunc
    {
        //3rd step) create delegate instances
        //NumberChanger nc1 = new NumberChanger(MethosForDelegate.AddNum);

        public void CallTests()
        {
            // 3rd step create delegate instances
            //NumberChanger nc1 = new NumberChanger(MethosForDelegate.AddNum);
                        
            var objCls = new MethosForDelegateAndFunc();

            // 3rd step create delegate instances
            MyDelegate firstDelegate = objCls.FunctionOne;
            Console.WriteLine("delegate to call First Function this the last step to..");

            //Step 4/last step)  calling the methods using the delegate objects
            firstDelegate("Ramis", 7);
            Console.ReadKey();
            firstDelegate = objCls.Function2nd;
            firstDelegate("Ramis", 7);

            Console.WriteLine("Func<> to call the same First Function ..");

            //Step 3 if we use Func<>) calling the methods using the delegate objects
            Func<string, int, bool> FuncFirstDelegate = objCls.FunctionOne;

            //Step 4 if we use Func<>) last step calling the methods using the delegate objects
            FuncFirstDelegate("Ramis Jadoon", 7);

            Action<string, int> ActionFirstDelegate = objCls.FunctionForAction;
            ActionFirstDelegate("Ramis Jadoon", 7);

            Console.ReadKey();

        }



    }
}
