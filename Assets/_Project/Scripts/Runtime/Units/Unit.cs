using RTSSelector.Scripts.OOP.Runtime.Core;
using UnityEngine;

public class Unit : MonoBehaviour
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
