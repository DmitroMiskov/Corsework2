using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
using System.Net.Mail;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager Instance;

    public static UnityEvent OnSignInSuccess = new UnityEvent();

    public static UnityEvent<string> OnSignInFailed = new UnityEvent<string>();

    public static UnityEvent<string> OnCreateAccounntFailed = new UnityEvent<string>();

    public static UnityEvent<string> OnNicknameRetrieved = new UnityEvent<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void CreateAccount(string userName, string emailAddress, string password)
    {
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest()
            {
                Username = userName,
                Email = emailAddress,
                Password = password

            },
            response =>
            {
                Debug.Log($"Succsefil Account Creation: {userName}, {emailAddress}");
                SignIn(userName, password);
            },
            error =>
            {
                Debug.Log($"Succsefil Account Creation: {userName}, {emailAddress} \n {error.ErrorMessage}");
                OnCreateAccounntFailed.Invoke(error.ErrorMessage);
            }
        );
    }

    public void SignIn(string userName, string password)
    {
        PlayFabClientAPI.LoginWithPlayFab(
            new LoginWithPlayFabRequest()
            {
                Username = userName,
                Password = password
            },
            response =>
            {
                Debug.Log($"Succsefil Account Login: {userName}");
                OnSignInSuccess.Invoke();
            },
            error =>
            {
                Debug.Log($"Unsuccsefil Account Login: {userName} \n {error.ErrorMessage}");
                OnSignInFailed.Invoke(error.ErrorMessage);
            }
        );
    }

    private void GetNickname(string playFabId)
    {
        PlayFabClientAPI.GetPlayerProfile(
            new GetPlayerProfileRequest()
            {
                PlayFabId = playFabId,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true
                }
            },
            result =>
            {
                string nickname = result.PlayerProfile.DisplayName;
                OnNicknameRetrieved.Invoke(nickname);
            },
            error =>
            {
                Debug.LogError($"Failed to retrieve nickname: {error.ErrorMessage}");
            }
        );
    }

}
