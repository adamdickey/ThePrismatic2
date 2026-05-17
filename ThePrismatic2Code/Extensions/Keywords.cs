using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Extensions;

public class Keywords
{
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] 
    public static CardKeyword DualWield;
    
    [CustomEnum] 
    public static CardKeyword Costly;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] 
    public static CardKeyword Cunning;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] 
    public static CardKeyword Starbound;
}