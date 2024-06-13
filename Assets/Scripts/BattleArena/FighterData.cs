using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class FighterData : MonoBehaviour
{
    public string fighterName;
    private static FighterData instance;
    private PhotonView PhotonView;
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private Arena _arena;
    public int playernumber;

    private Dictionary<string, string> playerAnimals = new Dictionary<string, string>();

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        PhotonView = GetComponent<PhotonView>();
    }

    public void setFighterName(string name) 
    {
        fighterName = name;
    }

    public void SendFighter()
    {
        PhotonView.RPC("initializeFighters", RpcTarget.AllBuffered, PhotonNetwork.NickName, fighterName);
    }

    public void InitializeArena()
    {
        _arena = GameObject.Find("ArenaWithoutGrass").GetComponent<Arena>();
        _camera = GameObject.Find("Main Camera");
        SendFighter();
    }

    public void SetAnimals() 
    {
        string player1Animal = playerAnimals.Values.ElementAt(0);
        _arena.SetPlayer1(player1Animal);
    }

    public bool CheckDictionary() 
    {
        if (playerAnimals.Count >= 2)
        {
            return true; 
        }
        else
            return false;
    }

    [PunRPC]
    private void initializeFighters(string nick, string animalName)
    {
        Debug.Log("User: " + nick + " send: " + animalName);
        playernumber = playerAnimals.Count;
        // Перевіряємо, чи існує вже такий ключ у Dictionary
        if (!playerAnimals.ContainsKey(nick))
        {
            playerAnimals[nick] = animalName;
        }
    }

    public void SetCameraAndPosition(Transform newPosition)
    {
        if (playernumber == 2)
        {
            _camera.transform.position = newPosition.position;
            _camera.transform.SetParent(newPosition);
            _camera.transform.localPosition = Vector3.zero;
            _camera.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
    }
}
