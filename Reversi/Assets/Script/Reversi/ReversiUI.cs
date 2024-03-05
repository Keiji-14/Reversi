﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Reversi
{
    /// <summary>
    /// オセロゲームのUI
    /// </summary>
    public class ReversiUI : MonoBehaviour
    {
        #region SerializeField
        [SerializeField] private Sprite blackSprite;
        [SerializeField] private Sprite whiteSprite;
        /// <summary>プレイヤー側のオセロ石の画像</summary>
        [SerializeField] private Image playerStoneImg;
        /// <summary>相手側のオセロ石の画像</summary>
        [SerializeField] private Image opponentStoneImg;
        /// <summary>プレイヤー側のオセロ石の数のテキスト</summary>
        [SerializeField] private TextMeshProUGUI playerStoneNumText;
        /// <summary>相手側のオセロ石の数のテキスト</summary>
        [SerializeField] private TextMeshProUGUI opponentStoneNumText;
        /// <summary>相手ターン中に表示するUIオブジェクト</summary>
        [SerializeField] private GameObject opponentTurnsUIObj;
        /// <summary>勝った時に表示するUIオブジェクト</summary>
        [SerializeField] private GameObject youWinUIObj;
        /// <summary>負けた時に表示するUIオブジェクト</summary>
        [SerializeField] private GameObject youLoseUIObj;
        /// <summary>引き分け時に表示するUIオブジェクト</summary>
        [SerializeField] private GameObject deowUIObj;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // 石の数を初期化する
            playerStoneNumText.text = "0";
            opponentStoneNumText.text = "0";

            opponentTurnsUIObj.SetActive(false);
        }

        /// <summary>
        /// 石の数を表示する
        /// </summary>
        public void ViewStoneNum(int playerStoneNum, int opponentStoneNum)
        {
            // 石の数を初期化する
            playerStoneNumText.text = playerStoneNum.ToString();
            opponentStoneNumText.text = opponentStoneNum.ToString();
        }

        /// <summary>
        /// プレイヤーと相手の石の色を表示する
        /// </summary>
        public void ViewStoneImage(StoneType type)
        {
            playerStoneImg.sprite = type == StoneType.Black ? blackSprite : whiteSprite;
            opponentStoneImg.sprite = type == StoneType.Black ? whiteSprite : blackSprite;
        }

        /// <summary>
        /// 相手の手番中に表示するUI
        /// </summary>
        public void OpponentTurnsUI(bool isOpponentTurn)
        {
            opponentTurnsUIObj.SetActive(isOpponentTurn);
        }

        /// <summary>
        /// 勝った時に表示するUI
        /// </summary>
        public void YouWinUI()
        {
            youWinUIObj.SetActive(true);
        }

        /// <summary>
        /// 負けた時に表示するUI
        /// </summary>
        public void YouLoseUI()
        {
            youLoseUIObj.SetActive(true);
        }

        /// <summary>
        /// 引き分け時に表示するUI
        /// </summary>
        public void DrowUI()
        {
            deowUIObj.SetActive(true);
        }
        #endregion

#endregion
    }
}