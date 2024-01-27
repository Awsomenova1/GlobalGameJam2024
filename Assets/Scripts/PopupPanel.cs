using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PopupPanel : MonoBehaviour
{
    public static bool open = false;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        anim.SetTrigger("Open");
        open = true;
    }

    public void Close()
    {
        anim.SetTrigger("Close");
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        open = false;
    }
}
