using System;

namespace POC_Delegates_Func_Action
{
    public static class TestDelegates
    {
        public static void UseMyDelegates()
        {
            // 3rd step create delegate instances
            NumberChanger nc1 = new NumberChanger(MethosForDelegate.AddNum);
            NumberChanger nc2 = new NumberChanger(MethosForDelegate.MultNum);

            //Step 4/last step) calling the methods using the delegate objects
            nc1(25);
            var tempValue = MethosForDelegate.getNum();
            Console.WriteLine("Value of Num: {0}", MethosForDelegate.getNum());
            nc2(5);
            Console.WriteLine("Value of Num: {0}", MethosForDelegate.getNum());
            Console.ReadKey();
        }

    }
}
