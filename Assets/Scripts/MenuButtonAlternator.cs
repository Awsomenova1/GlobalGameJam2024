using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonAlternator : MonoBehaviour, ISelectHandler
{
    [SerializeField] private GameObject PlayButton;

    public void OnSelect(BaseEventData eventData)
    {
        Navigation nav = PlayButton.GetComponent<UnityEngine.UI.Button>().navigation;
        nav.selectOnDown = GetComponent<UnityEngine.UI.Button>();
        PlayButton.GetComponent<UnityEngine.UI.Button>().navigation = nav;
    }
}
