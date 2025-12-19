using System.Collections.Generic;
using UnityEngine;
using static CardBehaviourFactory;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite sprite;
    public Color backgroundColor;
    public string description;
    public int sellValue;

    public CardBehaviourType cardBehaviour;
    public CardType cardType;

    public enum CardType
    {
        Coal,
        Iron,
        Copper,
        IronIngot,
        CopperIngot,
        BronzeIngot,
        Burner,
        Smelter
    }
}
