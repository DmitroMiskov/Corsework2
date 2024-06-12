using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private GameObject turnCanvas;
    [SerializeField] private GameObject submenuCanvas;
    [SerializeField] private GameObject collectionCanvas;
    [SerializeField] private GameObject messages;

    private bool isActive = false;

    public void SwitchCanvas()
    {
        if(menuCanvas != null)
        {
            menuCanvas.enabled = false;
        }

        if(turnCanvas != null)
        {
            turnCanvas.SetActive(true);
        }
    }

    public void SwitchCanvasSubmenu()
    {
        if (menuCanvas != null)
        {
            menuCanvas.enabled = true;
        }

        if (turnCanvas != null)
        {
            turnCanvas.SetActive(false);
        }

        if(submenuCanvas != null)
        {
            submenuCanvas.SetActive(true);
        }
    }

    public void SwitchCanvasBack()
    {
        if (menuCanvas != null)
        {
            menuCanvas.enabled = true;
        }

        if (turnCanvas != null)
        {
            turnCanvas.SetActive(false);
        }

        if (submenuCanvas != null)
        {
            submenuCanvas.SetActive(false);
        }
    }

    public void SwitchCanvasCollection()
    {
        if (menuCanvas != null)
        {
            menuCanvas.enabled = false;
        }

        if (turnCanvas != null)
        {
            turnCanvas.SetActive(false);
        }

        if (submenuCanvas != null)
        {
            submenuCanvas.SetActive(false);
        }

        if (collectionCanvas != null)
        {
            collectionCanvas.SetActive(true);
        }
    }

    public void SwitchCanvasCollectionExit()
    {
        if (menuCanvas != null)
        {
            menuCanvas.enabled = true;
        }

        if (turnCanvas != null)
        {
            turnCanvas.SetActive(false);
        }

        if (submenuCanvas != null)
        {
            submenuCanvas.SetActive(false);
        }

        if (collectionCanvas != null)
        {
            GameObject cardCollection = GameObject.Find("CardCollection");
            if (cardCollection != null)
            {
                // Отримати всі дочірні об'єкти
                foreach (Transform child in cardCollection.transform)
                {
                    // Знищити дочірні об'єкти
                    Destroy(child.gameObject);
                }
            }
            else
            {
                Debug.LogError("CardCollection object not found!");
            }

            GameObject.Find("PlayfabManager").GetComponent<CardManager>().ClearList();
            
            collectionCanvas.SetActive(false);
        }
    }

    public void OffOnMessages() 
    {
        isActive = !isActive;
        messages.gameObject.SetActive(isActive);
    }

    public void ExitGame()
    {
        Debug.Log("Вихід з гри!");
        Application.Quit();

        // Додатково, для перевірки в редакторі Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SignOut()
    {
        UserAccountManager.Instance.SingOut();
    }
}
