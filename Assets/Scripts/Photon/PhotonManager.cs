using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Data.Common;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string region;
    string nickName = PhotonNetwork.NickName;

    [SerializeField] Transform content;
    [SerializeField] ListItem itemPrefab;

    List<RoomInfo> allRoomsInfo = new List<RoomInfo>();

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    private void OnNicknameRetrieved(string nickName)
    {
        PhotonNetwork.NickName = nickName;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("�� �������� ��: " + PhotonNetwork.CloudRegion);
        if(nickName == "")
        {
            PhotonNetwork.NickName = "User";
        }
        else
        {
            PhotonNetwork.NickName = nickName;
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
        foreach(RoomInfo info in  roomList)
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
