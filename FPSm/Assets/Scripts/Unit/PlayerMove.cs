using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float cameraRoationLimit;
    [SerializeField] private Camera theCamera;
    [SerializeField] private GameObject[] CrossHair;
    private float characterRotationLimit;
    private float currentCameraRotationX;
    private Vector3 movDir;
    private CharacterController cc;
    private float angleY;
    private float angleX;
    GameObject SubCamera;

    PlayerFire theFire;

    private void Start()
    {
        theFire = GetComponent<PlayerFire>();
        Cursor.lockState = CursorLockMode.Locked;
        movDir = Vector3.zero;
        SubCamera = GameObject.FindGameObjectWithTag("SubCamera");
        SubCamera.SetActive(false);
        cc = GetComponent<CharacterController>();
        angleX = transform.eulerAngles.x;
        angleY = theCamera.transform.localEulerAngles.y;
        CrossHair = GameObject.FindGameObjectsWithTag("CrossHair");
        for (int i = 0; i < CrossHair.Length; i++)
        {
            CrossHair[i].SetActive(true);
        }
    }

    void Update()
    {
        CharacterMove();
        CameraRotation();
        CharacterRotation();
        SubCameraRotation();
        ChangeTo3view();
    }

    void CharacterMove()
    {
        if (cc.isGrounded)
        {
            movDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            movDir = transform.TransformDirection(movDir);
            movDir *= walkSpeed;
            movDir.y = 0.0f;
            if (Input.GetButton("Jump")) movDir.y = jumpForce;
        }
        else
            movDir.y -= gravity * Time.deltaTime;
        cc.Move(movDir * Time.deltaTime);
    }

    void CameraRotation()
    {
        float dirX = -Input.GetAxisRaw("Mouse Y");
        angleX += dirX * lookSensitivity * Time.deltaTime;
        angleX = Mathf.Clamp(angleX, -cameraRoationLimit, cameraRoationLimit);
        theCamera.transform.localEulerAngles = new Vector3(angleX, 0.0f, 0.0f);
    }

    void CharacterRotation()
    {
        float dirY = Input.GetAxisRaw("Mouse X");
        angleY += dirY * lookSensitivity * Time.deltaTime;
        transform.eulerAngles = new Vector3(0.0f, angleY, 0.0f);
    }

    void SubCameraRotation()
    {
        float dirX = -Input.GetAxisRaw("Mouse Y");
        angleX += dirX * lookSensitivity * Time.deltaTime;
        angleX = Mathf.Clamp(angleX, -cameraRoationLimit, cameraRoationLimit);
        SubCamera.transform.localEulerAngles = new Vector3(angleX, 0.0f, 0.0f);
    }

    void ChangeTo3view()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            if (SubCamera.activeSelf)
            {
                SubCamera.SetActive(false);
                theFire.ChangeCamera(Camera.main);
            }
            else
            {
                SubCamera.SetActive(true);
                theFire.ChangeCamera(SubCamera.GetComponent<Camera>());
            }
        }
    }
}
