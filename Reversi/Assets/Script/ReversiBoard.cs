using System.Collections.Generic;
using UnityEngine;

namespace Reversi
{
    /// <summary>
    /// オセロゲームの盤面
    /// </summary>
    public class ReversiBoard : MonoBehaviour
    {
        #region PrivateField
        /// <summary>盤面のマス/// </summary>
        private const int boardSize = 8;
        private BoardSquare[,] boardSquares;
        #endregion

        #region SerializeField
        [SerializeField] private Transform squareGroup;
        [SerializeField] private Transform stoneGroup;
        [SerializeField] private BoardSquare boardSquare;
        [SerializeField] private ReversiStone reversiStone;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            CreateBoard();

            SetupInitialStones();
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        /// 盤面を生成する処理
        /// </summary>
        private void CreateBoard()
        {
            boardSquares = new BoardSquare[boardSize, boardSize];

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    CreateSquare(row, col);
                }
            }
        }

        /// <summary>
        /// 盤面のマスを生成する処理
        /// </summary>
        private void CreateSquare(int row, int col)
        {
            float tileSize = 110f; // マスのサイズ
            float spacing = 10f;   // 幅間隔

            float xPos = col * (tileSize + spacing);
            float yPos = -row * (tileSize + spacing);

            var squareObj = Instantiate(boardSquare, squareGroup);
            squareObj.transform.localPosition = new Vector3(xPos, yPos);
            squareObj.Init(row, col);

            boardSquares[row, col] = squareObj;
            Debug.Log(boardSquares[row, col].transform.position);
        }

        private void SetupInitialStones()
        {
            // 中央に4つの石を配置
            PlaceStone(reversiStone, 3, 3);
            PlaceStone(reversiStone, 4, 4);
            PlaceStone(reversiStone, 3, 4);
            PlaceStone(reversiStone, 4, 3);
        }

        public void PlaceStone(ReversiStone stoneObj, int row, int col)
        {
            var stone = Instantiate(stoneObj, stoneGroup);

            boardSquares[row, col].SetStone(stone);
        }
        #endregion
    }
}