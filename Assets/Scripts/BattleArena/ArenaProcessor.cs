using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using WebSocketSharp;

public class ArenaProcessor : MonoBehaviour
{
    [SerializeField]
    private FighterData _fighterData;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Arena _arena;
    private List<string> animalsName = new List<string>();

    private void Start()
    {
        _arena = GetComponent<Arena>();
        _camera = GameObject.Find("MainCamera").GetComponent<Camera>();
        _fighterData = GameObject.Find("SaveFighterData").GetComponent<FighterData>();
        _fighterData.SendFighter();
    }

    [PunRPC]
    private void initializeFighters(string nick, string animalName)
    {
        Debug.Log("User: " + nick + "send: " + animalName);
        animalsName.Add(animalName);
    }

}
