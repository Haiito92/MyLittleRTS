using UnityEngine;

namespace RTSSelector.Scripts.Runtime
{
    public interface IRTSSelectable
    {
        Vector2 GetScreenPos();
        void Select();
        void Unselect();
    }
}
