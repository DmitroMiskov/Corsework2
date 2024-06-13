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
        // ���� ��������� ��� ���� � �� �� ��� ���������, ������� ��� ��'���
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // � ������ �������, ������� ��� ��������� ��������� � �� ��������� ���� ��� ����������� ����� ����
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
        Debug.Log("��� ��� ����� ������ ��������� �� PlayFab.");
    }

    private void OnSaveFailure(PlayFabError error)
    {
        Debug.LogError("������� ���������� ����� ��� ����� �� PlayFab: " + error.ErrorMessage);
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
                // ������� ������ ���������� ������
                savedCardNames.Add(cardData.Value);
            }
        }
        CreateImagesOnGrid();
    }

    private void OnGetUserDataFailure(PlayFabError error)
    {
        Debug.LogError("������� ��������� ����� ��� ������ � PlayFab: " + error.ErrorMessage);
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
                    // �������� ��������� ������� � ������ ���� �� Grid
                    Instantiate(prefab, grid.transform);
                }
                else
                {
                    Debug.LogWarning("������ � ������ " + name + " �� �������� � ����� Resources/Prefabs.");
                }
            }
        }
        else
        {
            Debug.LogWarning("��'� ������� �� �������.");
        }
    }
}
