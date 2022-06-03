
using System;
using Microsoft.Xna.Framework;

namespace ImGuiNET.SampleProgram.XNA;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)] 
public class ImGuiReadPropertyAttribute : Attribute
{
    public static ImGuiGameComponent _imGuiGameComponent;
    public string Name { get; set; }

    public ImGuiReadPropertyAttribute(string name)
    {
        Name = name;
    }
}