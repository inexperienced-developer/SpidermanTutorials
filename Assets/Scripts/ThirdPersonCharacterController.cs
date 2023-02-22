using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Windows;

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
        private float m_AnimMultiplier;
        private float m_LastHorizontal, m_LastVertical;
        [SerializeField] private float m_BlendSpeed = 2;

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
            {
                m_Speed = m_RunSpeed;
                m_AnimMultiplier = 1;
            }
            else
            {
                m_Speed = m_WalkSpeed;
                m_AnimMultiplier = 0.3f;
            }
            Move();
            Rotate();
        }

        private void Move()
        {
            Vector2 input = Input.Move.normalized;
            Vector3 move = transform.right * input.x + transform.forward * input.y;
            m_Controller.Move(move * m_Speed * Time.deltaTime);
            Animate(input);
        }

        private void Animate(Vector2 input)
        {
            float targetHorizontal = input.x * m_AnimMultiplier;
            float blend = Time.deltaTime * m_BlendSpeed;
            if (Mathf.Abs(targetHorizontal - m_LastHorizontal) < 0.1f)
                m_LastHorizontal = targetHorizontal;
            if (targetHorizontal > m_LastHorizontal)
            {
                m_LastHorizontal += blend;
            }
            else if(targetHorizontal < m_LastHorizontal)
            {
                m_LastHorizontal -= blend;
            }
            float targetVertical = input.y * m_AnimMultiplier;
            if (Mathf.Abs(targetVertical - m_LastVertical) < 0.1f)
                m_LastVertical = targetVertical;
            if (targetVertical > m_LastVertical)
                {
                m_LastVertical += blend;
            }
            else if (targetVertical < m_LastVertical)
            {
                m_LastVertical -= blend;
            }

            Mathf.Clamp(m_LastHorizontal, 0, targetHorizontal);
            Mathf.Clamp(m_LastVertical, 0, targetVertical);
            m_Animator.SetFloat("horizontal", m_LastHorizontal);
            m_Animator.SetFloat("vertical", m_LastVertical);
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

