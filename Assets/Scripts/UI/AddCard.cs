using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCard : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private GameObject grid;
    [SerializeField] private string prefabName;

    public void AddPrefabToGrid()
    {
        if (!string.IsNullOrEmpty(prefabName))
        {
         
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            if (prefab != null)
            {
                // Створити екземпляр префаба і додати його до Grid
                GameObject instance = Instantiate(prefab, grid.transform);

                // Викликати метод SaveCardData з CardManager
                cardManager.SaveCardData(prefabName);
            }
            else
            {
                Debug.LogWarning("Префаб з іменем " + prefabName + " не знайдено у папці Resources/Prefabs.");
            }
        }
        else
        {
            Debug.LogWarning("Ім'я префаба не вказано.");
        }
    }
}
