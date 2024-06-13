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
//        // Отримуємо всіх користувачів у грі
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
//            // Отримуємо PlayFabId кожного користувача
//            string playFabId = userInfo.PlayFabId;

//            // Отримуємо дані користувача за ключем "Wins"
//            GetUserData(playFabId);
//        }
//    }

//    private void OnGetAllUsersFailure(PlayFabError error)
//    {
//        Debug.LogError("Помилка отримання списку користувачів з PlayFab: " + error.ErrorMessage);
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
//                    Debug.Log("Користувач " + playFabId + " має " + winsValue + " перемог.");
//                    // Опрацьовуйте дані як вам потрібно
//                }
//                else
//                {
//                    Debug.LogError("Не вдалося розпізнати значення 'Wins' як число для користувача " + playFabId);
//                }
//            }
//            else
//            {
//                Debug.LogWarning("Дані 'Wins' не знайдено для користувача " + playFabId);
//            }
//        },
//        OnGetUserDataFailure);
//    }

//    private void OnGetUserDataFailure(PlayFabError error)
//    {
//        Debug.LogError("Помилка отримання даних користувача з PlayFab: " + error.ErrorMessage);
//    }
//}
