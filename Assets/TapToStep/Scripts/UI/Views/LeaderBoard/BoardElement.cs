using TMPro;
using UnityEngine;

namespace UI.Views.LeaderBoard
{
    public class BoardElement: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rankText;
        [SerializeField] private TextMeshProUGUI _userNameText;
        [SerializeField] private TextMeshProUGUI _distanceText;

        public void Init(int rank, string userName, double distance)
        {
            _rankText.text = rank.ToString();
            _userNameText.text = userName;
            _distanceText.text = ConvertToDistance(distance);
        }
        
        private  string ConvertToDistance(double distance)
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
    }
}