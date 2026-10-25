using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace ThePrismatic2.ThePrismatic2Code.Relics;

public sealed class PhylacteryRemnant: ThePrismatic2Relic
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    public override string PackedIconPath => "ThePrismatic2/images/relics/PhylacteryRemnant.png";
    protected override string PackedIconOutlinePath => "ThePrismatic2/images/relics/PhylacteryRemnant_Outline.png";
    protected override string BigIconPath => "ThePrismatic2/images/relics/big/PhylacteryRemnant.png";

    public override bool SpawnsPets => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlySingleElementList<DynamicVar>(new SummonVar(2m));

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new _003C_003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, DynamicVars.Summon));

    public override async Task BeforeCombatStart()
    {
        await OstyCmd.Summon(new ThrowingPlayerChoiceContext(), Owner, DynamicVars.Summon.BaseValue, this);
    }
}