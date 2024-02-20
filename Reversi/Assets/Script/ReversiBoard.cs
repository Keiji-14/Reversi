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
                PlaceStone(reversiStone, squareInfo.row, squareInfo.col);
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

                boardSquare.HighlightSquare(IsValidMove(row, col, stoneTypeTurns));
            }
        }

        /// <summary>
        /// 盤面の石を配置する処理
        /// </summary
        private void PlaceStone(ReversiStone stoneObj, int row, int col)
        {
            if (IsValidMove(row, col, stoneTypeTurns)) 
            {
                ReversiStone stone = Instantiate(stoneObj, stoneGroup);
                boardSquares[row, col].SetStone(stone, stoneTypeTurns);

                // 反転処理
                FlipStones(row, col, stoneTypeTurns);

                // 手番を交代する
                stoneTypeTurns = GetOpponentType(stoneTypeTurns);

                // ハイライト表示を更新する
                HighlightPlaceStone();
            }
        }

        /// <summary>
        /// 指定された位置に石を置けるかどうかを判定する
        /// </summary>
        public bool IsValidMove(int row, int col, StoneType stoneType)
        {
            // 既に石が置かれているかどうかの判定
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
                    while (IsInsideBoard(r, c) && boardSquares[r, c].SettedStone() && boardSquares[r, c].GetStoneType() != stoneType)
                    {
                        canFlip = true;
                        r += dr;
                        c += dc;
                    }

                    if (canFlip && IsInsideBoard(r, c) && boardSquares[r, c].GetStoneType() == stoneType)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 石の反転を行う処理
        /// </summary>
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
                    if (canFlip && IsInsideBoard(r, c) && boardSquares[r, c].SettedStone() && boardSquares[r, c].GetStoneType() == stoneType)
                    {
                        r = row + dr;
                        c = col + dc;

                        while (IsInsideBoard(r, c) && boardSquares[r, c].SettedStone() && boardSquares[r, c].GetStoneType() == GetOpponentType(stoneType))
                        {
                            // 石を反転する
                            boardSquares[r, c].GetStone().Flip();
                            boardSquares[r, c].FlipStone();
                            r += dr;
                            c += dc;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 盤面の外にはみ出していないかの判定を返す
        /// </summary>
        private bool IsInsideBoard(int row, int col)
        {
            return row >= 0 && row < boardSize && col >= 0 && col < boardSize;
        }

        /// <summary>
        /// 反転した石のタイプを返す
        /// </summary>
        private StoneType GetOpponentType(StoneType stoneType)
        {
            return stoneType == StoneType.Black ? StoneType.White : StoneType.Black;
        }
        #endregion
    }
}