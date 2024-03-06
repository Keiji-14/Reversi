using NetWork;
using Scene;
using GameData;
using Reversi;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Title
{
    /// <summary>
    /// タイトル画面の処理管理
    /// </summary>
    public class TitleController : MonoBehaviour
    {
        #region PrivateField
        /// <summary>マッチング完了時の待機時間</summary>
        private const float matchedWaitSeconds = 2.0f;
        /// <summary>マッチング中かどうかの処理</summary>
        private bool isMatching = false;
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
            titleUI.Init();

            // ひとりで遊ぶボタンを押した時の処理
            InputOnePlayerObservable.Subscribe(_ =>
            {
                GameDataManager.instance.SetGameMode(GameMode.OnePlay);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Reversi);
            }).AddTo(this);

            // ふたりで遊ぶボタンを押した時の処理
            InputTwoPlayerObservable.Subscribe(_ =>
            {
                GameDataManager.instance.SetGameMode(GameMode.TowPlay);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Reversi);
            }).AddTo(this);

            // オンライン対戦ボタンを押した時の処理
            InputOnlinePlayerObservable.Subscribe(_ =>
            {
                titleUI.SwicthMatchingWindow(true);
            }).AddTo(this);

            // マッチングボタンが押された時の処理
            titleUI.MatchingStartSubject.Subscribe(_ =>
            {
                IsMatching();
            }).AddTo(this);

            // 対戦画面に移動する時の処理
            NetworkManager.instance.OnlineBattleStartSubject.Subscribe(_ =>
            {
                PlayerMatched();
            }).AddTo(this);
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        /// マッチング中かどうかの処理
        /// </summary>
        private void IsMatching()
        {
            isMatching = !isMatching;

            titleUI.SwicthMatchingUI(isMatching);

            if (isMatching)
            {
                NetworkManager.instance.ConnectUsingSettings();
            }
            else
            {
                NetworkManager.instance.LeaveRoom();
            }
        }

        /// <summary>
        /// マッチング完了事の処理
        /// </summary>
        private IEnumerator PlayerMatched()
        {
            titleUI.SwicthMatchedUI();

            yield return new WaitForSeconds(matchedWaitSeconds);

            GameDataManager.instance.SetGameMode(GameMode.Online);
            SceneLoader.Instance().Load(SceneLoader.SceneName.Reversi);
        }
        #endregion
    }
}