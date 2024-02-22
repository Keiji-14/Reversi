using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace NetWork
{
    /// <summary>
    /// オンライン対戦のマッチング管理
    /// </summary>
    public class MatchingController : MonoBehaviour
    {
        #region PrivateField
        /// <summary></summary>
        private bool isMatching = false;
        /// <summary>ゲームが開始済みかどうか</summary>
        private bool isGameStarted = false;
        #endregion

        #region UnityEvent
        private void Update()
        {
            // ゲームが開始されている場合は何もしない
            if (isGameStarted || !isMatching)
                return;

            // ここに他のプレイヤーがいるかどうかを確認する処理を追加
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                // 他のプレイヤーを待つか、新しいプレイヤーが参加するまで待機
                Debug.Log("Waiting for another player...");
            }
            else
            {
                isGameStarted = true;
                Debug.Log("Matching");
            }
        }
        #endregion

        #region PublicMethod
        /// <summary>
        /// マッチングを開始する処理
        /// </summary>
        public void MatchingStart()
        {
            isMatching = true;

            StartCoroutine(CheckPlayerCount());
        }
        #endregion

        #region PrivateMethod
        private IEnumerator CheckPlayerCount()
        {
            while (true)
            {
                Debug.Log($"Player Count: {PhotonNetwork.CurrentRoom.PlayerCount}");
                yield return new WaitForSeconds(3f); // 3秒ごとにプレイヤー数を確認
            }
        }
        #endregion
    }
}