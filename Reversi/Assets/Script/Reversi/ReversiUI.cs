using GameData;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Reversi
{
    /// <summary>
    /// オセロゲームのUI
    /// </summary>
    public class ReversiUI : MonoBehaviourPunCallbacks
    {
        #region PrivateField
        /// <summary>石の初期表示数</summary>
        private const int stoneCountInit = 0;
        #endregion

        #region SerializeField
        /// <summaryひとりで遊ぶ時の相手側の名前</summary>
        [SerializeField] private string onePlayOpponentName;
        /// <summary>ふたりで遊ぶ時の相手側の名前</summary>
        [SerializeField] private string towPlayOpponentName;
        /// <summary>黒のオセロ石の画像</summary>
        [SerializeField] private Sprite blackSprite;
        /// <summary>白のオセロ石の画像</summary>
        [SerializeField] private Sprite whiteSprite;
        /// <summary>プレイヤー側のオセロ石の画像</summary>
        [SerializeField] private Image playerStoneImg;
        /// <summary>相手側のオセロ石の画像</summary>
        [SerializeField] private Image opponentStoneImg;
        /// <summary>プレイヤー側の名前のテキスト</summary>
        [SerializeField] private TextMeshProUGUI playerNameText;
        /// <summary>プレイヤー側のオセロ石の数のテキスト</summary>
        [SerializeField] private TextMeshProUGUI playerStoneNumText;
        /// <summary>相手側の名前のテキスト</summary>
        [SerializeField] private TextMeshProUGUI opponentNameText;
        /// <summary>相手側のオセロ石の数のテキスト</summary>
        [SerializeField] private TextMeshProUGUI opponentStoneNumText;
        /// <summary>相手ターン中に表示するUIオブジェクト</summary>
        [SerializeField] private GameObject opponentTurnsUIObj;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // 石の数を初期化する
            playerStoneNumText.text = stoneCountInit.ToString();
            opponentStoneNumText.text = stoneCountInit.ToString();

            playerNameText.text = GameDataManager.instance.GetPlayerData().name;
            SetOpponentName();

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
        #endregion

        #region PrivateMethod
        /// <summary>
        /// 相手側の名前を設定する処理
        /// </summary>
        private void SetOpponentName()
        {
            switch (GameDataManager.instance.GetGameMode())
            {
                case GameMode.OnePlay:
                    opponentNameText.text = onePlayOpponentName;
                    break;
                case GameMode.TowPlay:
                    opponentNameText.text = towPlayOpponentName;
                    break;
                case GameMode.Online:
                    photonView.RPC(nameof(RpcOnlinePlayerOpponentName), RpcTarget.Others, GameDataManager.instance.GetPlayerData().name);
                    break;
            }
        }

        /// <summary>
        /// オンライン対戦時の相手側の名前を設定
        /// </summary>
        [PunRPC]
        private void RpcOnlinePlayerOpponentName(string onlinePlayerOpponentName)
        {
            opponentNameText.text = onlinePlayerOpponentName;
        }
        #endregion
    }
}