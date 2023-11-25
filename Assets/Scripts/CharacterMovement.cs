using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement instance;
    
    private float previousMouseX;
    private float mouseX;
    private bool isMovingX;
    public bool IsMoving { get; set; }
    public bool GameOver { get; set; }
    
    
    private Animator animator;

    private Camera cam;
    private Plane plane = new(Vector3.forward, 0);

    [Header("References")] 
    public PostProcessVolume gameOverPpv;

    [Header("Properties")] 
    [SerializeField] private float runSpeed;

    [SerializeField] private float runSpeedX;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        cam = Camera.main;
        animator = GetComponentInChildren<Animator>();
    }

    void Update() {
        if(GameOver) return;
        Run();
        MouseUp();
        MouseDown();
        MouseMove();
    }

    private void Run() {
        if (!IsMoving) return;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z + runSpeed * Time.deltaTime);
    }

    private void MouseDown() {
        if (Input.GetMouseButtonDown(0)) {
            if (!IsMoving) {
                animator.SetFloat("RunSpeed", runSpeed);
                IsMoving = true;
            }

            previousMouseX = Input.mousePosition.x;
            mouseX = previousMouseX;

            isMovingX = true;
        }
    }

    private void MouseUp() {
        if (Input.GetMouseButtonUp(0)) {
            isMovingX = false;
        }
    }

    private void MouseMove() {
        if (!isMovingX) return;

        if (!Input.GetMouseButton(0)) return;
        mouseX = Input.mousePosition.x;

        float deltaX = mouseX - previousMouseX;
        float clampedX =
            Mathf.Clamp(Mathf.Lerp(transform.position.x, transform.position.x + deltaX, Time.deltaTime * runSpeedX), -1,
                1);
        transform.position = new Vector3(clampedX, 0, transform.position.z);
        previousMouseX = mouseX;
    }
}