using Photon.Pun;
using Photon.Realtime;
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

                StartGame();
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

        private void StartGame()
        {
            // ゲームを開始するための処理を実装
            SetPlayerIDs();
        }

        private void SetPlayerIDs()
        {
            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                int playerID = i + 1;
                players[i].CustomProperties["PlayerID"] = i + 1;

                Debug.Log($"Player {players[i].NickName} has ID: {playerID}");
            }
        }
        #endregion
    }
}