using System;

namespace ImGuiNET.SampleProgram.XNA;

public class ImGuiWriteAttribute: Attribute
{
    public string DisplayName;
    public Type VariableType;
    public bool IsPrivate;
    public float Min;
    public float Max;
    
    public ImGuiWriteAttribute(Type typeOfVariable, bool isPrivate = false, string name = "")
    {
        VariableType = typeOfVariable;
        DisplayName = name;
        IsPrivate = isPrivate;

    }

}