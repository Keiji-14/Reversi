using NetWork;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    /// <summary>
    /// タイトル画面のUI
    /// </summary>
    public class TitleUI : MonoBehaviour
    {
        #region PrivateField
        /// <summary>マッチング開始の処理</summary>
        public Subject<Unit> MatchingStartSubject = new Subject<Unit>();
        #endregion

        #region PrivateField
        /// <summary>マッチングボタンを押した時の処理</summary>
        private IObservable<Unit> InputMatchingBtnObservable =>
            matchingBtn.OnClickAsObservable();
        /// <summary>マッチングウィンドウを閉じるボタンを押した時の処理</summary>
        private IObservable<Unit> InputCloseMatchingWindowBtnObservable =>
            closeMatchingWindowBtn.OnClickAsObservable();
        #endregion

        #region SerializeField
        /// <summary>マッチングボタン</summary>
        [SerializeField] private Button matchingBtn;
        /// <summary>マッチングウィンドウを閉じるボタン</summary>
        [SerializeField] private Button closeMatchingWindowBtn;
        /// <summary>マッチングウィンドウ</summary>
        [SerializeField] private GameObject matchingWindow;
        /// <summary>マッチングロードUI</summary>
        [SerializeField] private GameObject matchingLoadingUI;
        /// <summary>マッチング開始のテキストUI</summary>
        [SerializeField] private GameObject matchingStartTextUI;
        /// <summary>マッチング中のテキストUI</summary>
        [SerializeField] private GameObject matchingNowTextUI;
        /// <summary>マッチング完了のテキストUI</summary>
        [SerializeField] private GameObject matchedTextUI;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // マッチングボタンを押した時の処理
            InputMatchingBtnObservable.Subscribe(_ =>
            {
                MatchingStartSubject.OnNext(Unit.Default);
            }).AddTo(this);

            // マッチングウィンドウの閉じるボタンを押した時の処理
            InputCloseMatchingWindowBtnObservable.Subscribe(_ =>
            {
                SwicthMatchingWindow(false);
                SwicthMatchingUI();
                NetworkManager.instance.LeaveRoom();
            }).AddTo(this);
        }

        /// <summary>
        /// マッチングウィンドウの表示を切り替える処理
        /// </summary>
        public void SwicthMatchingWindow(bool isView = false)
        {
            matchingWindow.SetActive(isView);
        }

        /// <summary>
        /// マッチング状態の表示を切り替える処理
        /// </summary>
        public void SwicthMatchingUI(bool isView = false)
        {
            matchingLoadingUI.SetActive(isView);
            matchingStartTextUI.SetActive(!isView);
            matchingNowTextUI.SetActive(isView);
        }

        /// <summary>
        /// マッチング完了時の表示に切り替える処理
        /// </summary>
        public void SwicthMatchedUI()
        {
            // マッチング完了時にボタンを無効化にする
            matchingBtn.interactable = false;

            matchingLoadingUI.SetActive(false);
            matchingNowTextUI.SetActive(false);
        }
        #endregion
    }
}