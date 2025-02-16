using TMPro;
using UnityEngine;

namespace Core.Extension.UI
{
    public static class TextMeshProExtension
    {
        public static string ConvertToDistance(float distance)
        {
            var meters = (int)distance;
            var centimeters = (distance - meters) * 100;
    
            var result = "";
            
            centimeters = Mathf.Round(centimeters * 10) / 10;

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
        
        public static void ConvertToBits(this TextMeshProUGUI text, int rawBitValue)
        {
            string result = "";

            switch (rawBitValue)
            {
                case < 1000:
                    text.SetText(rawBitValue.ToString());
                    return;
                case >= 1000 and < 1000000: 
                {
                    float k = rawBitValue / 1000f;
                    result += $"{k:F2}k";

                    text.SetText(result.Trim());
                    return;
                }
            }
            
            var v = rawBitValue / 1000000f;
            result += $"{v:F2}m";

            text.SetText(result.Trim());
        }
    }
}