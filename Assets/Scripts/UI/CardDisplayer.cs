using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayer : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;

    void Start()
    {
        cardManager.DisplaySavedCards();
    }
}
