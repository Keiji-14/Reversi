using UniRx;
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
        /// <summary>手番の判定に使用/// </summary>
        private StoneType stoneTypeTurns;

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
            // 先行を黒の石にする
            stoneTypeTurns = StoneType.Black;

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
            float tileSize = 115f; // マスのサイズ
            float spacing = 5f;   // 幅間隔

            float xPos = col * (tileSize + spacing);
            float yPos = -row * (tileSize + spacing);

            var squareObj = Instantiate(boardSquare, squareGroup);
            squareObj.transform.localPosition = new Vector2(xPos, yPos);
            
            squareObj.Init(row, col);

            squareObj.setStoneObservable.Subscribe(squareInfo =>
            {
                PlaceStone(reversiStone, stoneTypeTurns, squareInfo.row, squareInfo.col);
            }).AddTo(this);

            boardSquares[row, col] = squareObj;
        }

        /// <summary>
        /// 盤面に石を4個配置する
        /// </summary>
        private void SetupInitialStones()
        {
            PlaceInitStone(reversiStone, StoneType.Black, 3, 3);
            PlaceInitStone(reversiStone, StoneType.Black, 4, 4);
            PlaceInitStone(reversiStone, StoneType.White, 3, 4);
            PlaceInitStone(reversiStone, StoneType.White, 4, 3);
        }

        /// <summary>
        /// 盤面の石を初期配置する
        /// </summary>
        private void PlaceInitStone(ReversiStone stoneObj, StoneType stoneType, int row, int col)
        {
            ReversiStone stone = Instantiate(stoneObj, stoneGroup);
            boardSquares[row, col].SetStone(stone, stoneType);

            HighlightPlaceStone();
        }

        /// <summary>
        /// 石の置ける場所をハイライトする
        /// </summary>
        private void HighlightPlaceStone()
        {
            foreach (var boardSquare in boardSquares)
            {
                var row = boardSquare.GetSquareInfo().row;
                var col = boardSquare.GetSquareInfo().col;

                var type = boardSquare.GetStoneType();

                boardSquare.HighlightSquare(IsValidMove(row, col, type));
            }
        }

        /// <summary>
        /// 盤面の石を配置する処理
        /// </summary
        private void PlaceStone(ReversiStone stoneObj, StoneType stoneType, int row, int col)
        {
            if (IsValidMove(row, col, stoneType)) 
            {
                ReversiStone stone = Instantiate(stoneObj, stoneGroup);
                boardSquares[row, col].SetStone(stone, stoneType); // 石を配置

                // 反転処理
                FlipStones(row, col, stoneType);

                // 手番を交代する
                GetOpponentType(stoneTypeTurns);

                // ハイライト表示を更新する
                HighlightPlaceStone();
            }
        }

        // 指定された位置に石を置けるかどうかを判定するメソッド
        public bool IsValidMove(int row, int col, StoneType stoneType)
        {
            if (boardSquares[row, col].SettedStone())
            {
                return false;
            }

            // 反転できる相手の石があるかどうかの判定
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    // 自分自身の方向はスキップ
                    if (dr == 0 && dc == 0)
                        continue;

                    int r = row + dr;
                    int c = col + dc;

                    bool canFlip = false;

                    // 反転できる相手の石があれば置ける
                    while (IsInsideBoard(r, c) && boardSquares[r, c].SettedStone() && boardSquares[r, c].GetStoneType() == GetOpponentType(stoneType))
                    {
                        canFlip = true;
                        r += dr;
                        c += dc;
                    }

                    if (canFlip)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void FlipStones(int row, int col, StoneType stoneType)
        {
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    // 自分自身の方向はスキップ
                    if (dr == 0 && dc == 0)
                        continue;

                    int r = row + dr;
                    int c = col + dc;

                    bool canFlip = false;

                    // 反転できる相手の石があるかどうかを確認
                    while (IsInsideBoard(r, c) && boardSquares[r, c].SettedStone() && boardSquares[r, c].GetStoneType() == GetOpponentType(stoneType))
                    {
                        canFlip = true;
                        r += dr;
                        c += dc;
                    }

                    // 反転処理
                    if (canFlip)
                    {
                        r = row + dr;
                        c = col + dc;

                        while (IsInsideBoard(r, c) && boardSquares[r, c].SettedStone() && boardSquares[r, c].GetStoneType() == GetOpponentType(stoneType))
                        {
                            boardSquares[r, c].GetStone().Flip(); // 石を反転
                            r += dr;
                            c += dc;
                        }
                    }
                }
            }
        }
        
        private bool IsInsideBoard(int row, int col)
        {
            return row >= 0 && row < boardSize && col >= 0 && col < boardSize;
        }

        private StoneType GetOpponentType(StoneType stoneType)
        {
            return stoneType == StoneType.Black ? StoneType.White : StoneType.Black;
        }
        #endregion
    }
}