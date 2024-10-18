using UnityEngine;

namespace RTSSelector.Scripts.Runtime
{
    public interface IRTSSelectable
    {
        RTSScreenRect GetScreenRect();
        void Select();
        void Unselect();
    }
}
