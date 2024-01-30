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
    [SerializeField] private bool SelectPrevious, Closable = true;
    [SerializeField] private Volume PostProcessing;
    [SerializeField] private AK.Wwise.Event MenuBack;
    public float animProgress;
    private GameObject PreviousButton;
    public static bool open = false, visible = false;
    public static int mouseNeverMoved = 0;
    private Animator anim;
    private GameObject currentSelection;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        open = true;
        
        if (EventSystem.current.currentSelectedGameObject == PreviousButton && visible)
        {
            EventSystem.current.SetSelectedGameObject(PrimaryButton);
        }

        if (EventSystem.current.currentSelectedGameObject != null)
        {
            currentSelection = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(currentSelection);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Closable)
        {
            Close();
        }
        
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            mouseNeverMoved = 0;
        }

        PostProcessing.weight = animProgress;
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        anim.SetTrigger("Open");
        open = true;
        mouseNeverMoved = 2;
        visible = true;
        if (SelectPrevious)
            PreviousButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(PrimaryButton);
    }

    public void Close()
    {
        // MenuBack.Post(gameObject); 
        anim.SetTrigger("Close");
        visible = false;
        if (SelectPrevious)
            EventSystem.current.SetSelectedGameObject(PreviousButton);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        open = false;
    }
}
