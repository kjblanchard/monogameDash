using System.Collections.Generic;

namespace SupergoonDashCrossPlatform.SupergoonEngine.Interfaces;

public interface ITags
{
    public void AddTag(int tag);
    public bool RemoveTag(int tag);
    public bool HasTag(int tag);
    public List<int> Tags { get; set; } 
}