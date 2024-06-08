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
        UserAccountManager.OnCreateAccountFailed.AddListener(OnCreateAccountFailed);
        UserAccountManager.OnSignInSuccess.AddListener(OnSignInSuccess);
    }

    void OnDisable()
    {
        UserAccountManager.OnCreateAccountFailed.RemoveListener(OnCreateAccountFailed);
        UserAccountManager.OnSignInSuccess.RemoveListener(OnSignInSuccess);
    }

    void OnCreateAccountFailed(string error)
    {
        errorText.gameObject.SetActive(true);
        errorText.text = error;
    }

    void OnSignInSuccess()
    {
        Debug.Log("Вхід успішний. Деактивуємо канвас.");
        canvas.enabled = false;
    }

    public void UpdateUsername(string _username)
    {
        userName = _username;
        Debug.Log("Оновлено ім'я користувача: " + userName);
    }

    public void UpdatePassword(string _password)
    {
        password = _password;
        Debug.Log("Оновлено пароль.");
    }

    public void UpdateEmailAddress(string _emailAddress)
    {
        emailAddress = _emailAddress;
        Debug.Log("Оновлено адресу електронної пошти: " + emailAddress);
    }

    public void CreateAccount()
    {
        Debug.Log($"Створення облікового запису з ім'ям користувача: {userName}, електронною поштою: {emailAddress}");
        UserAccountManager.Instance.CreateAccount(userName, emailAddress, password);
    }
}
