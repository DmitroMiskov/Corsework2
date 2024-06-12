using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFighterButton : MonoBehaviour
{
    [SerializeField] private Button fighterButton;
    [SerializeField] private string fighterName;
    [SerializeField] private Image fighterimage;

    private void Start()
    {
        fighterButton.onClick.AddListener(SetFighter);
        fighterimage = GameObject.Find("ImageAnimal").GetComponent<Image>();
    }

    private void OnDestroy()
    {
        fighterButton.onClick.RemoveAllListeners();
    }

    public void SetFighter() 
    {
        var image = this.GetComponent<Image>().sprite;
        var color = this.GetComponent<Image>().color;
        fighterimage.sprite = image;
        fighterimage.color = color;

        GameObject.Find("SaveFighterData").GetComponent<FighterData>().setFighterName(fighterName);
    }
}
