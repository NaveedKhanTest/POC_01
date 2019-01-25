using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_Delegates_Func_Action.GenericDelegates
{
    public delegate void DelegateX<T>(T item);

    public class GenericDelegates
    {
        public delegate void DelegateBasic<T>(T TElement);

        //Anonymous Functions with Delegate
        public delegate void DelegateAnonymousFunc<T>(T TElement);

        public void TestDelegate()
        {
            var cls = new TestClassX();

            DelegateX<int> d1 = cls.Notify;
            d1(100);

            DelegateBasic<string> d2 = new TestClassX().NotifyGeneric;

            d2("10000");
            var xxx = new TestClassX().SayHello();


            //------------- Anonymous Delegate
            DelegateAnonymousFunc<string> anonymousDelegate = (x) =>
            {
                Console.WriteLine(x);
                Console.WriteLine(x);
            };
            anonymousDelegate("Allah o Allah .. -- Anonymous Delegate");

            //var temp = () => { return new TestClassX().SayHello(); };


            //Console.WriteLine(string.Format(" {0}", () =>  cls.SayHello() ) );
            //delegate void() { return cls.SayHello(); }

        }


    }

}
