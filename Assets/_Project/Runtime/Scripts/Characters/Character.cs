using System;
using RTSSelector.Scripts.Runtime;
using UnityEngine;

namespace _Project.Runtime.Scripts.Characters
{
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
