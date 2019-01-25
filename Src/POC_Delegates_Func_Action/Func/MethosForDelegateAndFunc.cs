using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Delegates_Func_Action
{
    //1) Declare delegate
    public delegate bool MyDelegate(string firstName, int age);


    public class MethosForDelegateAndFunc
    {
        //2) some method for the delegate (should match the signature of declared delegate)
        public bool FunctionOne(string firstName, int age)
        {
            Console.WriteLine($"Hello {firstName}, Age: {age} and is an Adult {(age > 18)}");
            return (age < 18);
        }

        public  bool Function2nd(string firstName, int age)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Hello {firstName}, Age: {age} and is a child {(age < 18)}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.ReadKey();
            return (age < 18);
        }

        public void FunctionForAction(string firstName, int age)
        {
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Action <> ...  Function called..");
            Console.WriteLine($"Hello {firstName}, Age: {age} and is a child {(age < 18)}");
            Console.ReadKey();
        }

    }
}
