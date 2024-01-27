using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite normal, hover;
    private Image image;
    
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnClick()
    {
        image.sprite = normal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = normal;
    }
}
