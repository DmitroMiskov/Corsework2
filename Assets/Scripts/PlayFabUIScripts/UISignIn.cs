using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab.ClientModels;

public class UISignIn : MonoBehaviour
{
    [SerializeField] Text errorText;
    [SerializeField] Canvas canvas;

    string userName, password;

    void OnEnable()
    {
        UserAccountManager.OnSignInFailed.AddListener(OnSignInFailed);
        UserAccountManager.OnSignInSuccess.AddListener(OnSignInSuccess);
    }

    void OnDisable()
    {
        UserAccountManager.OnSignInFailed.AddListener(OnSignInFailed);
        UserAccountManager.OnSignInSuccess.RemoveListener(OnSignInSuccess);
    }

    void OnSignInFailed(string error)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = error;
    }
    
    void OnSignInSuccess()
    {
        canvas.enabled = false;
        SceneManager.LoadScene("Menu");
    }

    public void UpdateUsername(string _username)
    {
        userName = _username;
    }

    public void UpdatePassword(string _password)
    {
        password = _password;
    }

    public void SignIn()
    {
        UserAccountManager.Instance.SignIn(userName, password);
    }
}
