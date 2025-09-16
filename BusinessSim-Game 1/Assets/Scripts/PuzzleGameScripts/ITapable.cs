using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITapable 
{
    public void OnTapped(ITapable tapped);
    public void OnReleased(ITapable released);
}
