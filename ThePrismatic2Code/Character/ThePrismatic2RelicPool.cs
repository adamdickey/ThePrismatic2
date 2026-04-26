using BaseLib.Abstracts;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using Godot;

namespace ThePrismatic2.ThePrismatic2Code.Character;

public class ThePrismatic2RelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => ThePrismatic2.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}