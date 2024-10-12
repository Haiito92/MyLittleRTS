using UnityEngine;

namespace RTSSelector.Scripts.Runtime.Interfaces
{
    public interface IRTSSelectable
    {

        Vector2 GetScreenPos();
        void Select();
        void Unselect();
    }
}
