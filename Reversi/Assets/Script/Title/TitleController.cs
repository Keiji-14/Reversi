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
        /// <summary>ひとりで遊ぶボタンを選択した時の処理 </summary>
        private IObservable<Unit> InputOnePlayerObservable =>
            onePlayerBtn.OnClickAsObservable();
        /// <summary>ふたりで遊ぶボタンを選択した時の処理 </summary>
        private IObservable<Unit> InputTwoPlayerObservable =>
            twoPlayerBtn.OnClickAsObservable();
        /// <summary>オンライン対戦開始ボタンを選択した時の処理 </summary>
        private IObservable<Unit> InputOnlinePlayerObservable =>
            onlinePlayerBtn.OnClickAsObservable();
        #endregion

        #region SerializeField
        /// <summary>ひとりで遊ぶボタン</summary>
        [SerializeField] private Button onePlayerBtn;
        /// <summary>ふたりで遊ぶボタン</summary>
        [SerializeField] private Button twoPlayerBtn;
        /// <summary>オンライン対戦開始ボタン</summary>
        [SerializeField] private Button onlinePlayerBtn;
        /// <summary>タイトル画面のUI</summary>
        [SerializeField] private TitleUI titleUI;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            InputOnePlayerObservable.Subscribe(_ =>
            {
                GameDataManager.instance.SetGameMode(GameMode.OnePlay);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Reversi);
            }).AddTo(this);

            InputTwoPlayerObservable.Subscribe(_ =>
            {
                GameDataManager.instance.SetGameMode(GameMode.TowPlay);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Reversi);
            }).AddTo(this);

            InputOnlinePlayerObservable.Subscribe(_ =>
            {
                titleUI.SwicthMatchingWindow(true);
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