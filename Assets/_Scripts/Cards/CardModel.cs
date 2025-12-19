using System.Collections.Generic;
using UnityEngine;
using static CardBehaviourFactory;
using static CardData;

public class CardModel
{
    public string cardName;
    public Sprite sprite;
    public string description;
    public int sellValue;
    public Color backgroundColor;

    public CardType cardType;

    public List<CardController> childCards;


    public CardModel(CardData cardData)
    {
        cardName = cardData.cardName;
        sprite = cardData.sprite;
        description = cardData.description;
        sellValue = cardData.sellValue;
        cardType = cardData.cardType;
        backgroundColor =  cardData.backgroundColor;

        childCards = new List<CardController>();
    }
}
