using UnityEngine;

namespace Reversi
{
    /// <summary>
    /// オセロゲームの盤面のマス
    /// </summary>
    public class BoardSquare : MonoBehaviour
    {
        #region PrivateField
        private int row;
        private int col;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public void SetStone(ReversiStone stone)
        {
            stone.transform.localPosition = transform.localPosition;
        }
        #endregion
    }
}