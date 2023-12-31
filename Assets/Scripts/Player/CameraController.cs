using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField] float sensitivity;

    [SerializeField] bool invertVertical;
    [SerializeField] bool invertHorizontal;

    [SerializeField] float maxLookAngle = 90f;

    Transform holder;

    float xRot, yRot;

    [SerializeField] float lerpSpeed = 1.5f;

    Vector3 targetPos;
    Quaternion targetRot;
    Vector3 startPos;
    Quaternion startRot;
    float lerpTime = 5f;

    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 144;

        PlayerData.cameraTransform = transform;
        PlayerData.playerCamera = GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;

        yRot = transform.rotation.y;
    }

    void Start()
    {
        holder = PlayerData.cameraHolder;

        lerpTime = 1f;

        //Book.Instance.StartUsing();
    }

    void Update()
    {
        if (GameManager.IsPaused) return;
        if (PlayerData.usingBook) return;

        GetInput();
        Rotate();
    }

    void LateUpdate()
    {
        /*if (lerpTime >= 1f && !PlayerData.usingBook) transform.position = holder.position;
        else
        {
            lerpTime += Time.deltaTime * lerpSpeed;

            transform.position = Vector3.Slerp(startPos, targetPos, lerpTime);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, lerpTime);
        }*/

        if (!PlayerData.usingBook)
        {
            targetPos = holder.position;
            targetRot = holder.rotation;
        }

        lerpTime += Time.deltaTime * lerpSpeed;
        
        transform.position = Vector3.Slerp(startPos, targetPos, lerpTime);
        transform.rotation = Quaternion.Slerp(startRot, targetRot, lerpTime);
    }

    void GetInput()
    {
        yRot += Input.GetAxisRaw("Mouse X") * (invertHorizontal ? -PlayerData.sensitivity : PlayerData.sensitivity);
        xRot += Input.GetAxisRaw("Mouse Y") * (invertVertical ? PlayerData.sensitivity : -PlayerData.sensitivity);

        xRot = Mathf.Clamp(xRot, -maxLookAngle, maxLookAngle);

        if (yRot > 360f) yRot -= 360f;
        else if (yRot < -360f) yRot += 360f;
    }

    void Rotate()
    {
        holder.rotation = Quaternion.Euler(xRot, yRot, 0f);
        PlayerData.playerTransform.rotation = Quaternion.Euler(0f, yRot, 0f);
    }

    void EnterBook()
    {
        if (Time.timeSinceLevelLoad < 1f) lerpTime = 1f;
        else lerpTime = 0f;

        startPos = transform.position;
        startRot = transform.rotation;

        targetPos = PlayerData.bookLookTransform.position;
        targetRot = PlayerData.bookLookTransform.rotation;
    }

    void ExitBook()
    {
        lerpTime = 0f;

        //targetRot = startRot;
        //targetPos = startPos;

        startPos = transform.position;
        startRot = transform.rotation;
    }

    void OnEnable()
    {
        GameManager.OnEnterBook += EnterBook;
        GameManager.OnExitBook += ExitBook;
    }

    void OnDisable()
    {
        GameManager.OnEnterBook -= EnterBook;
        GameManager.OnExitBook -= ExitBook;
    }
}
