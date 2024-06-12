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
                // �������� ��������� ������� � ������ ���� �� Grid
                GameObject instance = Instantiate(prefab, grid.transform);

                // ��������� ����� SaveCardData � CardManager
                cardManager.SaveCardData(prefabName);
            }
            else
            {
                Debug.LogWarning("������ � ������ " + prefabName + " �� �������� � ����� Resources/Prefabs.");
            }
        }
        else
        {
            Debug.LogWarning("��'� ������� �� �������.");
        }
    }
}
