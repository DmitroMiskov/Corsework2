using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICreateAccount : MonoBehaviour
{
    [SerializeField] Text errorText;
    [SerializeField] Canvas canvas;

    string userName, password, emailAddress;

    void OnEnable()
    {
        UserAccountManager.OnCreateAccounntFailed.AddListener(OnCreateAccountFailed);
        UserAccountManager.OnSignInSuccess.AddListener(OnSignInSuccess);
    }
    
    void OnDisable()
    {
        UserAccountManager.OnCreateAccounntFailed.AddListener(OnCreateAccountFailed);
        UserAccountManager.OnSignInSuccess.RemoveListener(OnSignInSuccess);
    }

    void OnCreateAccountFailed(string error)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = error;
    }

    void OnSignInSuccess()
    {
        canvas.enabled = false;
    }

    public void UpdateUsername(string _username)
    {
        userName = _username;
    }

    public void UpdatePassword(string _password)
    {
        password = _password;
    }

    public void UpdateEmailAddress(string _emailAddress)
    {
        emailAddress = _emailAddress;
    }

    public void CreateAccount()
    {
        UserAccountManager.Instance.CreateAccount(userName, emailAddress, password);
    }
}
