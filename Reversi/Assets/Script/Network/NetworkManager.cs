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

        #region SerializeField
        /// <summary>オンライン対戦のマッチング管理</summary>
        [SerializeField] private MatchingController matchingController;
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

        /// <summary>
        /// マスターサーバーへの接続が成功した時に呼ばれるコールバック
        /// </summary>
        public override void OnConnectedToMaster()
        {
            // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
        }

        /// <summary>
        /// ゲームサーバーへの接続が成功した時に呼ばれるコールバック
        /// </summary>
        public override void OnJoinedRoom()
        {
            matchingController.MatchingStart();
        }
        #endregion
    }
}