using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    [SerializeField] Canvas menuCanvas;
    [SerializeField] GameObject turnCanvas;
    [SerializeField] GameObject submenuCanvas;
    [SerializeField] GameObject collectionCanvas;


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
            collectionCanvas.SetActive(false);
        }
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
