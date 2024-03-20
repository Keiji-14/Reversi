﻿using System.Collections.Generic;
using UnityEngine;

namespace Audio
{    /// <summary>
     /// 効果音の再生処理
     /// </summary>
    public class SE : MonoBehaviour
    {
        #region PublicField
        public static SE instance = null;
        #endregion

        #region SerializeField
        [SerializeField] private AudioSource audioSource;
        /// <summary>効果音リスト</summary>
        [SerializeField] private List<AudioClip> seClipList;
        #endregion

        #region UnityEvent
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region PublicMethod
        public enum SEName
        {
            ButtonSE,
            MatchingSE,
            PlaceStoneSE,
        }

        /// <summary>
        /// SEを再生
        /// </summary>
        /// <param name="seName">効果音名</param>
        public void Play(SEName seName)
        {
            audioSource.PlayOneShot(seClipList[(int)seName]);
        }
        #endregion
    }
}