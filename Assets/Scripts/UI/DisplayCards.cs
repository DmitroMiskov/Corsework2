using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCards : MonoBehaviour
{
    [SerializeField] Transform cardParent;

    /*void OnEnable()
    {
        PlayfabManager.OnCardsLoadedEvent += OnCardsLoaded;
    }

    void OnDisable()
    {
        PlayfabManager.OnCardsLoadedEvent -= OnCardsLoaded;
    }*/

    void OnCardsLoaded(List<string> cardNames)
    {
        foreach (string cardName in cardNames)
        {
            GameObject cardPrefab = Resources.Load<GameObject>(cardName);
            if (cardPrefab != null)
            {
                Instantiate(cardPrefab, cardParent);
            }
            else
            {
                Debug.LogError("Не вдалося завантажити префаб карти: " + cardName);
            }
        }
    }
}
