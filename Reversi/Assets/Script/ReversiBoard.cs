﻿using UniRx;
using UnityEngine;

namespace Reversi
{
    /// <summary>
    /// オセロゲームの盤面
    /// </summary>
    public class ReversiBoard : MonoBehaviour
    {
        #region PublicField
        /// <summary>盤面上の石の数をカウントする処理 </summary>
        public Subject<StoneNumInfo> StoneCountSubject = new Subject<StoneNumInfo>();
        #endregion

        #region PrivateField
        /// <summary>盤面一列のマス数</summary>
        private const int boardSize = 8;
        /// <summary>マスのサイズ</summary>
        private const float tileSize = 115f;
        /// <summary>マスとマスの幅間隔</summary>
        private const float spacing = 5f;
        /// <summary>手番の判定に使用</summary>
        private StoneType stoneTypeTurns;

        private BoardTile[,] boardTiles;
        #endregion

        #region SerializeField
        [SerializeField] private Transform tileGroup;
        [SerializeField] private Transform stoneGroup;
        [SerializeField] private BoardTile boardTileObj;
        [SerializeField] private ReversiStone reversiStoneObj;
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

            StoneCount();
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        /// 盤面を生成する処理
        /// </summary>
        private void CreateBoard()
        {
            boardTiles = new BoardTile[boardSize, boardSize];

            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    CreateTile(row, col);
                }
            }
        }

        /// <summary>
        /// 盤面のマスを生成する処理
        /// </summary>
        private void CreateTile(int row, int col)
        {
            float xPos = col * (tileSize + spacing);
            float yPos = -row * (tileSize + spacing);

            var tileObj = Instantiate(boardTileObj, tileGroup);
            tileObj.transform.localPosition = new Vector2(xPos, yPos);
            
            tileObj.Init(row, col);

            tileObj.SetStoneObservable.Subscribe(tileInfo =>
            {
                PlaceStone(reversiStoneObj, tileInfo.row, tileInfo.col);
            }).AddTo(this);

            boardTiles[row, col] = tileObj;
        }

        /// <summary>
        /// 盤面に石を4個配置する
        /// </summary>
        private void SetupInitialStones()
        {
            PlaceInitStone(reversiStoneObj, StoneType.Black, 3, 3);
            PlaceInitStone(reversiStoneObj, StoneType.Black, 4, 4);
            PlaceInitStone(reversiStoneObj, StoneType.White, 3, 4);
            PlaceInitStone(reversiStoneObj, StoneType.White, 4, 3);

            HighlightPlaceStone();

            StoneCount();
        }

        /// <summary>
        /// 盤面の石を初期配置する
        /// </summary>
        private void PlaceInitStone(ReversiStone stoneObj, StoneType stoneType, int row, int col)
        {
            ReversiStone stone = Instantiate(stoneObj, stoneGroup);
            boardTiles[row, col].SetStone(stone, stoneType);
        }

        /// <summary>
        /// 石の置ける場所をハイライトする
        /// </summary>
        private void HighlightPlaceStone()
        {
            foreach (var boardTile in boardTiles)
            {
                var row = boardTile.GetTileInfo().row;
                var col = boardTile.GetTileInfo().col;

                boardTile.HighlightTile(IsValidMove(row, col, stoneTypeTurns));
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
                boardTiles[row, col].SetStone(stone, stoneTypeTurns);

                // 反転処理
                FlipStones(row, col, stoneTypeTurns);

                // 手番を交代する
                stoneTypeTurns = GetOpponentType(stoneTypeTurns);

                // ハイライト表示を更新する
                HighlightPlaceStone();

                // 石のカウントを更新する
                StoneCount();

                // 終了判定
                if (IsBoardFull())
                {
                    Debug.Log("Game Over!");
                    // Todo: ゲーム終了時の処理を追加する
                }
            }
        }

        /// <summary>
        /// 指定された位置に石を置けるかどうかを判定する
        /// </summary>
        public bool IsValidMove(int row, int col, StoneType stoneType)
        {
            // 既に石が置かれているかどうかの判定
            if (boardTiles[row, col].SettedStone())
            {
                return false;
            }

            // 全方向のマスを確認する
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
                    while (IsInsideBoard(r, c) && boardTiles[r, c].SettedStone() && boardTiles[r, c].GetStoneType() != stoneType)
                    {
                        canFlip = true;
                        r += dr;
                        c += dc;
                    }

                    if (canFlip && IsInsideBoard(r, c) && boardTiles[r, c].GetStoneType() == stoneType)
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
            // 全方向のマスを確認する
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
                    while (IsInsideBoard(r, c) && boardTiles[r, c].SettedStone() && boardTiles[r, c].GetStoneType() == GetOpponentType(stoneType))
                    {
                        canFlip = true;
                        r += dr;
                        c += dc;
                    }

                    // 反転処理
                    if (canFlip && IsInsideBoard(r, c) && boardTiles[r, c].SettedStone() && boardTiles[r, c].GetStoneType() == stoneType)
                    {
                        r = row + dr;
                        c = col + dc;

                        while (IsInsideBoard(r, c) && boardTiles[r, c].SettedStone() && boardTiles[r, c].GetStoneType() == GetOpponentType(stoneType))
                        {
                            // 石を反転する
                            boardTiles[r, c].GetStone().Flip();
                            boardTiles[r, c].FlipStone();
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

        /// <summary>
        /// 盤面がすべて埋まったかどうかの判定
        /// </summary>
        private bool IsBoardFull()
        {
            foreach (var boardTile in boardTiles)
            {
                // 空いているマスがある場合はfalseを返す
                if (!boardTile.SettedStone())
                {
                    return false; 
                }
            }

            return true;
        }

        /// <summary>
        /// 石の数を数える処理
        /// </summary>
        private void StoneCount()
        {
            var blackCountNum = 0;
            var whiteCountNum = 0;

            foreach (var boardTile in boardTiles)
            {
                switch (boardTile.GetStoneType())
                {
                    case StoneType.Black:
                        blackCountNum++;
                        break;
                    case StoneType.White:
                        whiteCountNum++;
                        break;
                    case StoneType.UnSetStone:
                        break;
                }
            }

            var stoneNumInfo = new StoneNumInfo(blackCountNum, whiteCountNum);
            // それぞれの石の数情報を送る
            StoneCountSubject.OnNext(stoneNumInfo);
        }
        #endregion
    }

    /// <summary>
    /// 配置している石の数情報
    /// </summary>
    public class StoneNumInfo
    {
        public int blackStoneNum;
        public int whiteStoneNum;

        public StoneNumInfo(int blackStoneNum, int whiteStoneNum)
        {
            this.blackStoneNum = blackStoneNum;
            this.whiteStoneNum = whiteStoneNum;
        }
    }
}