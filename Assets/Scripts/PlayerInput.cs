using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions m_Actions;

    public void Init()
    {
        m_Actions = new PlayerInputActions();
        m_Actions.Enable();
    }

    private void OnDisable()
    {
        m_Actions?.Disable();
    }

    public Vector2 Move => m_Actions.Player.Move.ReadValue<Vector2>();
    public bool Sprint => m_Actions.Player.Sprint.IsPressed();
    public bool RightClick => m_Actions.Player.RightClick.IsPressed();
}
