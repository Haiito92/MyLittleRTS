namespace RTSSelector.Scripts.Runtime.Core
{
    public interface IRTSSelectable
    {
        RTSScreenRect GetScreenRect();
        void Select();
        void Unselect();
    }
}
