using NetWork;
using Scene;
using GameData;
using Audio;
using Reversi;
using System;
using System.Collections;
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
        /// <summary>マイページボタンを選択した時の処理 </summary>
        private IObservable<Unit> InputMyPageObservable =>
            myPageBtn.OnClickAsObservable();
        #endregion

        #region SerializeField
        /// <summary>ひとりで遊ぶボタン</summary>
        [SerializeField] private Button onePlayerBtn;
        /// <summary>ふたりで遊ぶボタン</summary>
        [SerializeField] private Button twoPlayerBtn;
        /// <summary>オンライン対戦開始ボタン</summary>
        [SerializeField] private Button onlinePlayerBtn;
        /// <summary>マイページボタン</summary>
        [SerializeField] private Button myPageBtn;
        /// <summary>初回起動時の処理</summary>
        [SerializeField] private FirstStartup firstStartup;
        /// <summary>タイトル画面のUI</summary>
        [SerializeField] private TitleUI titleUI;
        /// <summary>マイページの処理</summary>
        [SerializeField] private MyPage myPage;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            if (!PlayerPrefs.HasKey("FirstTime"))
            {
                firstStartup.Init();
            }

            titleUI.Init();

            myPage.Init();

            // ひとりで遊ぶボタンを押した時の処理
            InputOnePlayerObservable.Subscribe(_ =>
            {
                SE.instance.Play(SE.SEName.ButtonSE);
                GameDataManager.instance.SetGameMode(GameMode.OnePlay);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Reversi);
            }).AddTo(this);

            // ふたりで遊ぶボタンを押した時の処理
            InputTwoPlayerObservable.Subscribe(_ =>
            {
                SE.instance.Play(SE.SEName.ButtonSE);
                GameDataManager.instance.SetGameMode(GameMode.TowPlay);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Reversi);
            }).AddTo(this);

            // オンライン対戦ボタンを押した時の処理
            InputOnlinePlayerObservable.Subscribe(_ =>
            {
                SE.instance.Play(SE.SEName.ButtonSE);
                titleUI.SwicthMatchingWindow(true);
            }).AddTo(this);

            // マイページボタンを押した時の処理
            InputMyPageObservable.Subscribe(_ =>
            {
                SE.instance.Play(SE.SEName.ButtonSE);
                myPage.OpenMyPage();
            }).AddTo(this);

            // マッチングボタンが押された時の処理
            titleUI.MatchingStartSubject.Subscribe(_ =>
            {
                IsMatching();
                SE.instance.Play(SE.SEName.ButtonSE);
            }).AddTo(this);

            // 対戦画面に移動する時の処理
            NetworkManager.instance.OnlineBattleStartSubject.Subscribe(_ =>
            {
                StartCoroutine(PlayerMatched());
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
            SE.instance.Play(SE.SEName.MatchingSE);

            yield return new WaitForSeconds(matchedWaitSeconds);

            GameDataManager.instance.SetGameMode(GameMode.Online);
            SceneLoader.Instance().Load(SceneLoader.SceneName.Reversi);
        }
        #endregion
    }
}