using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace ThePrismatic2.ThePrismatic2Code.Extensions;

public class Keywords
{
    [CustomEnum] 
    public static CardKeyword Offer;
    
    [CustomEnum] 
    public static CardKeyword Bleed;
    
    [CustomEnum] 
    public static CardKeyword Costly;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] 
    public static CardKeyword DualWield;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] 
    public static CardKeyword Cunning;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] 
    public static CardKeyword Starbound;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] 
    public static CardKeyword CunningThisTurn;
    
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] 
    public static CardKeyword StarboundThisTurn;
}