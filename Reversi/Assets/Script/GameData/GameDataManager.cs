using Reversi;
using UnityEngine;

namespace GameData
{
    public class GameDataManager : MonoBehaviour
    {
        #region PublicField
        public static GameDataManager instance = null;
        #endregion

        #region PrivateField
        private bool isPlayer1;
        private GameMode gameMode;
        private PlayerData playerData;
        #endregion

        #region UnityEvent
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                GameDataInit();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region PublicMethod
        /// <summary>
        /// ゲームモードを設定する処理
        /// </summary>
        public void SetGameMode(GameMode gameMode)
        {
            this.gameMode = gameMode;
        }

        /// <summary>
        /// ゲームモードの情報を返す
        /// </summary>
        public GameMode GetGameMode()
        {
            return gameMode;
        }

        /// <summary>
        /// プレイヤー1かどうかの判定を設定
        /// </summary>
        public void SetIsPlayer(bool isPlayer1)
        {
            this.isPlayer1 = isPlayer1;
        }

        /// <summary>
        /// プレイヤー1かどうかの判定を返す
        /// </summary>
        public bool GetIsPlayer()
        {
            return isPlayer1;
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        /// 初期化
        /// </summary>
        private void GameDataInit()
        {

        }
        #endregion
    }
}