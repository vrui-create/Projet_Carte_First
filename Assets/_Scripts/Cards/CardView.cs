using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] Image _cardtarget;

    [SerializeField] Image _cardBG;
    [SerializeField] Image _cardImage;
    [SerializeField] TextMeshProUGUI _cardName;
    [SerializeField] BoxCollider2D _cardBox;
    [SerializeField] GameObject _processBar;
    [SerializeField] Image _processBarImage;


    /*TODO 
     * anim de:
     * hover 
     * avaliable to place (le tour en poitillé)
     * burned
     * d�placement  ----------  ez
     * dropped by anoter card 
     * following parent
    */

    public void Update()
    {
        OnUpdate();
    }


    private void OnUpdate()
    {
    }
    
    
    
    public void EnableProcessBar(bool enable)
    {
        _processBar.SetActive(enable);
    }

    public void SetProcessBarValue(float value)
    {
        _processBarImage.fillAmount = value;
    }

    public void SetCardName(string name)
    {
        _cardName.text = name;
    }

    public void SetCardImage(Sprite sprite)
    {
        _cardImage.sprite = sprite;
    }
    
    public void SetCardColor(Color color)
    {
        _cardBG.color = color;
    }

    public void SetBoxCollider(bool enable)
    {
        _cardBox.enabled = enable;
    }

    public void SetRaycastTarget(bool enable)
    {
        _cardtarget.raycastTarget = enable;
    }

    public void SetPosition(Vector2 position)
    {
        rect.anchoredPosition =  position;
    }

   
    
}
