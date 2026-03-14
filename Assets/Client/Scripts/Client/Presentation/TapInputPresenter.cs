using System;
using UnityEngine;
using Zenject;

namespace Client.Scripts.Client
{
    public sealed class TapInputPresenter : MonoBehaviour
    {
        private AbstractInput _input;
        private bool _isInputEnabled = true;

        public event Action InputPerformed;

        [Inject]
        public void Construct(AbstractInput input)
        {
            _input = input;
        }

        private void Start()
        {
            _input.Init();
        }

        private void Update()
        {
            if (!_isInputEnabled)
            {
                return;
            }

            if (!_input.GetInputDown())
            {
                return;
            }

            InputPerformed?.Invoke();
        }

        public void SetInputEnabled(bool isInputEnabled)
        {
            _isInputEnabled = isInputEnabled;
        }
    }
}