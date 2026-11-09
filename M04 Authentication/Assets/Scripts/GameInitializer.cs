using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class GameInitializer : MonoBehaviour {
    private async void Awake() {
        if (UnityServices.State == ServicesInitializationState.Uninitialized) {
            Debug.Log("Services Initializing");
            await UnityServices.InitializeAsync();
        }

        AnalyticsService.Instance.StartDataCollection();
    }
}
