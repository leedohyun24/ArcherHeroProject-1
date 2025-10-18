using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.UI;

public class DisplayableButton : MonoBehaviour , IDisplayable
{
    private Sprite btnImage;
    private Action bindedAction;
    private Button button;
    private string btnName;
    private int canInteract = 1;

    void Awake()
    {
        button = GetComponent<Button>();

        if(button == null)
        {
            UnityEngine.Debug.LogError("Fail To Make DisplayableDataButton Button");
        }
    }

    public void SetDisplayableImage(Sprite BtnImage)
    {
        if(BtnImage == null)
        {
            UnityEngine.Debug.LogError("The Button Image is Null!!");
            return;
        }

        btnImage = BtnImage;

        Image img = button.GetComponent<Image>();

        if (img != null)
            img.sprite = btnImage;

    }
    public Sprite GetDisplayableImage()
    {
        if(btnImage != null)
        {
            return btnImage;
        }

        else
        {
            UnityEngine.Debug.LogWarning("Fail To Get Button Image");
            return null;
        }
        
    }

    public void BindingFunction(Action action)
    {
        if(action == null)
        {
            UnityEngine.Debug.LogError("Bin Button Action Failed!");
            return;
        }

        bindedAction = action;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => bindedAction.Invoke());
    }

    public Action GetBindedFunction()
    {
        if (bindedAction == null)
        {
            UnityEngine.Debug.LogError("DisplayableData Function was Null");
            return null;
        }

        return bindedAction;
    }

    public string GetDisplayableName()
    {
        return btnName;
    }

    public int GetExtraInfo()
    {
        return canInteract;
    }

    public void SetExtraInfo(int extraInfo) { canInteract = extraInfo; }
}
