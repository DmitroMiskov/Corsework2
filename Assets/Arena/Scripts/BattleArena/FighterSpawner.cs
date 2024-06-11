using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _player1Side; 
    [SerializeField] private GameObject[] _player2Side;

    public List<Fighter> SpawnPlayer1(Fighter[] player1)
    {
        CheckInputCorrect(player1, _player1Side);
        return SpawnFighters(player1, _player1Side);
    }

    public List<Fighter> SpawnPlayer2(Fighter[] player2)
    {
        CheckInputCorrect(player2, _player2Side);
        return SpawnFighters(player2, _player2Side);
    }
    
    private void CheckInputCorrect(Fighter[] fighter, GameObject[] side) 
    {
        if (fighter.Length > side.Length || fighter.Length == 0) 
        {
            throw new System.Exception("Incorrect Fighter Count");
        }
    }

    private List<Fighter> SpawnFighters(Fighter[] fighterTemplate, GameObject[] spawnPoints) 
    {
        List<Fighter> fighter = new List<Fighter>();
    
        for(int i = 0; i < fighterTemplate.Length; i++) 
        {
            Fighter newFighter = Instantiate(fighterTemplate[i], spawnPoints[i].transform);
            newFighter.CurrentStayPoint(spawnPoints[i]);
            fighter.Add(newFighter);
        }
        return fighter;
    }
}
