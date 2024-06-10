using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
using System.Net.Mail;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager Instance;

    public static UnityEvent OnSignInSuccess = new UnityEvent();
    public static UnityEvent<string> OnSignInFailed = new UnityEvent<string>();
    public static UnityEvent<string> OnCreateAccountFailed = new UnityEvent<string>();
    public static UnityEvent<string> OnNicknameRetrieved = new UnityEvent<string>();
    public static UnityEvent OnSignOutSuccess = new UnityEvent();

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
                Debug.Log($"������ ��������� ��������� ������: {userName}, {emailAddress}");
                UpdateDisplayName(userName, response.PlayFabId); // ������ ��������� �������
                SignIn(userName, password);
            },
            error =>
            {
                Debug.Log($"�� ������� �������� �������� �����: {userName}, {emailAddress} \n {error.ErrorMessage}");
                OnCreateAccountFailed.Invoke(error.ErrorMessage);
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
                Debug.Log($"������� ���� � �������� �����: {userName}");
                GetUserDisplayName(response);
                OnSignInSuccess.Invoke();
            },
            error =>
            {
                Debug.Log($"��������� ���� � �������� �����: {userName} \n {error.ErrorMessage}");
                OnSignInFailed.Invoke(error.ErrorMessage);
            }
        );
    }

    private void GetUserDisplayName(LoginResult result)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
        {
            PlayFabId = result.PlayFabId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true
            }
        }, profileResult =>
        {
            string displayName = profileResult.PlayerProfile?.DisplayName ?? "User";
            Debug.Log($"�������� ������ � PlayFab: {displayName}");
            OnNicknameRetrieved.Invoke(displayName);
        }, error =>
        {
            Debug.LogError($"�� ������� �������� ������: {error.ErrorMessage}");
            OnNicknameRetrieved.Invoke("User");
        });
    }

    private void UpdateDisplayName(string userName, string playFabId)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = userName
        }, result =>
        {
            Debug.Log($"ͳ����� ��������: {result.DisplayName}");
        }, error =>
        {
            Debug.LogError($"�� ������� ������� ������: {error.ErrorMessage}");
        });
    }

    public void SingOut()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        Debug.Log("����� � ��������");
        OnSignOutSuccess.Invoke();
        SceneManager.LoadScene("Registration");
    }
}
