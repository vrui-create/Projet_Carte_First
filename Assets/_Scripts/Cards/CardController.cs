using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler , IDropHandler
{ 
    public CardData cardTemplate;
    [HideInInspector] public CardModel cardModel { get; private set; }
    
    [SerializeField] CardView cardView;
    [SerializeField] RectTransform rect;
    
    RectTransform parentRect;
    Canvas canvas;

    ICardBehaviour cardBehaviour;
    
    Vector2 targetPos;
    Vector2 offset;

    public Vector2 childoffset;

    bool isParented;
    [HideInInspector] public CardController parentCard;

    [Header("Damping")]
    [SerializeField, Range(1f, 30f)] float damping = 15f;


    //bool isDragging = false;
    //bool _isHovering;


    private void Start()
    {
        // ------ To remove ------  (when instantiating)

        if(parentRect == null)
        {
            parentRect = rect.parent as RectTransform;
        }

        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

        // -----------------------



        if (cardTemplate == null)
        {
            Debug.LogError("Aucun template asssign�");

            Destroy(gameObject);
            return;
        }

        cardModel = new CardModel(cardTemplate);

        if(cardTemplate.cardBehaviour != CardBehaviourFactory.CardBehaviourType.Idle)
        {
            cardBehaviour = CardBehaviourFactory.AddCardBehaviour(gameObject,cardTemplate.cardBehaviour);
            cardBehaviour.Init(cardView,cardModel,this);
        }

        cardView.SetCardName(cardModel.cardName);
        cardView.SetCardImage(cardModel.sprite);
        cardView.SetCardColor(cardModel.backgroundColor);
        
        targetPos = rect.anchoredPosition;
        isParented = false;
        parentCard = null;

    }

    private void Update()
    {
        if (!isParented)
        {
            for(int i = 0; i < cardModel.childCards.Count; i++)
            {
                cardModel.childCards[i].cardView.SetPosition(childoffset);
            }

            cardView.SetPosition(Vector2.Lerp(rect.anchoredPosition, targetPos, Time.deltaTime * damping));
        }

        
    }
    
    private void FixedUpdate()
    {
        if (cardBehaviour != null)
        {
            cardBehaviour.OnUpdate();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //isDragging = true;

        transform.SetParent(parentRect);

        if(isParented)
        {
            for (int i = 0; i < cardModel.childCards.Count; i++)
            {
                parentCard.cardModel.childCards.Remove(cardModel.childCards[i]);
                parentCard.RemoveChildToParent(cardModel.childCards[i]);

            }

            parentCard.cardModel.childCards.Remove(this);
            parentCard.RemoveChildToParent(this);

        }

        isParented = false;
        parentCard = null;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPointer
        );

        offset = rect.anchoredPosition - localPointer;
        targetPos = rect.anchoredPosition;

        cardView.SetRaycastTarget(false);
        for (int i = 0; i < cardModel.childCards.Count; i++)
        {
            cardModel.childCards[i].cardView.SetRaycastTarget(false);
        }

        
        cardView.SetBoxCollider(false);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        targetPos = localPoint + offset;

        Vector2 parentSize = parentRect.rect.size;
        Vector2 cardSize = rect.rect.size;

        float halfWidth = cardSize.x * 0.5f;
        float halfHeight = cardSize.y * 0.5f;

        float minX = -parentSize.x * 0.5f + halfWidth;
        float maxX = parentSize.x * 0.5f - halfWidth;
        float minY = -parentSize.y * 0.5f + halfHeight;
        float maxY = parentSize.y * 0.5f - halfHeight;

        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //isDragging = false;
        
        if (!isParented)
        {
            cardView.SetBoxCollider(true);
        }
        
        cardView.SetRaycastTarget(true);
        for (int i = 0; i < cardModel.childCards.Count; i++)
        {
            cardModel.childCards[i].cardView.SetRaycastTarget(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    { 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        
        if (dropped != null && dropped.TryGetComponent<CardController>(out CardController draggedCard))
        {
            
            if (dropped == this.gameObject)
            {
                return;
            }
        
            CardController rootParent = GetUpperParentCard();
        
            for (int i = 0; i < rootParent.cardModel.childCards.Count; i++)
            {
                if (dropped == rootParent.cardModel.childCards[i].gameObject)
                {
                    return;
                }
            }
            
            CardController uppercard = GetUpperCard();

            if (CardStack.CanStack(draggedCard.cardModel.cardType, uppercard.cardModel.cardType))
            {
                draggedCard.parentCard = uppercard;
                draggedCard.isParented = true;
                
                uppercard.cardModel.childCards.Add(draggedCard);
                uppercard.AddChildToParent(draggedCard);

                for(int i =0 ;i < draggedCard.cardModel.childCards.Count;i++){
                    
                    uppercard.cardModel.childCards.Add(draggedCard.cardModel.childCards[i]);
                    uppercard.AddChildToParent(draggedCard.cardModel.childCards[i]);
                    
                }

                
                
                draggedCard.gameObject.transform.SetParent(uppercard.gameObject.transform);
                draggedCard.cardView.SetBoxCollider(false);
                draggedCard.cardView.SetPosition(childoffset);

                //add parent et tt
            }

        }

    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        targetPos = rect.anchoredPosition;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        targetPos = rect.anchoredPosition;
    }

    public CardController GetUpperCard()
    {
        int childcount = cardModel.childCards.Count;
        if (childcount > 0)
        {
            return cardModel.childCards[childcount - 1];
        }
        return this;
    }

   public void AddChildToParent(CardController childcardController)
   {
        if (isParented)
        {
            if (parentCard == null)
            {
                isParented =  false;
                return;
                
            }else
            {
                parentCard.cardModel.childCards.Add(childcardController);
                parentCard.AddChildToParent(childcardController);
            }
        }
   }

    public void RemoveChildToParent(CardController childcardController)
    {
        if (isParented)
        {
            if (parentCard == null)
            {
                isParented =  false;
                return;
                
            }
            else
            {
                parentCard.cardModel.childCards.Remove(childcardController);
                parentCard.RemoveChildToParent(childcardController);
            }
            //fix d�parentage
        }
    }

    public CardController GetUpperParentCard()
    {
        if (!isParented)
        {
            return this;
        }
        else
        {
            if (parentCard == null)
            {
                isParented =false;
                return this;
            }else
            {
                return parentCard.GetUpperParentCard();
                
            }
        }
    }
}
