using UnityEngine;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace Unity.Services.Authentication.PlayerAccounts.Samples {
    public class LoginManager : MonoBehaviour {
        public Action PlayerSignedIn;

        private async void Awake() {
            // if (UnityServices.State == ServicesInitializationState.Uninitialized) {
            //     Debug.Log("Services Initializing");
            //     await UnityServices.InitializeAsync();
            // }

            // PlayerAccountService.Instance.SignedIn += SignInOrLinkWithUnity;

            if (UnityServices.State == ServicesInitializationState.Initialized) {
                UnitySignInSubscription();
            }
            else {
                UnityServices.Initialized += UnitySignInSubscription;
            }
        }

        private async void Start() {
            if (!AuthenticationService.Instance.SessionTokenExists) {
                return;
            }

            await SignInAnonymouslyAsync();
        }

        private void OnDestroy() {
            PlayerAccountService.Instance.SignedIn -= SignInOrLinkWithUnity;
            UnityServices.Initialized -= UnitySignInSubscription;
        }

        private void UnitySignInSubscription() {
            PlayerAccountService.Instance.SignedIn += SignInOrLinkWithUnity;
        }
        
        public async void StartAnonymousSignIn() {
            await SignInAnonymouslyAsync();
        }

        private async Task SignInAnonymouslyAsync() {
            try {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("Sign in anonymously succeeded!");
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");


                PlayerSignedIn.Invoke();
            }
            catch (AuthenticationException ex) {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex) {
                Debug.LogException(ex);
            }
        }

        public async void StartUnitySignInAsync() {
            if (PlayerAccountService.Instance.IsSignedIn) {
                SignInOrLinkWithUnity();
                return;
            }

            try {
                await PlayerAccountService.Instance.StartSignInAsync();
            }
            catch (RequestFailedException ex) {
                Debug.LogException(ex);
            }
        }

        async void SignInOrLinkWithUnity() {
            try {
                if (!AuthenticationService.Instance.IsSignedIn) {
                    Debug.Log("Signing up with Unity Player Account...");

                    await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
                    
                    Debug.Log("Successfully signed up with Unity Player Account");
                    return;
                }

                if (!HasUnityID()) {
                    Debug.Log("Linking anonymous account to Unity...");
                    Debug.Log("Successfully linked anonymous account!");

                    return;
                }

                Debug.Log("Player is already signed in to their Unity Player Account");
            }
            catch (RequestFailedException ex) {
                Debug.LogException(ex);
            }
        }

        private bool HasUnityID() {
            return AuthenticationService.Instance.PlayerInfo.GetUnityId() != null;
        }
    }
}