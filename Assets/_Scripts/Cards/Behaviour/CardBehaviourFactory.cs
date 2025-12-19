using System;
using System.Collections.Generic;   
using UnityEngine;

public static class CardBehaviourFactory
{
    public enum CardBehaviourType
    {
        Idle,
        Smelter,
        Burner
    }

    static Dictionary<CardBehaviourType, Type> cardBehaviourScriptMap = new Dictionary<CardBehaviourType, Type>()
    {
        {CardBehaviourType.Idle, typeof(IdleCardBehaviour)},
        {CardBehaviourType.Smelter, typeof(SmelterCardBehaviour)},
        {CardBehaviourType.Burner, typeof(BurnerCardBehaviour)}
    };

    public static ICardBehaviour AddCardBehaviour(GameObject Card, CardBehaviourType cardBehaviourType)
    {
        if (cardBehaviourScriptMap.TryGetValue(cardBehaviourType, out Type scriptType))
        {
            var existing = Card.GetComponent(scriptType) as ICardBehaviour;
            if (existing != null)
            {
                Debug.Log($"Le behaviour est déjà présente.");
                return existing;
            }

            var behaviour = Card.AddComponent(scriptType) as ICardBehaviour;
            //behaviour.Setup(abilitySO);
            return behaviour;
        }
        else
        {
            Debug.LogError($"Pas de script lié pour le behaviour");
            return null;
        }
    }
}
