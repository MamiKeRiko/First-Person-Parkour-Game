using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCharacterController : MonoBehaviour
{
    [SerializeField] float m_walkSpeed = 8f;
    [SerializeField] float m_runSpeed = 12.5f;
    [SerializeField] float m_gravity = 9.8f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_jumpForwardMomentumRatio = 0.03f;

    [SerializeField] float m_playerCameraRotationSpeed;
    [SerializeField] float m_playerCameraRotationXLimit;

    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform m_floorDetection;

    [SerializeField] float m_airDrag;
    [SerializeField] float m_groundDrag;

    [SerializeField] float m_staminaRecoveryPerSecond;
    [SerializeField] float m_staminaRunningLossPerSecond;
    [SerializeField] float m_staminaRanOutCooldown;
    [SerializeField] float m_staminaLossPerJump;

    Vector2 m_playerCameraRotation;
    Vector3 m_velocity;
    Vector3 m_realVelocity;

    bool m_hasJumped = true;
    bool m_hasBeenPushed = false;
    bool m_airborne = false;
    bool m_running = false;
    bool m_canUseStamina = true;

    float currentSpeed;
    
    CharacterController m_controller;
    Camera m_playerCamera;
    Player m_player;

    public delegate void Sprint();
    public Sprint StartSprint;
    public Sprint EndSprint;

    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_player = GetComponent<Player>();
        m_playerCamera = Camera.main;
        currentSpeed = m_walkSpeed;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //input
        Vector3 input;
        Vector3 move;
        input = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        if (input.magnitude > 1) input.Normalize();
        move = input;
        move *= currentSpeed;

        //floor detection
        if (m_velocity.y < 0 && Physics.CheckSphere(m_floorDetection.position, 0.5f, groundMask))
        {
            if (m_hasJumped) //wait 1 frame before applying ground drag
            {
                m_hasJumped = false;
            }
            else
            {
                m_airborne = false;
                m_velocity.y = -1;
            }
        }
        else
        {
            m_velocity.y -= m_gravity * Time.deltaTime;
            m_hasJumped = true;
            m_airborne = true;
        }

        //running
        if(Input.GetButton("Sprint") && m_canUseStamina && !m_airborne && move.magnitude > 1f && m_player.GetStamina() > 0)
        {
            if (!m_running)
            {
                m_running = true;
                StartSprint();
            }
            
            currentSpeed = m_runSpeed;
            m_player.SubtractStamina(m_staminaRunningLossPerSecond * Time.deltaTime);

            if(m_player.GetStamina() <= 0)
            {
                StaminaRanOut();
            }
        }
        else
        {
            if (m_running)
            {
                m_running = false;
                EndSprint();
            }

            currentSpeed = m_walkSpeed;
            m_player.AddStamina(m_staminaRecoveryPerSecond * Time.deltaTime);
        }

        //drag
        if (m_airborne)
        {
            LerpAxisToZero(ref m_velocity.x, m_airDrag);
            LerpAxisToZero(ref m_velocity.z, m_airDrag);
        }
        else
        {
            LerpAxisToZero(ref m_velocity.x, m_groundDrag);
            LerpAxisToZero(ref m_velocity.z, m_groundDrag);
        }

        //jump
        if (!m_hasJumped && m_canUseStamina && Input.GetButton("Jump"))
        {
            m_player.SubtractStamina(m_staminaLossPerJump);

            m_velocity.y = m_jumpForce;
            Vector3 jumpForwardPush = input * m_jumpForce * currentSpeed * m_jumpForwardMomentumRatio;
            m_velocity += jumpForwardPush;

            m_hasJumped = true;
            m_airborne = true;

            if (m_player.GetStamina() <= 0)
            {
                StaminaRanOut();
            }
        }

        //apply move
        m_realVelocity = m_velocity + move;
        m_controller.Move(m_realVelocity * Time.deltaTime);

        if (Mathf.Abs(m_velocity.x) < Mathf.Abs(move.x)) m_velocity.x = 0;
        if (Mathf.Abs(m_velocity.z) < Mathf.Abs(move.z)) m_velocity.z = 0;

        //camera movement & player rotation
        m_playerCameraRotation.y += Input.GetAxis("Mouse X") * m_playerCameraRotationSpeed;
        m_playerCameraRotation.x += -Input.GetAxis("Mouse Y") * m_playerCameraRotationSpeed;
        m_playerCameraRotation.x = Mathf.Clamp(m_playerCameraRotation.x, -m_playerCameraRotationXLimit, m_playerCameraRotationXLimit);

        m_playerCamera.transform.localRotation = Quaternion.Euler(m_playerCameraRotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, m_playerCameraRotation.y);
    }

    void StaminaRanOut()
    {
        m_canUseStamina = false;
        UIManager.Instance.ToggleCanRun(m_canUseStamina);
        Invoke("ActivateRunning", m_staminaRanOutCooldown);
    }

    public void Push(Vector3 vec) //used to add explosion force
    {
        Debug.Log(vec);
        m_velocity += vec;
        m_hasJumped = true;
        m_airborne = true;
    }

    void LerpAxisToZero(ref float value, float rate)
    {
        if (value > 0.001f || value < -0.001f)
            value = Mathf.Lerp(value, 0, rate);
        else value = 0;
    }

    void ActivateRunning()
    {
        m_canUseStamina = true;
        UIManager.Instance.ToggleCanRun(m_canUseStamina);
    }
}
