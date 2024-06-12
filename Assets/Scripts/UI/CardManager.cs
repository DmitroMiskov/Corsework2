using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Transform grid;


    private List<string> savedCardNames = new List<string>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SaveCardData(string cardName)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"CardName", cardName},
            }
        };

        PlayFabClientAPI.UpdateUserData(request, OnSaveSuccess, OnSaveFailure);
    }

    private void OnSaveSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Дані про карту успішно збережено на PlayFab.");
    }

    private void OnSaveFailure(PlayFabError error)
    {
        Debug.LogError("Помилка збереження даних про карту на PlayFab: " + error.ErrorMessage);
    }

    public void GetSavedCardData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnGetUserDataSuccess, OnGetUserDataFailure);
    }

    private void OnGetUserDataSuccess(GetUserDataResult result)
    {
        if (result.Data.TryGetValue("CardName", out UserDataRecord cardData))
        {
            // Оновити список збережених карток
            savedCardNames.Add(cardData.Value);
        }
    }

    private void OnGetUserDataFailure(PlayFabError error)
    {
        Debug.LogError("Помилка отримання даних про картки з PlayFab: " + error.ErrorMessage);
    }

    public void DisplaySavedCards()
    {
        foreach (string cardName in savedCardNames)
        {
            // Знайти префаб за іменем картки
            GameObject prefab = Resources.Load<GameObject>(cardName);

            if (prefab != null)
            {
                // Створити екземпляр префаба і додати його до Grid
                Instantiate(prefab, grid);
            }
            else
            {
                Debug.LogWarning("Префаб з іменем " + cardName + " не знайдено у папці Resources/Prefabs.");
            }
        }
    }
}
