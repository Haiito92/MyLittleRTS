using UnityEngine;

namespace _Project.Scripts.Runtime.Characters
{
    using RTSSelector.Scripts.Runtime.Core;
    
    public class Character : MonoBehaviour
    {
        [SerializeField] private RTSSelectable _rtsSelectable;

        private void OnSelected()
        {
            Debug.Log("Character Selected");
        }

        private void OnEnable()
        {
            _rtsSelectable.Selected += OnSelected;
        }

        private void OnDisable()
        {
            _rtsSelectable.Selected -= OnSelected;
        }
    }
}
