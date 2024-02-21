using UniRx;
using UnityEngine;

namespace Reversi
{
    /// <summary>
    /// 石の種類
    /// </summary>
    public enum StoneType
    {
        Black,
        White,
        UnSetStone,
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
        /// <summary>先攻後攻の判定</summary>
        private StoneType firstMove;
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

            firstMove = GetRandomPlayer();

            reversiUIInit();

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

            reversiUI.ViewStoneImage(firstMove);
        }

        /// <summary>
        /// 石の数を保持する処理
        /// </summary>
        private void SetStoneNum(StoneNumInfo stoneNumInfo)
        {
            // 先攻だった場合
            (playerStoneNum, opponentStoneNum) = (firstMove == StoneType.Black)
                ? (stoneNumInfo.blackStoneNum, stoneNumInfo.whiteStoneNum)
                : (stoneNumInfo.whiteStoneNum, stoneNumInfo.blackStoneNum);

            reversiUI.ViewStoneNum(playerStoneNum, opponentStoneNum);
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