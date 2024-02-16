namespace ThomassPuzzle.Models.Flask
{
    public class RadiusModel
    {
       
        public static float GetMidRadius(int liquidIndex)
        {
            switch (liquidIndex)
            {
                case 3:
                    return -52.2f;
                case 2:
                    return -60.4f;
                case 1:
                    return -67.8f;
                case 0:
                    return -79.6f;
            }

            return 0;
        }

        public static float GetEndRadius(int index)
        {
            switch (index)
            {
                case 3:
                    return -58f;
                case 2:
                    return -66f;
                case 1:
                    return -80f;
                case 0:
                    return -93f;
            }

            return 0;
        }
    }
}
