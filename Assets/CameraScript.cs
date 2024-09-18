using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class CameraScript : MonoBehaviour
{
    protected Callback<GamepadTextInputDismissed_t> m_GamepadTextInputDismissed2 = null;
    protected Callback<FloatingGamepadTextInputDismissed_t> m_GamepadTextInputDismissed = null;
    protected Callback<GameOverlayActivated_t> overlayAction = null;

    public TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (inputField.isFocused)
        {
            if (!inputFieldFocus)
            {
                //                m_GamepadTextInputDismissed = Callback<FloatingGamepadTextInputDismissed_t>.Create(OnGamepadTextInputDismissed);
                //                m_GamepadTextInputDismissed2 = Callback<GamepadTextInputDismissed_t>.Create(OnGamepadTextInputDismissed2);
                //                Steamworks.SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, "", 25, "");


                Vector3[] p = new Vector3[4];

                inputField.GetComponent<RectTransform>().GetWorldCorners(p);
                Vector3 ls = inputField.GetComponent<RectTransform>().localScale;

                RectTransform rt = inputField.GetComponent<RectTransform>();

                Steamworks.SteamUtils.ShowFloatingGamepadTextInput(EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine, (int)p[1].x, (int)p[1].y, (int)(p[2].x - p[1].x), (int)(p[1].y - p[0].y));

                inputFieldFocus = true;
            }
        }
        else
        {
            if (inputFieldFocus)
            {
            }
        }


    }

    bool inputFieldFocus = false; // we need this so the keyboard does keep being called to open in update loop

    void OnGamepadTextInputDismissed(FloatingGamepadTextInputDismissed_t pCallback)
    {
        Debug.Log("Got text input dismissed!");
        inputFieldFocus = false;
        inputField.DeactivateInputField();

    }

    private void OnEnable()
    {
        bool res = SteamManager.Initialized;

        m_GamepadTextInputDismissed2 = Callback<GamepadTextInputDismissed_t>.Create(OnGamepadTextInputDismissed2);
        m_GamepadTextInputDismissed = Callback<FloatingGamepadTextInputDismissed_t>.Create(OnGamepadTextInputDismissed);
        overlayAction = Callback<GameOverlayActivated_t>.Create(OverlayAction);
    }

    void OverlayAction(GameOverlayActivated_t callback)
    {
        if (callback.m_bActive == 0)
        {
            EventSystem.current.GetComponent<InputSystemUIInputModule>().enabled = true;
        }
        else
        {
            EventSystem.current.GetComponent<InputSystemUIInputModule>().enabled = false;
        }
    }

    void OnGamepadTextInputDismissed2(GamepadTextInputDismissed_t pCallback)
    {
        Debug.Log("Got text input dismissed!");
        if (!pCallback.m_bSubmitted) return;

        uint length = Steamworks.SteamUtils.GetEnteredGamepadTextLength();
        string enteredText = "";
        bool success = Steamworks.SteamUtils.GetEnteredGamepadTextInput(out enteredText, length);

        if (success)
        {
            inputField.text = enteredText;
            return;
        }
    }

    public void ShowGamepadTextInput()
    {
        Steamworks.SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, "", 25, "");
    }

}
