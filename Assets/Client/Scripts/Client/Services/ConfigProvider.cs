using UnityEngine;

namespace Client.Scripts.Client.Services
{
    public sealed class ConfigProvider : MonoBehaviour
    {
        [field: SerializeField] public BootstrapConfig BootstrapConfig { get; private set; }

    }
}