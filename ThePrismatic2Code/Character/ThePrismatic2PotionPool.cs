using BaseLib.Abstracts;
using ThePrismatic2.ThePrismatic2Code.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models;
using ThePrismatic2.ThePrismatic2Code.Potions;

namespace ThePrismatic2.ThePrismatic2Code.Character;

public class ThePrismatic2PotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => ThePrismatic2.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    
    protected override IEnumerable<PotionModel> GenerateAllPotions()
    {
        return
        [
            ModelDb.Potion<ExposedPotion>()
        ];
    }
}