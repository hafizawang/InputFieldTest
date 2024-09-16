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
    protected Callback<GameOverlayActivated_t> overlayAction = null;

    public TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        bool res = SteamManager.Initialized;

        m_GamepadTextInputDismissed2 = Callback<GamepadTextInputDismissed_t>.Create(OnGamepadTextInputDismissed2);
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
