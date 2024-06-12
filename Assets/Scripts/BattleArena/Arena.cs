using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arena : MonoBehaviour
{
    [SerializeField] private Fighter[] _player1Template;
    [SerializeField] private Fighter[] _player2Template;

    private List<Fighter> _player1;
    private List<Fighter> _player2;
    private Queue<Fighter> _readyToFight;
    private FighterSpawner _spawner;

    private void Awake()
    {
        _spawner = GetComponent<FighterSpawner>();
        _readyToFight = new Queue<Fighter>();
    }

    public void SetPlayer1(List<Fighter> player1) 
    {
        _player1Template = new Fighter[player1.Count];
        for (int i = 0; i < player1.Count; i++)
        {
            _player1Template[i] = player1[i];
        }
    }

    public void SetPlayer2(List<Fighter> player2)
    {
        _player2Template = new Fighter[player2.Count];
        for (int i = 0; i < player2.Count; i++) 
        {
            _player2Template[i] = player2[i];
        }
    }

    private void Start()
    {
        _player1 = _spawner.SpawnPlayer1(_player1Template);
        _player2 = _spawner.SpawnPlayer2(_player2Template);

        InitialiseFighter(_player2);
        InitialiseFighter(_player1);

        StartCoroutine(Battle());
    }

    private IEnumerator Battle() 
    {
        while (_player1.Count > 0 && _player2.Count > 0) 
        {
            var nextFighter = GetNextFighter();

            if (nextFighter != null) 
            {
                if (_player1.Contains(nextFighter))
                {
                    nextFighter._ui.firstSkill.interactable = true;
                    nextFighter._ui.secondSkill.interactable = true;
                    nextFighter._ui.thirdSkill.interactable = true;
                    nextFighter.SetTarget(GetRandomFighter(_player2));
                    yield return StartCoroutine(WaitForSkillSelection(nextFighter));
                }
                else
                {
                    nextFighter._ui.firstSkill.interactable = false;
                    nextFighter._ui.secondSkill.interactable = false;
                    nextFighter._ui.thirdSkill.interactable = false;
                    nextFighter.SetTarget(GetRandomFighter(_player1));
                    yield return nextFighter.FirstSkill();
                    nextFighter.TurnMeter(true);
                }
            }

            yield return new WaitForSeconds(1f);

            IncreaseTurnMeter(_player1);
            IncreaseTurnMeter(_player2);
        }
        if (_player1.Count > 0) 
        {
            Debug.Log("Battle win"); 
            //Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Battle lose");
            //Destroy(this.gameObject);
        }
    }
    private void IncreaseTurnMeter(List<Fighter> fighter) 
    {
        fighter.ForEach(f => f.TurnMeter(false));
    } 

    private Fighter GetNextFighter() 
    {
        if (_readyToFight.Count > 0)
        {
            return _readyToFight.Dequeue();
        }
        else
        {
            return null;
        }
    }

    private Fighter GetRandomFighter(List<Fighter> fighters) 
    {
        return fighters[Random.Range(0, fighters.Count)];
    } 

    private void OnDied(Fighter fighter)
    {
        fighter.TurnMeterFilled -= OnTurnMeterFilled;
        fighter.Died -= OnDied;
        DeleteFighter(fighter);
    }

    private void DeleteFighter(Fighter fighter) 
    {
        if (_readyToFight.Contains(fighter))
        {
            List<Fighter> remainingFighters = new List<Fighter>();

            // Переносимо усіх бійців які не є тим кого ми хочемо видалити до тимчасового списку
            while (_readyToFight.Count > 0)
            {
                Fighter nextFighter = _readyToFight.Dequeue();
                if (nextFighter != fighter)
                {
                    remainingFighters.Add(nextFighter);
                }
            }

            // Повертаємо усіх залишених бійців назад в чергу
            foreach (Fighter remainingFighter in remainingFighters)
            {
                _readyToFight.Enqueue(remainingFighter);
            }
        }
        _player1.Remove(fighter);
        _player2.Remove(fighter);
    }

    private IEnumerator WaitForSkillSelection(Fighter fighter)
    {
        // Wait until a skill button is clicked
        yield return new WaitUntil(() => fighter._ui.skillNumber > 0);

        // Execute the selected skill
        switch (fighter._ui.skillNumber)
        {
            case 1:
                yield return fighter.FirstSkill();
                break;
            case 2:
                yield return fighter.SecondSkill();
                break;
            case 3:
                yield return fighter.ThirdSkill();
                break;
        }

        fighter.TurnMeter(true);
        fighter._ui.skillNumber = 0;
    }
    private void OnTurnMeterFilled(Fighter fighter)
    {
        if (_readyToFight.Contains(fighter) == false) 
        {
            _readyToFight.Enqueue(fighter);
        }
    }

    private void InitialiseFighter(List<Fighter> Fighters) 
    {
        foreach (var fighter in Fighters)
        {
            fighter.TurnMeterFilled += OnTurnMeterFilled;
            fighter.Died += OnDied;
        }
    } 
}
