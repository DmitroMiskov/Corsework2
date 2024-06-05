using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    [SerializeField] Canvas menuCanvas;
    [SerializeField] GameObject turnCanvas;
    
    /*void Start()
    {
        if(turnCanvas != null)
        {
            turnCanvas.enabled = false;
        }

        if(menuCanvas != null) 
        { 
            menuCanvas.enabled = true;
        }
    }*/

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
}
