using UnityEngine;

public class ReferenceSetter : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;

    void Awake()
    {
        SetReferences();
    }

    void OnValidate()
    {
        SetReferences();
    }

    [ContextMenu("Set References")]
    void SetReferences()
    {
        PlayerData.playerTransform = playerTransform;
        PlayerData.cameraTransform = cameraTransform;
    }
}
