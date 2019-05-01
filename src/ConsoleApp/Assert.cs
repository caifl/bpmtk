using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    class Assert
    {
        public static void True(bool condition)
        {
            if (!condition)
                throw new Exception("condition == false");
        }
    }
}
