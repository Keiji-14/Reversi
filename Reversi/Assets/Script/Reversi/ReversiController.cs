using GameData;
using UniRx;
using UnityEngine;

namespace Reversi
{
    /// <summary>
    /// ゲームモードの種類
    /// </summary>
    public enum GameMode
    {
        CPU,
        Online,
    }

    /// <summary>
    /// オセロゲーム画面の処理管理
    /// </summary>
    public class ReversiController : MonoBehaviour
    {
        #region PrivateField
        /// <summary>プレイヤー側のオセロ石の数</summary>
        private int playerStoneNum;
        /// <summary>相手側のオセロ石の数</summary>
        private int opponentStoneNum;
        /// <summary>プレイヤーの石タイプ</summary>
        private StoneType playerStoneType;
        #endregion

        #region SerializeField
        /// <summary>オセロゲームのUI</summary>
        [SerializeField] private ReversiUI reversiUI;
        /// <summary>オセロの盤面の処理</summary>
        [SerializeField] private ReversiBoard reversiBoard;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // Todo: ゲームモードを識別する処理を追加する
            switch (GameDataManager.instance.GetGameMode())
            {
                case GameMode.CPU:
                    playerStoneType = GetRandomPlayer();
                    break;
                case GameMode.Online:
                    DeterminePlayerOrder();
                    break;
            }

            reversiUIInit();

            reversiBoard.GameFinishedSubject.Subscribe(_ =>
            {
                Outcome();
            }).AddTo(this);

            reversiBoard.StoneTypeTurnsSubject.Subscribe(stoneType =>
            {
                reversiUI.OpponentTurnsUI(playerStoneType != stoneType ? true : false);
            }).AddTo(this);

            reversiBoard.StoneCountSubject.Subscribe(stoneNumInfo =>
            {
                SetStoneNum(stoneNumInfo);
            }).AddTo(this);

            reversiBoard.Init();
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        /// UI表示の初期化
        /// </summary>
        private void reversiUIInit()
        {
            reversiUI.Init();

            reversiUI.ViewStoneImage(playerStoneType);
        }

        /// <summary>
        /// オンライン対戦で先攻後攻を決める処理
        /// </summary>
        private void DeterminePlayerOrder()
        {
            playerStoneType = GameDataManager.instance.GetIsPlayer() ? StoneType.Black : StoneType.White;
        }

        /// <summary>
        /// 石の数を保持する処理
        /// </summary>
        private void SetStoneNum(StoneNumInfo stoneNumInfo)
        {
            // 先攻だった場合
            (playerStoneNum, opponentStoneNum) = (playerStoneType == StoneType.Black)
                ? (stoneNumInfo.blackStoneNum, stoneNumInfo.whiteStoneNum)
                : (stoneNumInfo.whiteStoneNum, stoneNumInfo.blackStoneNum);

            reversiUI.ViewStoneNum(playerStoneNum, opponentStoneNum);
        }

        /// <summary>
        /// 勝敗を確認する
        /// </summary>
        private void Outcome()
        {
            if (playerStoneNum > opponentStoneNum)
            {
                Debug.Log("Player Win");
            }
            else if (playerStoneNum == opponentStoneNum)
            {
                Debug.Log("Drow");
            }
            else
            {
                Debug.Log("Player Lose");
            }
        }
        
        /// <summary>
        /// 先攻後攻の結果を返す
        /// </summary>
        private StoneType GetRandomPlayer()
        {
            return Random.Range(0, 2) == 0 ? StoneType.Black : StoneType.White;
        }
        #endregion
    }
}