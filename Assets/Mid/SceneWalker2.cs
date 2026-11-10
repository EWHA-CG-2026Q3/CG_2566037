// SceneWalker2.cs
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SceneWalker2 : MonoBehaviour
{
    public Transform cameraTransform;      // 자식으로 둔 카메라 (눈높이)
    public float moveSpeed = 2.5f;         // 걷는 속도 (초당 m)
    public float runMultiplier = 2f;       // Shift를 누르면 곱해지는 배율
    public float lookSensitivity = 0.1f;   // 마우스 감도
    public float gravity = -9.8f;

    CharacterController controller;
    float pitch;          // 위아래로 고개를 든 각도
    float verticalSpeed;  // 떨어지는 속도

    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.stepOffset = 0.5f;

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;

            // 카메라를 플레이어의 자식으로 만들고, 위치를 사람 눈높이로 끌어올립니다.
            cameraTransform.parent = this.transform;
            cameraTransform.localPosition = new Vector3(0f, 0.7f, 0f);
            cameraTransform.localRotation = Quaternion.identity;
        }
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (kb == null || mouse == null) return;

        // 마우스 고정: 클릭하면 고정, Esc로 해제
        if (mouse.leftButton.wasPressedThisFrame) Cursor.lockState = CursorLockMode.Locked;
        if (kb.escapeKey.wasPressedThisFrame) Cursor.lockState = CursorLockMode.None;

        // 둘러보기: 좌우는 몸 전체, 위아래는 카메라만
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Vector2 delta = mouse.delta.ReadValue() * lookSensitivity;
            transform.Rotate(0f, delta.x, 0f);
            pitch = Mathf.Clamp(pitch - delta.y, -80f, 80f);
            if (cameraTransform != null) cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }

        // 이동: 바라보는 방향 기준 앞뒤, 좌우
        Vector3 input = Vector3.zero;
        if (kb.wKey.isPressed) input.z += 1f;
        if (kb.sKey.isPressed) input.z -= 1f;
        if (kb.dKey.isPressed) input.x += 1f;
        if (kb.aKey.isPressed) input.x -= 1f;

        if (kb.upArrowKey.isPressed) input.z += 1f;
        if (kb.downArrowKey.isPressed) input.z -= 1f;
        if (kb.rightArrowKey.isPressed) input.x += 1f;
        if (kb.leftArrowKey.isPressed) input.x -= 1f;

        float speed = moveSpeed * (kb.shiftKey.isPressed ? runMultiplier : 1f);
        Vector3 move = transform.TransformDirection(input.normalized) * speed;

        // 중력: 땅에 닿아 있으면 살짝 눌러 두고, 아니면 떨어짐
        // [수정 포인트] 바닥 및 계단에 더 확실히 밀착하도록 -1f -> -2f로 변경
        if (controller.isGrounded && verticalSpeed < 0f) verticalSpeed = -2f;
        verticalSpeed += gravity * Time.deltaTime;
        move.y = verticalSpeed;

        controller.Move(move * Time.deltaTime);
    }
}