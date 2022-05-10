using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraController : MonoBehaviour
{
    //------------------------------------------------------
    //                  VARIABLES
    //------------------------------------------------------

    private GameManager gameManager;
    [SerializeField] private CinemachineFreeLook cinemachine;
    [SerializeField] private float xlookSpeed = 10f;
    [SerializeField] private float ylookSpeed = 1f;
    private InputControl inputControl;


    private void Awake() {
        gameManager = GameManager.Instance;
        inputControl = gameManager.GetInputControl();
        cinemachine = GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable() {
        //inputControl.CharacterControls.Look.Enable();
    }
    private void OnDisable() {
        //inputControl.CharacterControls.Look.Disable();
    }

    private void Update() {
        Vector2 delta = inputControl.CharacterControls.Look.ReadValue<Vector2>();
        cinemachine.m_XAxis.Value += delta.x * xlookSpeed * Time.deltaTime;
        cinemachine.m_YAxis.Value += delta.y * ylookSpeed * Time.deltaTime;
    }

}
