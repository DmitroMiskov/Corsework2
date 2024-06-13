using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string region;
    [SerializeField] private string nickName;

    List<RoomInfo> allRoomsInfo = new List<RoomInfo>();

    private static PhotonManager instance;

    void Awake()
    {
        Debug.Log("ϳ��������� �� Photon...");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    void Start()
    {
        UserAccountManager.OnNicknameRetrieved.AddListener(OnNicknameRetrieved);

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnNicknameRetrieved(string name)
    {
        nickName = name;
        PhotonNetwork.NickName = nickName;
        Debug.Log("�������� Photon �����������: " + nickName);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("�� �������� ��: " + PhotonNetwork.CloudRegion);
        if (string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            PhotonNetwork.NickName = "User";
            Debug.Log("�������� Photon ��� �������, ����������� �� �������������: User");
        }

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("�� �������� �� �������!!!");
    }

    public void PlayButton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("�� ������� ���������� �� ��������� ������, ������� ����.");
        CreateRoom();
    }

    public void CreateRoom()
    {
        string roomName = "Room_" + Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("ʳ����� �������� ��� ������:" + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("game_scene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("�� ������� �������� ������!!!" + message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                allRoomsInfo.Remove(info);
            }
            else
            {
                allRoomsInfo.Add(info);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("���������� �� ������: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("game_scene");
    }

    public void JoinRandRoomButton()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }
}
