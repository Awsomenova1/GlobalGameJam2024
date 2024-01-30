using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IDeselectHandler, ISelectHandler
{
    [SerializeField] private Sprite normal;
    [SerializeField] private bool MainMenu;
    [SerializeField] private AK.Wwise.Event MenuNav;
    public void OnDeselect(BaseEventData eventData)
    {
        GetComponent<Image>().sprite = normal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PopupPanel.mouseNeverMoved > 0)
        {
            PopupPanel.mouseNeverMoved--;
            return;
        }

        if (MainMenu ^ PopupPanel.visible)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.hovered.Contains(gameObject) && EventSystem.current.currentSelectedGameObject != gameObject && !PopupPanel.visible)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (MainMenuManager.firstopen && !MainMenuManager.quitting && !MainMenuManager.playing)
        {
            MenuNav.Post(gameObject);
        }
    }
}
