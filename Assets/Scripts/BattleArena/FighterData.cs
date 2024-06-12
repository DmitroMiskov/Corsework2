using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class FighterData : MonoBehaviour
{
    public string fighterName;

    private static FighterData instance;

    private PhotonView PhotonView;

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
}
