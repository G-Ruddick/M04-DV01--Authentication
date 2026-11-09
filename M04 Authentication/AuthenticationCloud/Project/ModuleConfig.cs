using Microsoft.Extensions.DependencyInjection;
using Unity.Services.CloudCode.Apis;
using Unity.Services.CloudCode.Core;

namespace M04_Authentication;

public class ModuleConfig : ICloudCodeSetup {
    public void Setup(ICloudCodeConfig config) {
        config.Dependencies.AddSingleton(GameApiClient.Create());
    }
}
