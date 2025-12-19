using UnityEngine;  

public interface ICardBehaviour
{

    public void Init(CardView  cardView, CardModel cardModel, CardController cardController);
    
    public void OnGrab(); // ex: highlight les cartes disponible
    public void OnDrop(); // ex: se rajouter 

    public void OnUpdate(); // ex: process le metal en plate 
}