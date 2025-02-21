//using UnityEngine;
//using TMPro;
//using System;
//using UnityEngine.InputSystem;

//public class Keybind : MonoBehaviour
//{
//    [Header("Objects")]
//    [SerializeField] private TextMeshProUGUI buttonLbl;

//    private PlayerInputActions inputActions;
//    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
//    private string actionName = "Jump"; // Change this to the action you want to bind

//    private void Awake()
//    {
//        inputActions = new PlayerInputActions();

//        if (inputActions == null)
//        {
//            Debug.LogError("❌ inputActions FAILED to initialize!");
//            return;
//        }

//        inputActions.Enable();
//        Debug.Log("✅ inputActions initialized.");
//    }



//    private void Start()
//    {
//        // Debug inputActions
//        if (inputActions == null)
//        {
//            Debug.LogError("❌ inputActions is NULL!");
//            return;
//        }

//        // Debug asset
//        if (inputActions.asset == null)
//        {
//            Debug.LogError("❌ inputActions.asset is NULL!");
//            return;
//        }

//        // Get the saved key from PlayerPrefs
//        string savedKey = PlayerPrefs.GetString("CustomKey", "space");

//        if (buttonLbl == null)
//        {
//            Debug.LogError("❌ buttonLbl (TextMeshPro) is NULL! Check the UI assignment.");
//            return;
//        }

//        buttonLbl.text = savedKey;

//        // Find the action
//        var action = inputActions.asset.FindAction(actionName);
//        if (action == null)
//        {
//            Debug.LogError($"❌ Action '{actionName}' not found in InputActions!");
//            return;
//        }

//        action.ApplyBindingOverride("<Keyboard>/" + savedKey.ToLower());

//        Debug.Log("✅ Keybinds successfully loaded.");
//    }



//public void ChangeKey()
//{
//    Debug.Log("🔄 Rebinding started for: " + actionName);

//    var action = inputActions.asset.FindAction(actionName);
//    if (action != null)
//    {
//        Debug.Log("✅ Action found: " + action.name);
//        action.Disable();

//        rebindingOperation = action.PerformInteractiveRebinding()
//            .WithControlsExcluding("Mouse") // Pomija mysz
//            .OnComplete(operation =>
//            {
//                string newKey = operation.selectedControl.path.Replace("<Keyboard>/", "");
//                Debug.Log("✅ New key assigned: " + newKey);
//                buttonLbl.text = newKey;
//                PlayerPrefs.SetString("CustomKey", newKey);
//                PlayerPrefs.Save();

//                action.Enable();
//                operation.Dispose();
//            })
//            .Start();
//    }
//    else
//    {
//        Debug.LogError("❌ Action not found: " + actionName);
//    }
//}


//}
