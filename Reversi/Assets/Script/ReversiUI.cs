using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Reversi
{
    /// <summary>
    /// �I�Z���Q�[����UI
    /// </summary>
    public class ReversiUI : MonoBehaviour
    {
        #region SerializeField
        [SerializeField] private Sprite blackSprite;
        [SerializeField] private Sprite whiteSprite;
        /// <summary>�v���C���[���̃I�Z���΂̉摜</summary>
        [SerializeField] private Image playerStoneImg;
        /// <summary>���葤�̃I�Z���΂̉摜</summary>
        [SerializeField] private Image opponentStoneImg;
        /// <summary>�v���C���[���̃I�Z���΂̐��̃e�L�X�g</summary>
        [SerializeField] private TextMeshProUGUI playerStoneNumText;
        /// <summary>���葤�̃I�Z���΂̐��̃e�L�X�g</summary>
        [SerializeField] private TextMeshProUGUI opponentStoneNumText;
        #endregion

        #region PublicMethod
        /// <summary>
        /// ������
        /// </summary>
        public void Init()
        {
            // �΂̐�������������
            playerStoneNumText.text = "0";
            opponentStoneNumText.text = "0";
        }

        public void ViewStoneNum(int playerStoneNum, int opponentStoneNum)
        {
            // �΂̐�������������
            playerStoneNumText.text = playerStoneNum.ToString();
            opponentStoneNumText.text = opponentStoneNum.ToString();
        }

        public void ViewStoneImage(StoneType type)
        {
            playerStoneImg.sprite = type == StoneType.Black ? blackSprite : whiteSprite;
            opponentStoneImg.sprite = type == StoneType.Black ? whiteSprite : blackSprite;
        }
        #endregion
    }
}