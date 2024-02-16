using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomassPuzzle.Helpers.Base
{
    public class RandomationEnum
    {
        public static T GetRandomEnum<T>() where T : Enum
        {
            // Get all enum values
            Array enumValues = Enum.GetValues(typeof(T));

            // Get a random index
            int randomIndex = RandomationNumber.GetRandomNumber(enumValues.Length);

            // Return the random enum value
            return (T)enumValues.GetValue(randomIndex);
        }

    }
}
