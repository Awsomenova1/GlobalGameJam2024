using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonAlternator : MonoBehaviour, ISelectHandler
{
    [SerializeField] private GameObject PlayButton;
    [SerializeField] private UnityEngine.UI.Button UpButton, DownButton;

    public void OnSelect(BaseEventData eventData)
    {
        Navigation nav = PlayButton.GetComponent<UnityEngine.UI.Button>().navigation;
        nav.selectOnDown = DownButton;
        nav.selectOnUp = UpButton;
        PlayButton.GetComponent<UnityEngine.UI.Button>().navigation = nav;
    }
}
