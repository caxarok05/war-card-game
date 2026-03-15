using UnityEngine;

namespace Client.Scripts.Client
{
    public sealed class ConfigProvider : MonoBehaviour
    {
        [field: SerializeField] public BootstrapConfig BootstrapConfig { get; private set; }
        [field: SerializeField] public CardSpriteConfig CardSpriteConfig { get; private set; }
        [field: SerializeField] public AnimationConfig AnimationConfig { get; private set; }

    }
}