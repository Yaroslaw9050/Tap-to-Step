using UnityEngine;

namespace Core.Extension.UI
{
    public static class ValueConvertor
    {
        public static string ToDistance(double distance)
        {
            var meters = (int)distance;
            var centimeters = (distance - meters) * 100;
    
            var result = "";
            
            centimeters = Mathf.Round((float)centimeters * 10) / 10;

            if (meters >= 1000)
            {
                int kilometers = meters / 1000;
                meters = meters % 1000;
        
                if (centimeters > 0)
                {
                    result += $"{kilometers}km {meters}.{centimeters / 10}m";
                }
                else if (meters > 0)
                {
                    result += $"{kilometers}km {meters}m";
                }
                else
                {
                    result += $"{kilometers}km";
                }
            }
            else
            {
                if (centimeters > 0)
                {
                    result += $"{meters}.{centimeters / 10}m";
                }
                else if (meters > 0)
                {
                    result += $"{meters}m";
                }
            }

            return result.Trim();
        }
        
        public static string ToBits(ulong rawBitValue)
        {
            string result = "";

            switch (rawBitValue)
            {
                case < 1000:
                    return rawBitValue.ToString();
                case >= 1000 and < 1000000: 
                {
                    float k = rawBitValue / 1000f;
                    result += $"{k:F2}k";

                    return result.Trim();
                }
            }
            
            var v = rawBitValue / 1000000f;
            result += $"{v:F2}m";

            return result.Trim();
        }
    }
}