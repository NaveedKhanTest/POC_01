
/*
Delegates allow methods to be passed as parameters.
 
Delegates can be used to define callback methods.
Delegates can be chained together; for example, multiple methods can be called on a single event.
	
We need following 4 steps for a delegate
//1) Declare delegate
delegate int NumberChanger(int n);

//2) some method for the delegate (should match the signature of declared delegate)
public static int AddNum(int p)
{
    num += p;
    return num;
}

//3)create delegate instances
NumberChanger nc1 = new NumberChanger(MethosForDelegate.AddNum);

//4) calling the methods using the delegate objects
    nc1(25);

NOTE: //Step 3 and 4 get combined if we use Func<>
*/

namespace POC_Delegates_Func_Action
{
    // Step 1 declare/define delegate
    delegate int NumberChanger(int n);

        public static class MethosForDelegate
        {
            static int num = 10;
        //step 2) some method for the delegate matching the signature of the defined delegate in this case NumberChanger
        public static int AddNum(int p)
            {
                num += p;
                return num;
            }
            public static int MultNum(int q)
            {
                num *= q;
                return num;
            }
            public static int getNum()
            {
                return num;
            }

        }
    }
