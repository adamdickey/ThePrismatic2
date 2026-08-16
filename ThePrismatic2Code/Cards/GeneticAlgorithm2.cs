using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace ThePrismatic2.ThePrismatic2Code.Cards;

public class GeneticAlgorithm2() : ThePrismatic2Card(1, 
    CardType.Skill, CardRarity.Rare, 
    TargetType.Self)
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<DefectCardPool>();
    public override string CustomPortraitPath => "res://.godot/imported/genetic_algorithm.png-802ab61d00de6b12c1847d2013fd18fe.ctex";
    public override string PortraitPath => "res://.godot/imported/genetic_algorithm.png-802ab61d00de6b12c1847d2013fd18fe.ctex";

    private int _currentBlock = 1;

    private int _increasedBlock;

    public override bool GainsBlock => true;

    [SavedProperty]
    public int CurrentBlock
    {
        get
        {
            return _currentBlock;
        }
        set
        {
            AssertMutable();
            _currentBlock = value;
            DynamicVars.Block.BaseValue = _currentBlock;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new _003C_003Ez__ReadOnlyArray<DynamicVar>([
        new BlockVar(CurrentBlock, ValueProp.Move),
        new IntVar("Increase", 3m)
    ]);

    public override IEnumerable<CardKeyword> CanonicalKeywords => new _003C_003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);

    [SavedProperty]
    public int IncreasedBlock
    {
        get
        {
            return _increasedBlock;
        }
        set
        {
            AssertMutable();
            _increasedBlock = value;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        int intValue = DynamicVars["Increase"].IntValue;
        BuffFromPlay(intValue);
        (DeckVersion as GeneticAlgorithm2)?.BuffFromPlay(intValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Increase"].UpgradeValueBy(1m);
    }

    protected override void AfterDowngraded()
    {
        UpdateBlock();
    }

    private void BuffFromPlay(int extraBlock)
    {
        IncreasedBlock += extraBlock;
        UpdateBlock();
    }

    private void UpdateBlock()
    {
        CurrentBlock = 1 + IncreasedBlock;
    }
}