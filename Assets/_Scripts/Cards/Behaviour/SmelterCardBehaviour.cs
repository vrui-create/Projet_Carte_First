using System.Collections.Generic;
using UnityEngine;

public class SmelterCardBehaviour : MonoBehaviour , ICardBehaviour
{
    CardView _cardView;
    CardModel _cardModel;
    CardController _cardController;
    

    private bool isProcessing;
    private float startTime;

    private float ActualProcessTime;
    private CardData ActualCardToInstantiate;

    possibleoutcomes outcome;

    Vector3 SpawnOffset = new (80,0,0);
    
    private List<float> processTimes = new List<float>();
    
    //----- ADD ASSET MANAGER -----
    private List<CardData> outComesTemplates = new List<CardData>();

    private GameObject voidCard;
    // ----------------------------
        
    private possibleoutcomes[,] outcomes =
    {
        //Smelt with:               Iron,                  Copper
        /*Nothing   */ {possibleoutcomes.IronIngot,   possibleoutcomes.CopperIngot},
        /*Iron      */ {possibleoutcomes.Null,        possibleoutcomes.BronzeIngot },
        /*Copper    */ {possibleoutcomes.BronzeIngot, possibleoutcomes.Null }
        
    };
    
    
    private enum possibleoutcomes
    {
        IronIngot,
        CopperIngot,
        BronzeIngot,
        Null
    }
    
    public void Init(CardView  cardView, CardModel cardModel,CardController cardController)
    {
        _cardView = cardView;
        _cardModel = cardModel;
        _cardController = cardController;
        
        processTimes.Add(2f);
        processTimes.Add(3f);
        processTimes.Add(5f);

        outComesTemplates.Add(Resources.Load("Card/Iron Ingot") as CardData);
        outComesTemplates.Add(Resources.Load("Card/Copper Ingot") as CardData);
        outComesTemplates.Add(Resources.Load("Card/Bronze Ingot") as CardData);
        
        voidCard  = Resources.Load("_Prefabs/Card") as GameObject;
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
        if (_cardModel.childCards.Count > 0)
        {
            if (!isProcessing)
            {
                startTime = Time.time;
                _cardView.EnableProcessBar(true);

                
                if (_cardModel.childCards.Count > 1 && _cardModel.childCards[0].cardModel.cardType != _cardModel.childCards[1].cardModel.cardType)
                {
                    outcome = outcomes[(int)_cardModel.childCards[0].cardModel.cardType,(int)_cardModel.childCards[1].cardModel.cardType - 1];
                }
                else
                {
                    outcome = outcomes[0, (int)_cardModel.childCards[0].cardModel.cardType - 1];
                }
                
                ActualProcessTime = processTimes[(int)outcome];
                ActualCardToInstantiate = outComesTemplates[(int)outcome];
                isProcessing =  true;

            }
            else
            {
                Process();
            }

        }
        else
        {
            isProcessing = false;
            startTime = Time.time;
            _cardView.EnableProcessBar(false);
        }
    }

    public void Process()
    {
        float now = Time.time;
        
        _cardView.SetProcessBarValue((now - startTime)/ActualProcessTime);
        
        if (now >= startTime + ActualProcessTime)
        {
            GameObject newcard = Instantiate(voidCard, transform.parent);
            newcard.transform.localPosition = this.transform.localPosition + SpawnOffset;
            newcard.GetComponent<CardController>().cardTemplate = ActualCardToInstantiate;
            
            int numbertodestroy = 1;
            if ((int)outcome >= 2) //hard coded alloy 
            {
                numbertodestroy = 2;
            }

            if (_cardModel.childCards.Count > numbertodestroy)
            {
                _cardModel.childCards[numbertodestroy].gameObject.transform.parent = gameObject.transform;
                _cardModel.childCards[numbertodestroy].GetComponent<CardController>().parentCard = _cardController;
            }

            GameObject ToDestroy = _cardModel.childCards[0].gameObject;

            for (int i = 0; i < numbertodestroy;i++)
            {
                _cardModel.childCards.RemoveAt(0);
                
            }
            Destroy(ToDestroy,0.1f);
            
            isProcessing = false;
            startTime = Time.time;
        }
    }
}
