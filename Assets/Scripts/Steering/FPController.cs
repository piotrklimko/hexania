using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour {

    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private float m_GravityMultiplier;

    private Camera m_Camera;
    public bool m_Jump;
    private float m_YRotation;
    private Vector3 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    public bool m_Jumping;
    private AudioSource m_AudioSource;

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;

    public Animator avatar_animator;


    public Quaternion m_CharacterTargetRot;
    public Quaternion m_CameraTargetRot;

    Vector3 v_shiftx;
    //Vector3 v_shifty;
    Vector3 v_shiftz;

    public bool fly_mode;

    // Use this for initialization
    void Start() {

        fly_mode = false;

        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_CharacterTargetRot = transform.localRotation;
        m_CameraTargetRot = m_Camera.transform.localRotation;

      

    }

    // Update is called once per frame
    void Update()
    {
        LookRotation(transform, m_Camera.transform);


        if (!fly_mode)
        {
            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                //StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


    }

    void FixedUpdate() {


            if (!fly_mode) {

                if (!m_Jump) {
                    m_Jump = MyInput.jump;
                }
            }

            float speed;


           

            if (!fly_mode) {
                GetInput(out speed);
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = transform.forward * m_Input.z + transform.right * m_Input.x;

                // get a normal for the surface that is being touched to move along it
                RaycastHit hitInfo;
                Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                    m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

                m_MoveDir.x = desiredMove.x * speed;
                m_MoveDir.z = desiredMove.z * speed;

                if (m_CharacterController.isGrounded) {
                    m_MoveDir.y = -m_StickToGroundForce;

                    if (m_Jump) {
                        m_MoveDir.y = m_JumpSpeed;
                        PlayJumpSound();
                        m_Jump = false;
                        m_Jumping = true;
                    }
                }
                else {
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                    m_Jump = false;
                }
            }
            else {

                GetInput(out speed);
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = transform.forward * m_Input.z + transform.right * m_Input.x + transform.up * m_Input.y;

                m_MoveDir.x = desiredMove.x * speed;
                m_MoveDir.z = desiredMove.z * speed;
                m_MoveDir.y = desiredMove.y * speed;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
        
    }

    void PlayJumpSound() {

    }

    void PlayLandingSound() {

    }

    private void GetInput(out float speed) {
        // Read input

        float horizontal = MyInput.my_steering_h;
        float vertical = MyInput.my_steering_v;
        float updown = MyInput.my_steering_up_down;
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector3(horizontal,updown, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1) {
            m_Input.Normalize();
        }

    }

    public void LookRotation(Transform character, Transform camera) {

        if (TouchArea.instance == null) return;

        float yRot = TouchArea.instance.delta_pos.x * XSensitivity;
        float xRot = TouchArea.instance.delta_pos.y * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth) {
            character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else {
            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q) {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below) {
            return;
        }

        if (body == null || body.isKinematic) {
            return;
        }

        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

    public void FlyMode(bool flymode) {
        fly_mode = flymode;

    }
}
