using NetWork;
using Scene;
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
        OnePlay,
        TowPlay,
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
        /// <summary>オセロゲーム終了のUI</summary>
        [SerializeField] private ReversiFinishUI reversiFinishUI;
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
                case GameMode.OnePlay:
                    playerStoneType = GetRandomPlayer();
                    break;
                case GameMode.TowPlay:
                    // ふたりで遊ぶ時は黒固定
                    playerStoneType = StoneType.Black;
                    break;
                case GameMode.Online:
                    DeterminePlayerOrder();
                    break;
            }

            ReversiUIInit();

            reversiBoard.Init();

            reversiBoard.GameFinishedSubject.Subscribe(_ =>
            {
                // ゲームモードがふたりで遊ぶ以外の場合
                if (GameDataManager.instance.GetGameMode() != GameMode.TowPlay)
                {
                    Outcome();
                }
                else
                {
                    TowPlayOutcome();
                }
                
            }).AddTo(this);

            // 相手の手番を示すUIを表示する処理
            reversiBoard.StoneTypeTurnsSubject.Subscribe(stoneType =>
            {
                // ゲームモードがふたりで遊ぶ以外の場合
                if (GameDataManager.instance.GetGameMode() != GameMode.TowPlay)
                {
                    reversiUI.OpponentTurnsUI(playerStoneType != stoneType ? true : false);
                }
            }).AddTo(this);

            reversiBoard.StoneCountSubject.Subscribe(stoneNumInfo =>
            {
                SetStoneNum(stoneNumInfo);
            }).AddTo(this);
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        /// UI表示の初期化
        /// </summary>
        private void ReversiUIInit()
        {
            reversiUI.Init();

            reversiUI.ViewStoneImage(playerStoneType);

            reversiFinishUI.Init();

            reversiFinishUI.TitleBackSubject.Subscribe(_ =>
            {
                TitleBack();
            }).AddTo(this);
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
                reversiFinishUI.YouWinUI();
            }
            else if (playerStoneNum == opponentStoneNum)
            {
                reversiFinishUI.DrowUI();
            }
            else
            {
                reversiFinishUI.YouLoseUI();
            }

            reversiFinishUI.FinishWindowUI();
        }

        /// <summary>
        /// 二人で遊ぶ時の勝敗を確認する
        /// </summary>
        private void TowPlayOutcome()
        {
            if (playerStoneNum > opponentStoneNum)
            {
                reversiFinishUI.BlackWinUI();
            }
            else if (playerStoneNum == opponentStoneNum)
            {
                reversiFinishUI.DrowUI();
            }
            else
            {
                reversiFinishUI.WhiteWinUI();
            }

            reversiFinishUI.FinishWindowUI();
        }

        /// <summary>
        /// タイトルシーン遷移時の処理
        /// </summary>
        private void TitleBack()
        {
            // オンライン対戦だった場合はルーム退出を行う
            if (GameDataManager.instance.GetGameMode() == GameMode.Online)
            {
                NetworkManager.instance.LeaveRoom();
            }
            SceneLoader.Instance().Load(SceneLoader.SceneName.Title);
        }

        /// <summary>
        /// 先攻後攻の結果を返す
        /// </summary>
        private StoneType GetRandomPlayer()
        {
            return Random.Range(0, 2) == 0 ? StoneType.Black : StoneType.White;
        }

        /// <summary>
        /// オンライン対戦で先攻後攻を決める処理
        /// </summary>
        private void DeterminePlayerOrder()
        {
            playerStoneType = GameDataManager.instance.GetIsPlayer() ? StoneType.Black : StoneType.White;
        }
        #endregion
    }
}