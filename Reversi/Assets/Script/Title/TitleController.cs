using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
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
                NetWork.NetworkManager.instance.ConnectUsingSettings();
            }).AddTo(this);
        }
        #endregion
    }
}