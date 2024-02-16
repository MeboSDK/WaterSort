using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomassPuzzle.Helpers.Base
{
    public class RandomationNumber
    {
        public static int GetRandomNumber(int number) 
        {
            Random random = new Random();
            return random.Next(number);
        }
    }
}
