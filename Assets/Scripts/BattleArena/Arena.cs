using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using PlayFab.ClientModels;
using PlayFab;
using UnityEditor.SearchService;


public class Arena : MonoBehaviour
{
    [SerializeField] private Fighter[] _player1Template;
    [SerializeField] private Fighter[] _player2Template;
    [SerializeField] private GameObject player2Cam;
    [SerializeField] private FighterData fighterData;

    private List<Fighter> _player1;
    private List<Fighter> _player2;
    private Queue<Fighter> _readyToFight;
    private FighterSpawner _spawner;

    private Fighter CurrentFighter;
    private int currentPlayerTurn;

    private PhotonView PhotonView;

    public string currentPlayerPoints;

    private void Awake()
    {
        _spawner = GetComponent<FighterSpawner>();
        _readyToFight = new Queue<Fighter>();
        PhotonView = GetComponent<PhotonView>();
        currentPlayerTurn = 1;
    }

    public void SetPlayer1(string name) 
    {
        string path = "ArenaPrefabs/" + name;
        Fighter player1 = Resources.Load<Fighter>(path);

        _player1Template = new Fighter[1];
        _player1Template[0] = player1;
    }

    public void SetPlayer2(string name)
    {
        string path = "ArenaPrefabs/" + name;
        Fighter player2 = Resources.Load<Fighter>(path);

        _player2Template = new Fighter[1];
        _player2Template[0] = player2;
    }

    private void Start()
    {
        fighterData = GameObject.Find("SaveFighterData").GetComponent<FighterData>();

        fighterData.InitializeArena();
        fighterData.SetAnimals();

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
            CurrentFighter = GetNextFighter();
            
            if (CurrentFighter != null)
            {
                if (_player1.Contains(CurrentFighter))
                {
                    CurrentFighter._ui.firstSkill.interactable = true;
                    CurrentFighter._ui.secondSkill.interactable = true;
                    CurrentFighter._ui.thirdSkill.interactable = true;
                    CurrentFighter.SetTarget(GetRandomFighter(_player2));
                    yield return StartCoroutine(WaitForSkillSelection(CurrentFighter));
                }
                else
                {
                    CurrentFighter.SetTarget(GetRandomFighter(_player1));
                    yield return CurrentFighter.FirstSkill();
                    CurrentFighter.TurnMeter(true);
                }
            }

            yield return new WaitForSeconds(1f);

            IncreaseTurnMeter(_player1);
            IncreaseTurnMeter(_player2);
        }
        if (_player1.Count > 0)
        {
            Debug.Log("Battle win by player 1");
            GetSavedValue();
            SceneManager.LoadScene(1);
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

        PhotonView.RPC("SetSkillNumber", RpcTarget.AllBuffered, fighterData.playernumber, fighter._ui.skillNumber);

        fighter._ui.firstSkill.interactable = false;
        fighter._ui.secondSkill.interactable = false;
        fighter._ui.thirdSkill.interactable = false;

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

        // Перемикання ходу
        PhotonView.RPC("SwitchTurn", RpcTarget.All);
    }
    private void OnTurnMeterFilled(Fighter fighter)
    {
        if (_readyToFight.Contains(fighter) == false) 
        {
            _readyToFight.Enqueue(fighter);
        }
    }

    [PunRPC]
    private void SwitchTurn()
    {
        currentPlayerTurn = currentPlayerTurn == 1 ? 2 : 1;
        Debug.Log("Current player turn: " + currentPlayerTurn);
    }

    [PunRPC]
    private void SetSkillNumber(int playernumber, int number) 
    {
        Debug.Log("User: " + playernumber.ToString() + " send: " + number.ToString());
        CurrentFighter._ui.skillNumber = number;
    }

    private void InitialiseFighter(List<Fighter> Fighters) 
    {
        foreach (var fighter in Fighters)
        {
            fighter.TurnMeterFilled += OnTurnMeterFilled;
            fighter.Died += OnDied;
        }
    }

    public void AddValueToData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), SaveCardData, OnGetValueFailure);
    }

    private void SaveCardData(GetUserDataResult result)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Wins", currentPlayerPoints}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnSaveSuccess, OnSaveFailure);
    }

    private void OnSaveSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Дані про value успішно збережено на PlayFab.");
    }

    private void OnSaveFailure(PlayFabError error)
    {
        Debug.LogError("Помилка збереження даних про value на PlayFab: " + error.ErrorMessage);
    }

    public void GetSavedValue()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnGetValueSuccess, OnGetValueFailure);
    }

    private void OnGetValueSuccess(GetUserDataResult result)
    {
        if (result.Data.TryGetValue("Wins", out UserDataRecord value))
        {
            if (float.TryParse(value.Value, out float winsValue))
            {
                float newValue = winsValue + 1;
                currentPlayerPoints = newValue.ToString();
                AddValueToData();
            }
            
        }
    }

    private void OnGetValueFailure(PlayFabError error)
    {
        Debug.LogError("Помилка отримання даних про value з PlayFab: " + error.ErrorMessage);
    }
}
