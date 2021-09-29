using System.Collections;
using UnityEngine;
using WorldSystems;

namespace Player
{
    [RequireComponent(typeof(PlayerColliders))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerDash : MonoBehaviour

    {
        private CinemachineShake m_Shake;
        private PlayerJump m_Jump;
        private CoyoteTime m_Coyote;
        private PlayerWalk m_Walk;
        private PlayerColliders m_Colliders;
        private Rigidbody2D m_Rigidbody2D;
        private PlayerInput m_Input;
        [SerializeField] private float normalGravity = 5f;
        [SerializeField] private float dashSpeed = 10f;

        private void Awake()
        {
            m_Shake = GetComponent<CinemachineShake>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Colliders = GetComponent<PlayerColliders>();
            m_Input = GetComponent<PlayerInput>();
            m_Walk = GetComponent<PlayerWalk>(); 
            m_Coyote = GetComponent<CoyoteTime>();
            m_Jump = GetComponent<PlayerJump>();
        }

        private void Update()
        {
            if (m_Coyote.canCoyote) return;
            if (m_Input.dash)
            {
                if (m_Coyote.hasDashed || m_Coyote.isDashing) return;
                Dash();
            }
        }

        private void Dash()
        {
            if (m_Input.moveVector.y > 0f) return;
            if (m_Coyote.hasDashed || m_Coyote.isDashing) return;
            if (m_Input.moveVector == Vector2.zero) return;
            CinemachineShake.Instance.CameraShake(3f, 0.3f);
            m_Coyote.hasDashed = true;
            m_Walk.enabled = false;
            m_Jump.enabled = false;
            m_Input.enabled = false;

            if (m_Input.moveVector.magnitude >= 1)
            {
               m_Input.moveVector = m_Input.moveVector.normalized;
            }
            
            
            Vector2 velocity = new Vector2(m_Input.moveVector.x * dashSpeed, -m_Input.moveVector.y * dashSpeed * 0.6f);
            m_Rigidbody2D.velocity = velocity;
            
            StartCoroutine(DashWait());
        }
        
        private IEnumerator DashWait()
        {
            StartCoroutine(GroundDash());
            m_Rigidbody2D.gravityScale = 0f;
            m_Coyote.isDashing = true;
            yield return new WaitForSeconds(0.3f);
            m_Rigidbody2D.gravityScale = normalGravity;
            m_Input.enabled = true;
            m_Walk.enabled = true;
            m_Jump.enabled = true;
            m_Coyote.isDashing = false;
        }
        IEnumerator GroundDash()
        {
            yield return new WaitForSeconds(.15f);
            if (m_Colliders.IsGrounded())
                m_Coyote.hasDashed = false;
        }
    }
}

