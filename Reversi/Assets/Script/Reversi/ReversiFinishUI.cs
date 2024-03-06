using NetWork;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Reversi
{
    /// <summary>
    /// �I�Z���Q�[���I����UI
    /// </summary>
    public class ReversiFinishUI : MonoBehaviour
    {
        #region PrivateField
        /// <summary>�^�C�g���ɖ߂鎞�̏���</summary>
        public Subject<Unit> TitleBackSubject = new Subject<Unit>();
        #endregion

        #region PrivateField
        /// <summary>�^�C�g���ɖ߂�{�^�������������̏���</summary>
        private IObservable<Unit> InputTitleBackBtnObservable =>
            titleBackBtn.OnClickAsObservable();
        #endregion

        #region SerializeField
        /// <summary>�^�C�g���ɖ߂�{�^��</summary>
        [SerializeField] private Button titleBackBtn;
        /// <summary>�I�����ɕ\������E�B���h�E</summary>
        [SerializeField] private GameObject finishWindowObj;
        /// <summary>���������ɕ\������UI�I�u�W�F�N�g</summary>
        [SerializeField] private GameObject youWinUIObj;
        /// <summary>���������ɕ\������UI�I�u�W�F�N�g</summary>
        [SerializeField] private GameObject youLoseUIObj;
        /// <summary>�����������ɕ\������UI�I�u�W�F�N�g</summary>
        [SerializeField] private GameObject deowUIObj;
        #endregion

        #region PublicMethod
        /// <summary>
        /// ������
        /// </summary>
        public void Init()
        {
            finishWindowObj.SetActive(false);

            // �^�C�g���ɖ߂�{�^�������������̏���
            InputTitleBackBtnObservable.Subscribe(_ =>
            {
                TitleBackSubject.OnNext(Unit.Default);
            }).AddTo(this);
        }

        /// <summary>
        /// �I�����ɕ\������UI
        /// </summary>
        public void FinishWindowUI()
        {
            finishWindowObj.SetActive(true);
        }

        /// <summary>
        /// ���������ɕ\������UI
        /// </summary>
        public void YouWinUI()
        {
            youWinUIObj.SetActive(true);
        }

        /// <summary>
        /// ���������ɕ\������UI
        /// </summary>
        public void YouLoseUI()
        {
            youLoseUIObj.SetActive(true);
        }

        /// <summary>
        /// �����������ɕ\������UI
        /// </summary>
        public void DrowUI()
        {
            deowUIObj.SetActive(true);
        }
        #endregion
    }
}