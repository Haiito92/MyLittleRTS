using System.Collections;
using System.Collections.Generic;
using RTSSelector.Scripts.Runtime.Core;
using UnityEngine;

public class RTSSelectableProxy : IRTSSelectable
{
    private RTSSelectable _rtsSelectable;

    public RTSScreenRect GetScreenRect()
    {
        return _rtsSelectable.GetScreenRect();
    }

    public void PreSelect()
    {
        _rtsSelectable.PreSelect();
    }

    public void Select()
    {
        _rtsSelectable.Select();
    }

    public void PreUnselect()
    {
        _rtsSelectable.PreUnselect();
    }

    public void Unselect()
    {
        _rtsSelectable.Unselect();
    }
}
