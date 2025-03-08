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

        public void Init(int rank, string userName, double distance)
        {
            _rankText.text = rank.ToString();
            _userNameText.text = userName;
            _distanceText.text = ValueConvertor.ToDistance(distance);
        }
    }
}