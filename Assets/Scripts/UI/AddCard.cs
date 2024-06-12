using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCard : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private string prefabName;

    private void Start()
    {
        cardManager = GameObject.Find("PlayfabManager").GetComponent<CardManager>();
    }

    public void SetName(string name) 
    {
        prefabName = name;
    }

    public void AddTameAnimals()
    {
        if (!string.IsNullOrEmpty(prefabName))
        {
            cardManager.cardName = prefabName;
            cardManager.AddCardToData();
        }
        else
        {
            Debug.LogWarning("Ім'я префаба не вказано.");
        }
    }
}
