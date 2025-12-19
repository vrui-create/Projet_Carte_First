using UnityEngine;

public class BurnerCardBehaviour : MonoBehaviour , ICardBehaviour
{
    CardView _cardView;
    CardModel _cardModel;
    CardController _cardController;
    
    public void Init(CardView  cardView, CardModel cardModel, CardController cardController)
    {
        _cardView =  cardView;
        _cardModel = cardModel;
        _cardController = cardController;
    }
    public void OnDrop()
    {
        throw new System.NotImplementedException();
    }

    public void OnGrab()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        //je fais rien
    }
}
