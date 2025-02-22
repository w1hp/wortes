//using UnityEngine;
//using TMPro;
//using UnityEngine.InputSystem;
//using System;

//public class Keybind : MonoBehaviour
//{
//    [Header("Objects")]
//    [SerializeField] private TextMeshProUGUI buttonLbl;
//    private InputAction inputAction; // Przechowuje akcję zamiast stringa
//    [SerializeField] private string actionName; // 👈 Dodaj to, żeby można było ustawić w Inspectorze!


//    private PlayerInputActions inputActions;
//    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

//    private void Awake()
//    {
//        inputActions = new PlayerInputActions();
//        inputActions.Enable();
//    }

//    // Ustawienie akcji zamiast stringa
//    public void SetAction(InputAction action)
//    {
//        inputAction = action;
//        LoadKeybind();
//    }

//    private void LoadKeybind()
//    {
//        if (inputAction == null)
//        {
//            Debug.LogError($"❌ inputAction is NULL for {gameObject.name}! Call SetAction() before using this script.");
//            return;
//        }

//        if (buttonLbl == null)
//        {
//            Debug.LogError("❌ buttonLbl is NULL! Assign it in the Inspector.");
//            return;
//        }

//        if (inputAction.bindings.Count == 0)
//        {
//            Debug.LogError($"❌ Action '{inputAction.name}' has no bindings! Assign keys in PlayerInputActions.");
//            buttonLbl.text = "None";
//            return;
//        }

//        string currentBinding;

//        if (PlayerPrefs.HasKey(inputAction.name))
//        {
//            currentBinding = PlayerPrefs.GetString(inputAction.name);
//            Debug.Log($"✅ Loaded saved key for {inputAction.name}: {currentBinding}");
//        }
//        else
//        {
//            int bindingIndex = inputAction.bindings[0].isComposite ? 1 : 0;
//            currentBinding = inputAction.bindings[bindingIndex].effectivePath ?? "UNKNOWN";
//            currentBinding = currentBinding.Substring(currentBinding.LastIndexOf('/') + 1).ToUpper();

//            Debug.Log($"📌 No saved key for {inputAction.name}. Using default: {currentBinding}");

//            // 🔥 Zapisujemy domyślny klawisz w PlayerPrefs
//            PlayerPrefs.SetString(inputAction.name, currentBinding);
//            PlayerPrefs.Save();
//        }

//        buttonLbl.text = currentBinding;
//    }







//    private void Start()
//    {
//        if (string.IsNullOrEmpty(actionName))
//        {
//            Debug.LogError($"❌ actionName is NULL for {gameObject.name}. Set it in the Inspector!");
//            return;
//        }

//        Debug.Log($"🎮 Setting action for {gameObject.name}: {actionName}");

//        if (inputActions.GameplayMap.Move.controls.Count == 0)
//        {
//            Debug.LogError("❌ GameplayMap.Move nie ma przypisanych żadnych kontrolerów!");
//        }
//        else
//        {
//            Debug.Log($"✅ Pierwszy kontroler dla Move: {inputActions.GameplayMap.Move.controls[0].path}");
//        }


//        int bindingIndex = -1;

//        // 📌 Pobieramy odpowiednią akcję
//        switch (actionName)
//        {
//            case "Forward":
//                inputAction = inputActions.GameplayMap.Move;
//                bindingIndex = inputAction.bindings.IndexOf(x => x.isPartOfComposite && x.name == "Up");
//                break;
//            case "Back":
//                inputAction = inputActions.GameplayMap.Move;
//                bindingIndex = inputAction.bindings.IndexOf(x => x.isPartOfComposite && x.name == "Down");
//                break;
//            case "Left":
//                inputAction = inputActions.GameplayMap.Move;
//                bindingIndex = inputAction.bindings.IndexOf(x => x.isPartOfComposite && x.name == "Left");
//                break;
//            case "Right":
//                inputAction = inputActions.GameplayMap.Move;
//                bindingIndex = inputAction.bindings.IndexOf(x => x.isPartOfComposite && x.name == "Right");
//                break;
//            case "Fire":
//                inputAction = inputActions.GameplayMap.Choose1;
//                break;
//            case "Water":
//                inputAction = inputActions.GameplayMap.Choose2;
//                break;
//            case "Sand":
//                inputAction = inputActions.GameplayMap.Choose3;
//                break;
//            case "Wood":
//                inputAction = inputActions.GameplayMap.Choose4;
//                break;
//            //case "Metal":
//            //    inputAction = inputActions.GameplayMap.Choose5;
//            //    break;
//            case "ShootOrBuild":
//                inputAction = inputActions.GameplayMap.ShootOrBuild;
//                break;
//            case "Switch":
//                inputAction = inputActions.GameplayMap.SwitchMode;
//                break;
//            default:
//                Debug.LogError($"❌ No action found for '{actionName}'");
//                return;
//        }

//        LoadKeybind();
//    }




//    public void ChangeKey()
//    {
//        if (inputAction == null)
//        {
//            Debug.LogError($"❌ inputAction is NULL for {actionName}! Call SetAction() before using this script.");
//            return;
//        }

//        if (inputAction.bindings.Count == 0)
//        {
//            Debug.LogError($"❌ Action '{inputAction.name}' has no bindings!");
//            return;
//        }

//        buttonLbl.text = "Awaiting Input...";

//        int bindingIndex = inputAction.bindings[0].isComposite ? 1 : 0;

//        inputAction.Disable();

//        rebindingOperation = inputAction.PerformInteractiveRebinding(bindingIndex)
//            .WithControlsExcluding("Mouse")
//            .OnComplete(operation =>
//            {
//                string newKey = operation.selectedControl.path;
//                newKey = newKey.Substring(newKey.LastIndexOf('/') + 1).ToUpper();

//                Debug.Log($"[ChangeKey] After Replace: {newKey}");

//                // 🔥 Zapisujemy klawisz w PlayerPrefs
//                PlayerPrefs.SetString(inputAction.name, newKey);
//                PlayerPrefs.Save();

//                // 🔥 Natychmiast sprawdzamy, czy PlayerPrefs poprawnie zapisuje
//                string checkKey = PlayerPrefs.GetString(inputAction.name);
//                Debug.Log($"🔄 PlayerPrefs Check: {inputAction.name} = {checkKey}");

//                buttonLbl.text = newKey;
//                operation.Dispose();
//                inputAction.Enable();
//            })
//            .Start();
//    }
//}
