using TMPro;
using UnityEngine;

namespace Client.Scripts.Client
{
    public class GameStatusTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private GameObject _root;

        public void Show(string text)
        {
            _root.SetActive(true);
            _statusText.text = text;
        }

        public void Hide()
        {
            _root.SetActive(false);
        }
    }
}