using TMPro;

namespace Core.Extension.UI
{
    public static class TextMeshProExtension
    {
        public static string ConvertToDistance(float distance)
        {
            var meters = (int)distance;
            var centimeters = (int)((distance - meters) * 100); 
            
            var result = "";
            
            if (meters >= 1000)
            {
                int kilometers = meters / 1000; 
                meters = meters % 1000; 
                result += $"{kilometers}km";
                
                if (meters > 0)
                {
                    result += $" {meters}m";
                }
            }
            else
            {
                if (meters > 0)
                {
                    result += $"{meters}m";
                }
                
                if (centimeters > 0)
                {
                    result += $" {centimeters}cm";
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
                    int k = rawBitValue / 1000; 
                    result += $"{k}k";
                
                    text.SetText(result.Trim());
                    return;
                }
            }
            
            var v = rawBitValue / 1000000; 
            result += $"{v}m";
            
            text.SetText(result.Trim());
        }
    }
}