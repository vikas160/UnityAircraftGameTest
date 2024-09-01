using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Inputs : MonoBehaviour
{
    // References
    [SerializeField] private InputActionAsset _InputActionsAsset = default;

    // Input variables 
    internal float HrInput { get; private set; }
    internal float VrInput { get; private set; }
    internal float Roll_Input { get; private set; }

   

    internal bool ShootBtn { get; private set; }
    internal bool ThrottleBtn { get; private set; }

    internal bool BreakeBtn { get; private set; }   
    internal bool CamSwitchBtn { get; private set; }    

    // Actions
    private InputAction ShootAction = default;
    private InputAction ThrottleAction = default;
    private InputAction AxisInputAction = default;
    private InputAction RollInputAction = default;
    private InputAction CamSwitchAction = default;
    private InputAction BreakAction = default;


    public static Inputs Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        AxisInput();
        BtnInputHandler();
    }

    private void AxisInput()
    {
        AxisInputAction = _InputActionsAsset.FindActionMap("Player").FindAction("Axis");
        RollInputAction = _InputActionsAsset.FindActionMap("Player").FindAction("Roll");

        AxisInputAction.started += _AxisInput;
        AxisInputAction.performed += _AxisInput;
        AxisInputAction.canceled += _AxisInput;

        RollInputAction.started += ctx => Roll_Input = ctx.ReadValue<float>();
        RollInputAction.performed += ctx => Roll_Input = ctx.ReadValue<float>();
        RollInputAction.canceled += ctx => Roll_Input = 0f;
    }

    private void _AxisInput(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        HrInput = input.x;
        VrInput = input.y;
    }

    private void BtnInputHandler()
    {
        ShootAction = _InputActionsAsset.FindActionMap("Player").FindAction("Shoot");
        CamSwitchAction = _InputActionsAsset.FindActionMap("Player").FindAction("CamSwitch");
        ThrottleAction = _InputActionsAsset.FindActionMap("Player").FindAction("Throttle");
        BreakAction = _InputActionsAsset.FindActionMap("Player").FindAction("Breake");

        ShootAction.started += ctx => ShootBtn = ctx.ReadValueAsButton();
        ShootAction.canceled += ctx => ShootBtn = false;

        ThrottleAction.started += ctx => ThrottleBtn = ctx.ReadValueAsButton();
        ThrottleAction.canceled += ctx => ThrottleBtn = false;

        CamSwitchAction.performed += ctx => CamSwitchBtn =  ctx.ReadValueAsButton();
        CamSwitchAction.canceled += ctx => CamSwitchBtn = ctx.ReadValueAsButton();

        BreakAction.started += ctx => BreakeBtn = ctx.ReadValueAsButton();
        BreakAction.canceled += ctx => BreakeBtn = ctx.ReadValueAsButton();
    }

    private void OnEnable()
    {
        RollInputAction.Enable();
        AxisInputAction.Enable();
        ShootAction.Enable();
        ThrottleAction.Enable();
        CamSwitchAction.Enable();
        BreakAction.Enable();
    }

    private void OnDisable()
    {
        RollInputAction.Disable();
        AxisInputAction.Disable();
        ShootAction.Disable();
        ThrottleAction.Disable();
        CamSwitchAction.Disable();
        BreakAction.Disable();
    }
}
