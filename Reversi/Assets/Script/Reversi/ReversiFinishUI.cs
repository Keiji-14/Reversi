using NetWork;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Reversi
{
    /// <summary>
    /// オセロゲーム終了のUI
    /// </summary>
    public class ReversiFinishUI : MonoBehaviour
    {
        #region PrivateField
        /// <summary>タイトルに戻る時の処理</summary>
        public Subject<Unit> TitleBackSubject = new Subject<Unit>();
        #endregion

        #region PrivateField
        /// <summary>タイトルに戻るボタンを押した時の処理</summary>
        private IObservable<Unit> InputTitleBackBtnObservable =>
            titleBackBtn.OnClickAsObservable();
        #endregion

        #region SerializeField
        /// <summary>タイトルに戻るボタン</summary>
        [SerializeField] private Button titleBackBtn;
        /// <summary>終了時に表示するウィンドウ</summary>
        [SerializeField] private GameObject finishWindowObj;
        /// <summary>勝った時に表示するUIオブジェクト</summary>
        [SerializeField] private GameObject youWinUIObj;
        /// <summary>負けた時に表示するUIオブジェクト</summary>
        [SerializeField] private GameObject youLoseUIObj;
        /// <summary>引き分け時に表示するUIオブジェクト</summary>
        [SerializeField] private GameObject deowUIObj;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            finishWindowObj.SetActive(false);

            // タイトルに戻るボタンを押した時の処理
            InputTitleBackBtnObservable.Subscribe(_ =>
            {
                TitleBackSubject.OnNext(Unit.Default);
            }).AddTo(this);
        }

        /// <summary>
        /// 終了時に表示するUI
        /// </summary>
        public void FinishWindowUI()
        {
            finishWindowObj.SetActive(true);
        }

        /// <summary>
        /// 勝った時に表示するUI
        /// </summary>
        public void YouWinUI()
        {
            youWinUIObj.SetActive(true);
        }

        /// <summary>
        /// 負けた時に表示するUI
        /// </summary>
        public void YouLoseUI()
        {
            youLoseUIObj.SetActive(true);
        }

        /// <summary>
        /// 引き分け時に表示するUI
        /// </summary>
        public void DrowUI()
        {
            deowUIObj.SetActive(true);
        }
        #endregion
    }
}