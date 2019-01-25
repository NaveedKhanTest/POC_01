using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Delegates_Func_Action
{
    public class GenericExample
    {
        static void Swap<T>(ref T input1, ref T input2)
        {
            T temp = default(T);
            temp = input2;
            input2 = input1;
            input1 = temp;
        }

        public static TElement Add<TElement>(TElement number1, TElement number2)
        {
            dynamic a = number1;
            dynamic b = number2;
            return a + b;
        }
    }
}
