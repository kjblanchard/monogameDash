using System.Collections.Generic;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

public interface ITags
{
    public void AddTag(int tag);
    public void RemoveTag(int tag);
    public List<int> Tags { get; set; } 
}