using System.Reflection;

namespace ImGuiNET.SampleProgram.XNA.TiledDrawObjects;

public class BoolImGuiDisplay
{
    public string Name;
    public FieldInfo FieldPtr;
    public object Owner;
    public bool Value;
    public bool IsWritable;

    public BoolImGuiDisplay(string name, FieldInfo fieldPtr, object owner, bool writable)
    {
        Name = name;
        FieldPtr = fieldPtr;
        Owner = owner;
        Value = GetValue;
        IsWritable = writable;
    }

    public void Update()
    {
        if (IsWritable)
            SetValue(Value);
    }

    public void Draw()
    {
        ImGui.Text(Name);
        ImGui.Text(GetValue.ToString());

        ImGui.Checkbox($"{Name}: {Value.ToString()}", ref Value);
        ImGui.SameLine();
        if (IsWritable)
        {
            if (ImGui.Button($"{Name} Update"))
                Update();
        }
    }


    public bool GetValue => (bool)FieldPtr.GetValue(Owner);
    public void SetValue(object value) => FieldPtr.SetValue(Owner, value);
}