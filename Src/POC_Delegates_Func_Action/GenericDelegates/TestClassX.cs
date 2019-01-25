using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Delegates_Func_Action
{
    public class TestClassX
    {
        public void Notify(int i)
        {
            Console.WriteLine($"You entered {i} of type {i.GetType().Name}");
        }

        public void NotifyGeneric<TInput>(TInput input)
        {
            var type = input.GetType();
            Console.WriteLine($"You entered {input} of type {type.Name}");
        }

        public string SayHello()
        {
            return "Hello I am a developer ..";
        }


    }
}
