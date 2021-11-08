using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPanelManager : Singleton<TopPanelManager>
{
    [Header("BUTTON LIST")]
    public List<GameObject> buttons = new List<GameObject>();

    // [Header("BUTTON ANIMS")]
    private string buttonFadeIn = "Hover to Pressed";
    private string buttonFadeOut = "Pressed to Normal";

    private GameObject currentButton;
    private GameObject nextButton;

    public int currentButtonlIndex = 0;

    private Animator currentButtonAnimator;
    private Animator nextButtonAnimator;

    public void PanelAnim(int index)
    {
        currentButton = buttons[currentButtonlIndex];

        currentButtonlIndex = index;
        nextButton = buttons[index];

        currentButtonAnimator = currentButton.GetComponent<Animator>();
        nextButtonAnimator = nextButton.GetComponent<Animator>();

        currentButtonAnimator.Play(buttonFadeOut);
        nextButtonAnimator.Play(buttonFadeIn);

    }
}
