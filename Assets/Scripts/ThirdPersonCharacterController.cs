using UnityEditor.Rendering;
using UnityEngine;

namespace InexperiencedDeveloper
{
    public class ThirdPersonCharacterController : MonoBehaviour
    {
        public PlayerInput Input { get; protected set; }
        private CharacterController m_Controller;
        private Animator m_Animator;

        [SerializeField] private float m_RunSpeed;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RotationSpeed = 100;

        private float m_Speed;

        private float m_LastLookRot;
        private float m_LastCamRot;
        private float m_Lerp;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            Input = GetComponent<PlayerInput>();
            Input.Init();
            m_Controller = GetComponent<CharacterController>();
            m_Animator = GetComponent<Animator>();
            m_LastLookRot = transform.eulerAngles.y;
            m_LastCamRot = Camera.main.transform.eulerAngles.y;
        }

        private void Update()
        {
            if (Input.Sprint)
                m_Speed = m_RunSpeed;
            else
                m_Speed = m_WalkSpeed;
            Move();
            Rotate();
        }

        private void Move()
        {
            Vector2 input = Input.Move.normalized;
            Vector3 move = transform.right * input.x + transform.forward * input.y;
            m_Controller.Move(move * m_Speed * Time.deltaTime);
            m_Animator.SetFloat("horizontal", move.x);
            m_Animator.SetFloat("vertical", move.z);
        }

        private void Rotate()
        {
            if (Input.RightClick)
            {
                m_LastLookRot = transform.eulerAngles.y;
                m_LastCamRot = Camera.main.transform.eulerAngles.y;
                m_Lerp = 0;
            }
            if (m_Lerp >= 1) return;
            m_Lerp += Time.deltaTime * m_RotationSpeed;
            float lookRot = Mathf.Lerp(m_LastLookRot, m_LastCamRot, m_Lerp);
            Vector3 rotation = transform.eulerAngles;
            rotation.y = lookRot;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}

