using UnityEditor.Experimental.GraphView;
using UnityEngine;
using RTSSelector.Runtime.Interfaces;

namespace _Project.Runtime.Scripts.Characters
{
    public class Character : MonoBehaviour, IRTSSelectable
    {
        public void Select()
        {
            Debug.Log("Character Selected");
        }
    }
}
