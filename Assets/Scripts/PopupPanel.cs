using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PopupPanel : MonoBehaviour
{
    [SerializeField] private GameObject PrimaryButton;
    [SerializeField] private bool SelectPrevious;
    [SerializeField] private Volume PostProcessing;
    public float animProgress;
    private GameObject PreviousButton;
    public static bool open = false;
    private Animator anim;
    private GameObject currentSelection;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (open)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                currentSelection = EventSystem.current.currentSelectedGameObject;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(currentSelection);
            }
        }
        PostProcessing.weight = animProgress;
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        anim.SetTrigger("Open");
        open = true;
        if (SelectPrevious)
            PreviousButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(PrimaryButton);
    }

    public void Close()
    {
        anim.SetTrigger("Close");
        if (SelectPrevious)
            EventSystem.current.SetSelectedGameObject(PreviousButton);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        open = false;
    }
}
