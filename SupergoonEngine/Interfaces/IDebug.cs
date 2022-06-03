using System;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

public interface IDebug
{
    public bool Debug { get; set; }
    public void SendAttributesToDebugger(object parent);

}