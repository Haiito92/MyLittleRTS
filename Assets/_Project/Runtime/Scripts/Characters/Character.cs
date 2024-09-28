using _Project.Runtime.Interfaces;
using UnityEngine;

namespace _Project.Runtime.Scripts.Characters
{
    public class Character : MonoBehaviour, ISelectable
    {

        public void Select()
        {
            Debug.Log("Character Selected");
        }
    }
}
