//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using PlayFab;
//using PlayFab.ClientModels;

//public class RankingSystem : MonoBehaviour
//{
//    [SerializeField] private Transform prefabPosition; 
//    [SerializeField] private GameObject rankPrefab;

//    private void Start()
//    {
//        // �������� ��� ������������ � ��
//        GetAllUsers();
//    }

//    private void GetAllUsers()
//    {
//        var request = new GetAllUsersRequest();

//        PlayFabClientAPI.Getuse
//    }

//    private void OnGetAllUsersSuccess(GetAllUsersResult result)
//    {
//        foreach (var userInfo in result.Users)
//        {
//            // �������� PlayFabId ������� �����������
//            string playFabId = userInfo.PlayFabId;

//            // �������� ��� ����������� �� ������ "Wins"
//            GetUserData(playFabId);
//        }
//    }

//    private void OnGetAllUsersFailure(PlayFabError error)
//    {
//        Debug.LogError("������� ��������� ������ ������������ � PlayFab: " + error.ErrorMessage);
//    }

//    private void GetUserData(string playFabId)
//    {
//        var request = new GetUserDataRequest
//        {
//            PlayFabId = playFabId
//        };

//        PlayFabClientAPI.GetUserData(request, result =>
//        {
//            if (result.Data.TryGetValue("Wins", out UserDataRecord value))
//            {
//                if (float.TryParse(value.Value, out float winsValue))
//                {
//                    Debug.Log("���������� " + playFabId + " �� " + winsValue + " �������.");
//                    // ������������ ��� �� ��� �������
//                }
//                else
//                {
//                    Debug.LogError("�� ������� ��������� �������� 'Wins' �� ����� ��� ����������� " + playFabId);
//                }
//            }
//            else
//            {
//                Debug.LogWarning("��� 'Wins' �� �������� ��� ����������� " + playFabId);
//            }
//        },
//        OnGetUserDataFailure);
//    }

//    private void OnGetUserDataFailure(PlayFabError error)
//    {
//        Debug.LogError("������� ��������� ����� ����������� � PlayFab: " + error.ErrorMessage);
//    }
//}
