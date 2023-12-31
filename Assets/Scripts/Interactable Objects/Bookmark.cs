using UnityEngine;

public class Bookmark : MonoBehaviour
{
    [SerializeField] public int index;

    void Start()
    {
        if (index == 0) Select();
    }

    public void Select()
    {
        Book.Instance.SelectBookmark(this);

        transform.localScale = Vector3.one * 1.05f;
    }

    public void UnSelect()
    {
        transform.localScale = Vector3.one;
    }
}
