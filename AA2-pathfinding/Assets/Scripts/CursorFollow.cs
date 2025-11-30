using AI;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollow : MonoBehaviour
{
    private bool active = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputSystem.actions.FindAction("Jump").started += Toggle;
    }

    private void Toggle(InputAction.CallbackContext ctx)
    {
        active = !active;
        GetComponent<DynamicObstacle>().enabled = active;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Camera.main.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue() + Vector3.forward);
        this.transform.position -= Vector3.forward;
    }
}
