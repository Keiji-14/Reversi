using Scene;
using UnityEngine;

namespace Reversi
{
    /// <summary>
    /// オセロゲーム画面の管理
    /// </summary>
    public class ReversiScene : SceneBase
    {
        #region SerializeField
        [SerializeField] private ReversiController reversiController;
        #endregion

        #region UnityEvent
        public override void Start()
        {
            base.Start();

            reversiController.Init();
        }
        #endregion
    }
}