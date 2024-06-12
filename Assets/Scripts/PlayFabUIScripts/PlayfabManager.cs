using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.ComponentModel.Design.Serialization;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance { get; private set; }
    /*
        public delegate void OnCardsLoaded(List<string> cardNames);
        public static event OnCardsLoaded OnCardsLoadedEvent;*/

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCard(string nameCard)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> 
            { 
                { "nameCard", nameCard } 
            }

        };

        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);

    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Дані успішно збережено на PlayFab");
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError("Помилка при збереженні даних на PlayFab: " + error.GenerateErrorReport());
    }

    public void LoadCard()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    void OnDataReceived(GetUserDataResult result)
    {
        List<string> nameCards = new List<string>();

        if(result.Data != null && result.Data.ContainsKey("nameCard"))
        {
            nameCards.Add(result.Data["nameCard"].Value);
        }

    }
}
