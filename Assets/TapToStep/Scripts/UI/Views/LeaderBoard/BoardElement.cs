using Core.Extension.UI;
using TMPro;
using UnityEngine;

namespace UI.Views.LeaderBoard
{
    public class BoardElement: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rankText;
        [SerializeField] private TextMeshProUGUI _userNameText;
        [SerializeField] private TextMeshProUGUI _distanceText;

        public void Init(int rank, string userName, double distance, bool isMyCard = false)
        {
            _rankText.text = rank.ToString();
            _userNameText.text = userName;
            _distanceText.text = TextMeshProExtension.ConvertToDistance((float)distance);

            if (isMyCard == false) return;
            
            _rankText.color = Color.magenta;
            _userNameText.color = Color.magenta;
            _distanceText.color = Color.magenta;
        }
    }
}