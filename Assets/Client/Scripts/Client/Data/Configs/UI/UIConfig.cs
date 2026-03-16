using UnityEngine;

namespace Client.Scripts.Client
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Configs/UI Config")]
    public class UIConfig : ScriptableObject
    {
        [Header("Game Over PopUp")]
        public string WinText = "You Win. Congratulations!";
        public string DrawText = "Draw happened. Well played!";
        public string LoseText = "You Lose. Good luck next time!";
        
        [Header("Game State messages")]
        public string TapToDrawText = "Tap anywhere to draw";
        public string RequestingServerText = "Requesting server...";
        
        [Header("Failed messages")]
        public string InitalizeFailedText = "Failed to initialize match.";
        public string RequestFailedText = "Server request failed.";
    }
}