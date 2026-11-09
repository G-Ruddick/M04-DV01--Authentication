using System;
using UnityEngine;
using Unity.Services.CloudCode;
using Unity.Services.CloudCode.GeneratedBindings;
using Newtonsoft.Json;
using System.Linq;

namespace Unity.Services.Authentication.PlayerAccounts.Samples {
    public class PlayerDataManager : MonoBehaviour {
        public LoginManager LoginManager;
        private MyModuleBindings authenticationBindings;
        public string PlayerName;

        private void OnEnable() {
        }

        private void OnDisable() {
            LoginManager.PlayerSignedIn -= InitializePlayer;
        }

        void Start() {
            LoginManager.PlayerSignedIn += InitializePlayer;

            authenticationBindings = new MyModuleBindings(CloudCodeService.Instance);
        }

        private async void InitializePlayer() {
            try {
                var playerDataResponse = await authenticationBindings.SayHello(PlayerName);
            }

            catch (CloudCodeException ex) {
                Debug.LogException(ex);
            }
        }

        public async void SaveNewPlayerName() {
            try {
                // var playerDataResponse = await UGSTutorialBindings.SayHello(PlayerName);

                PlayerName = await authenticationBindings.HandleNewPlayerNameEntry(PlayerName);
                Debug.Log($"Saved new player name in the cloud: {PlayerName}");
            }

            catch (Exception ex) {
                Debug.LogException(ex);
            }
        }

        // [CloudCodeFunction("HandleNewPlayerNameEntry")]
        // public string HandleNewPlayerNameEntry(string newName) {
        //     if (IsPlayerNameValid(newName)) {
        //         return newName;
        //     }


        // }

        private bool IsPlayerNameValid(string name) {
            if (name.Length is < 4 or > 16) {
                return false;
            }

            if (!name.All(c => char.IsLetterOrDigit(c))) {
                return false;
            }

            return true;
        }
    }
}