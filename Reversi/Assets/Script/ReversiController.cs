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
        #region SerializeField
        [SerializeField] private ReversiBoard reversiBoard;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            reversiBoard.Init();
        }
        #endregion
    }
}