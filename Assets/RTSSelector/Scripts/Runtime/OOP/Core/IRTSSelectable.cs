using RTSSelector.Scripts.Runtime.Core;

namespace RTSSelector.Scripts.Runtime.OOP.Core
{
    public interface IRTSSelectable
    {
        RTSScreenRect GetScreenRect();

        void PreSelect();
        void Select();

        void PreUnselect();
        void Unselect();
    }
}
