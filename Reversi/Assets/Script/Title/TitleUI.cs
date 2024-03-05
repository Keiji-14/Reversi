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
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            InputMatchingBtnObservable.Subscribe(_ =>
            {
                NetworkManager.instance.ConnectUsingSettings();
            }).AddTo(this);

            InputCloseMatchingWindowBtnObservable.Subscribe(_ =>
            {
                SwicthMatchingWindow();
            }).AddTo(this);
        }

        /// <summary>
        /// マッチングウィンドウの表示を切り替える処理
        /// </summary>
        public void SwicthMatchingWindow(bool isView = false)
        {
            matchingWindow.SetActive(isView);

        }
        #endregion
    }
}