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
        /// <summary>盤面マスの座標情報/// </summary>
        private SquareInfo squareInfo;
        /// <summary>オセロの石/// </summary>
        private ReversiStone reversiStone;
        /// <summary>マスに置かれている石のタイプ/// </summary>
        private StoneType stoneType;
        #endregion

        #region SerializeField
        /// <summary>ハイライト表示に使用するオブジェクト/// </summary>
        [SerializeField] private GameObject highlightObj;
        /// <summary>マスを押すボタン/// </summary>
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

        /// <summary>
        /// 石を盤面に設定する処理
        /// </summary>
        public void SetStone(ReversiStone stone, StoneType stoneType)
        {
            stone.transform.localPosition = transform.localPosition;

            // マスに置いている石のタイプを保持
            reversiStone = stone;
            this.stoneType = stoneType;


            stone.Init(stoneType);
        }

        /// <summary>
        /// 盤面のマスをハイライトするかどうか
        /// </summary>
        public void HighlightSquare(bool isValidMove)
        {
            highlightObj.SetActive(isValidMove);
        }

        /// <summary>
        /// 既に石が置かれているかどうかの判定
        /// </summary>
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