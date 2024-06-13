using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Transform grid;

    private List<string> savedCardNames = new List<string>();

    public string cardName = "";

    private static CardManager instance;

    private void Start()
    {
        // Якщо екземпляр вже існує і це не цей екземпляр, знищити цей об'єкт
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // В іншому випадку, зробити цей екземпляр унікальним і не знищувати його при завантаженні нових сцен
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        FindGrid();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindGrid();
    }

    private void FindGrid()
    {
        GameObject cardCollection = GameObject.Find("CardCollection");
        if (cardCollection != null)
        {
            grid = cardCollection.transform;

            GameObject.Find("SceneManager").GetComponent<CanvasSwitcher>().SwitchCanvasCollectionExit();
        }
        else
        {
            Debug.LogError("CardCollection object not found with tag 'CardCollection' in the scene.");
        }
    }

    public void AddCardToData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), SaveCardData, OnGetUserDataFailure);
    }

    private void SaveCardData(GetUserDataResult result)
    {
        int currentCount = result.Data.Count;

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"CardName" + currentCount.ToString(), cardName}
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
        int tmp = result.Data.Count;
        for (int i = 0; i < tmp; i++) 
        {
            if (result.Data.TryGetValue("CardName" + i.ToString(), out UserDataRecord cardData))
            {
                // Оновити список збережених карток
                savedCardNames.Add(cardData.Value);
            }
        }
        CreateImagesOnGrid();
    }

    private void OnGetUserDataFailure(PlayFabError error)
    {
        Debug.LogError("Помилка отримання даних про картки з PlayFab: " + error.ErrorMessage);
    }

    public void ClearList()
    {
        savedCardNames.Clear();
    }

    public void CreateImagesOnGrid()
    {
        if (savedCardNames != null)
        {
            foreach (var name in savedCardNames)
            {
                GameObject prefab = Resources.Load<GameObject>(name);

                if (prefab != null)
                {
                    // Створити екземпляр префаба і додати його до Grid
                    Instantiate(prefab, grid.transform);
                }
                else
                {
                    Debug.LogWarning("Префаб з іменем " + name + " не знайдено у папці Resources/Prefabs.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Ім'я префаба не вказано.");
        }
    }
}
