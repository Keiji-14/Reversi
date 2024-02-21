using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NetWork
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        #region PublicField
        public static NetworkManager instance = null;
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
        public void ConnectUsingSettings()
        {
            // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
            PhotonNetwork.ConnectUsingSettings();
        }

        // マスターサーバーへの接続が成功した時に呼ばれるコールバック
        public override void OnConnectedToMaster()
        {
            // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
        }

        // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
        public override void OnJoinedRoom()
        {
            // ルームに入った時の処理
            Debug.Log("Joined Room");

            // ルーム内のプレイヤー数を確認
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                // 他のプレイヤーを待つか、新しいプレイヤーが参加するまで待機
                Debug.Log("Waiting for another player...");
            }
            else
            {
                // 他のプレイヤーがいる場合、ゲームを開始
                StartGame();
            }
        }
        #endregion

        #region PrivateMethod
        private void StartGame()
        {
            // ゲームを開始するための処理を実装
            Debug.Log("Starting the game!");
        }
        #endregion
    }
}