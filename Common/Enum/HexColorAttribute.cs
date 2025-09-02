namespace LegislacionAPP.Common.Enums;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class HexColorAttribute : Attribute
{
    public string Hex { get; }
    public HexColorAttribute(string hex) => Hex = hex;
}
