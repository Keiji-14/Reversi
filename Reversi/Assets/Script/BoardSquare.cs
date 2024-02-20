using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Reversi
{
    /// <summary>
    /// オセロゲームの盤面のマス
    /// </summary>
    public class BoardSquare : MonoBehaviour
    {
        #region PublicField
        /// <summary>マスを選択した時の処理 </summary>
        public IObservable<SquareInfo> setStoneObservable => 
            boardSquareBtn.OnClickAsObservable().Select(_ => squareInfo);
        #endregion

        #region PrivateField
        private SquareInfo squareInfo;

        private ReversiStone reversiStone;
        /// <summary>マスに置かれている石のタイプ/// </summary>
        private StoneType stoneType;
        #endregion

        #region SerializeField
        [SerializeField] private Button boardSquareBtn;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(int row, int col)
        {
            squareInfo = new SquareInfo(row, col);
        }

        public void SetStone(ReversiStone stone, StoneType stoneType)
        {
            stone.transform.localPosition = transform.localPosition;

            // マスに置いている石のタイプを保持
            reversiStone = stone;
            this.stoneType = stoneType;


            stone.Init(stoneType);
        }

        public bool SettedStone()
        {
            if (reversiStone != null)
            {
                return true;
            }
            return false;
        }

        public SquareInfo GetSquareInfo()
        {
            return squareInfo;
        }

        public ReversiStone GetStone()
        {
            return reversiStone;
        }

        public StoneType GetStoneType()
        {
            return stoneType;
        }
        #endregion
    }

    /// <summary>
    /// 盤面のマスの座標
    /// </summary>
    public class SquareInfo
    {
        public int row;
        public int col;

        public SquareInfo(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
}