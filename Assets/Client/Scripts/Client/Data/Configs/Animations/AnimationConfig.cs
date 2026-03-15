using UnityEngine;

namespace Client.Scripts.Client
{
    [CreateAssetMenu(fileName = "AnimationConfig", menuName = "Configs/Animation Config")]
    public class AnimationConfig : ScriptableObject
    {
        [Header("Reveal animation")]
        public float RevealMoveDuration = 0.25f;
        public float DelayBetweenRevealCards = 0.06f;
        public float RevealPauseSeconds = 0.6f;

        [Header("Collect animation")]
        public float CollectMoveDuration = 0.45f;
        public float CollectArcHeight = 0.9f;
        public float CollectSpinAngle = 360f;
        public float DelayBetweenCollectCards = 0.03f;
        public float CollectPauseSeconds = 0.15f;

        [Header("Deck feedback")]
        public float DeckPunchScale = 0.1f;
        public float DeckPunchDuration = 0.25f;

        [Header("Sorting")]
        public int CollectSortingOffset = 1000;
        
        [Header("CameraPunch")]
        public float WarPunchDuration = 0.2f;
        public Vector3 WarPunchStrength = new Vector3(0.18f, 0.18f, 0f);
        public int WarPunchVibrato = 10;
        public float WarPunchElasticity = 0.8f;
    }
}