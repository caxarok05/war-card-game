using System;
using UnityEngine;

namespace Client.Scripts.Client
{
    [Serializable]
    public sealed class BootstrapStageData
    {
        [SerializeField] private BootstrapStage _stage;
        [SerializeField] [TextArea] private string _message;
        [SerializeField] [Range(0f, 1f)] private float _progress;

        public BootstrapStage Stage => _stage;
        public string Message => _message;
        public float Progress => _progress;
    }
}