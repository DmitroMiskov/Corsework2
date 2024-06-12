using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class FighterUI : MonoBehaviour
{
    [SerializeField] private Slider sliderHP;
    public Button firstSkill;
    public Button secondSkill;
    public Button thirdSkill;

    public int skillNumber = 0;

    private float _maxHealth;

    private void Start()
    {
        firstSkill = GameObject.Find("FirstSkill").GetComponent<Button>();
        secondSkill = GameObject.Find("SecondSkill").GetComponent<Button>();
        thirdSkill = GameObject.Find("ThirdSkill").GetComponent<Button>();

        firstSkill.onClick.AddListener(FirstSkill);
        secondSkill.onClick.AddListener(SecondSkill);
        thirdSkill.onClick.AddListener(ThirdSkill);
    }

    private void OnDestroy()
    {
        firstSkill.onClick.RemoveAllListeners();
        secondSkill.onClick.RemoveAllListeners();
        thirdSkill.onClick.RemoveAllListeners();
    }
    public void SetMaxHp(float Hp)
    {
        _maxHealth = Hp;
    }

    public void UpdateHealth(float currentHealth)
    {
        if (sliderHP != null)
        {
            sliderHP.value = currentHealth / _maxHealth;
        }
        else
        {
            Debug.LogError("Slider for HP is not assigned!");
        }
    }
    public void FirstSkill()
    {
        skillNumber = 1;
        firstSkill.interactable = false;
        secondSkill.interactable = false;
        thirdSkill.interactable = false;
    }

    public void SecondSkill()
    {
        skillNumber = 2;
        firstSkill.interactable = false;
        secondSkill.interactable = false;
        thirdSkill.interactable = false;
    }

    public void ThirdSkill()
    {
        skillNumber = 3;
        firstSkill.interactable = false;
        secondSkill.interactable = false;
        thirdSkill.interactable = false;
    }
}
