using NetWork;
using Scene;
using GameData;
using Reversi;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    /// <summary>
    /// タイトル画面の処理管理
    /// </summary>
    public class TitleController : MonoBehaviour
    {
        #region PrivateField
        /// <summary>オンライン対戦開始ボタンを選択した時の処理 </summary>
        private IObservable<Unit> InputOnlinePlayerObservable =>
            onlinePlayerBtn.OnClickAsObservable();
        #endregion

        #region SerializeField
        /// <summary>オンライン対戦開始ボタン</summary>
        [SerializeField] private Button onlinePlayerBtn;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            InputOnlinePlayerObservable.Subscribe(_ =>
            {
                NetworkManager.instance.ConnectUsingSettings();
            }).AddTo(this);

            NetworkManager.instance.OnlineBattleStartSubject.Subscribe(_ =>
            {
                GameDataManager.instance.SetGameMode(GameMode.Online);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Reversi);
            }).AddTo(this);
        }
        #endregion
    }
}