using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UniRx;
using UnityEngine;

namespace NetWork
{
    /// <summary>
    /// オンライン対戦のマッチング管理
    /// </summary>
    public class MatchingController : MonoBehaviour
    {
        #region PrivateField
        /// <summary>マッチング完了時の処理</summary>
        public Subject<Unit> MatchingCompletedSubject = new Subject<Unit>();
        #endregion

        #region PrivateField
        /// <summary>マッチング中かどうかの処理</summary>
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

                StartCoroutine(MovePlayersBattleRoom());
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
        }
        #endregion

        #region PrivateMethod
        private IEnumerator MovePlayersBattleRoom()
        {
            // 対戦用の新しいルーム名を動的に生成
            string battleRoomName = "BattleRoom" + Random.Range(1, 1000);

            // 新しいルームを作成
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(battleRoomName, roomOptions);

            // 全てのプレイヤーを新しいルームに移動
            yield return null;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinRoom(battleRoomName);

            MatchingCompletedSubject.OnNext(Unit.Default);
        }
        #endregion
    }
}