using UnityEngine;

public static class CardStack
{
    
    
    private static readonly bool[,] _allow =
    {
        //can stack with:   Coal,  Iron, Copper, IronIngot, CopperIngot, BronzeIngot, Burner,Smelter
        /*Coal          */{ true,  false, false,   false,    false,       false,       true,   false},
        /*Iron          */{ false, true,  true,   false,    false,       false,       false,   true},
        /*Copper        */{ false, true,  true,   false,    false,       false,       false,   true},
        /*IronIngot     */{ false, false, false,   true,    false,       false,       false,   false},
        /*CopperIngot   */{ false, false, false,   false,    true,       false,       false,   false},
        /*BronzeIngot   */{ false, false, false,   false,    false,       true,       false,   false},
        /*Burner       */{ false, false, false,   false,    false,       false,       false,   false},
        /*Smelter        */{ false, false, false,   false,    false,       false,       false,   false}
    };

    public static bool CanStack(CardData.CardType selectedCard, CardData.CardType whereDroppedCard)
    {
        return _allow[(int)selectedCard, (int)whereDroppedCard];
    }
}