using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Client
{
    [CreateAssetMenu(fileName = "BootstrapConfig", menuName = "Configs/Bootstrap Config")]
    public sealed class BootstrapConfig : ScriptableObject
    {
        [SerializeField] private string _gameSceneName = "GameScene";
        [SerializeField] private string _bootstrapSceneName = "BootstrapScene";
        [SerializeField] private List<BootstrapStageData> _stages = new();

        public string GameSceneName => _gameSceneName;
        public string BootstrapSceneName => _bootstrapSceneName;
        public IReadOnlyList<BootstrapStageData> Stages => _stages;

        public BootstrapStageData GetStageData(BootstrapStage stage)
        {
            for (int i = 0; i < _stages.Count; i++)
            {
                BootstrapStageData stageData = _stages[i];
                if (stageData.Stage == stage)
                {
                    return stageData;
                }
            }

            throw new InvalidOperationException(
                $"Bootstrap stage config was not found for stage: {stage}");
        }
    }
}