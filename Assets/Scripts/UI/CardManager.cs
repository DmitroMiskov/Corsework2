using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] PlayfabManager playfabManager;
    [SerializeField] public string cardName = "Animals1";
    [SerializeField] public Transform grid;

    public void AddCard()
    {
        GameObject cardPrefab = Resources.Load<GameObject>(cardName);

        if (cardPrefab != null)
        {
            GameObject newCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);

            newCard.transform.SetParent(grid, false);

            playfabManager.SaveCard(cardName);
        }
        else
        {
            Debug.LogError("Card prefab named '" + cardName + "' not found in Resources/Cards folder!");
        }
    }
}
