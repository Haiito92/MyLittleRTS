namespace RTSSelector.Scripts.OOP.Runtime.Core
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
