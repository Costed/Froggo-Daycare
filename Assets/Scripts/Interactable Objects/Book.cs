using HietakissaUtils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour, IInteractable
{
    public static Book Instance;

    [SerializeField] Transform bookLookTransform;
    Bookmark activeBookmark;

    [SerializeField] Transform[] menus;

    [SerializeField] Transform hatButtonHolder;
    [SerializeField] Transform hatButtonHolder2;

    [SerializeField] Transform[] generalBookmarkPages;
    [SerializeField] Transform[] hatBookmarkPages;

    [SerializeField] Image xpBarImage;
    [SerializeField] TextMeshProUGUI[] xpTexts;
    float xpBarFillAmount;
    float xpBarFillTargetAmount;

    [SerializeField] float xpBarFillSpeed;

    void Awake()
    {
        Instance = this;

        PlayerData.bookLookTransform = bookLookTransform;
    }

    void Start()
    {
        LevelManager.Instance.OnXPGain += GainedXP;
        LevelManager.Instance.AddXP(0);

        StartUsingBook();
    }

    public void SelectBookmark(Bookmark bookmark)
    {
        if (activeBookmark != null) activeBookmark.UnSelect();
        activeBookmark = bookmark;
        Debug.Log($"Selected bookmark {activeBookmark.index}");

        OpenMenu(0);

        if (activeBookmark.index == 0)
        {
            foreach (Transform t in generalBookmarkPages) t.gameObject.SetActive(true);
            foreach (Transform t in hatBookmarkPages) t.gameObject.SetActive(false);
        }
        else
        {
            foreach (Transform t in generalBookmarkPages) t.gameObject.SetActive(false);
            foreach (Transform t in hatBookmarkPages) t.gameObject.SetActive(true);
        }
    }

    public void OpenMenu(int index)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].gameObject.SetActive(i == index);
        }
    }

    public void Interact()
    {
        Debug.Log("Interacted with book");

        StartUsingBook();
    }

    public void StartUsingBook()
    {
        //GameManager.Pause();
        GameManager.EnterBook();
        PlayerData.usingBook = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StopUsingBook()
    {
        OpenMenu(0);
        GameManager.ExitBook();
        PlayerData.usingBook = false;

        Cursor.lockState = CursorLockMode.Locked;

        SettingsManager.Instance.SaveSettings();
        Cursor.visible = false;
    }

    public void UnlockHat(int id)
    {
        Debug.Log($"Unlocked hat {id}");
        
        if (id < 4) hatButtonHolder.GetChild(id).gameObject.SetActive(true);
        else hatButtonHolder2.GetChild(id - 4).gameObject.SetActive(true);
    }

    void GainedXP(int currentXP, int maxXP)
    {
        xpBarFillTargetAmount = currentXP / (float)maxXP;
        if (fillXPBarRoutine != null) StopCoroutine(fillXPBarRoutine);
        fillXPBarRoutine = StartCoroutine(FillXPBar());

        foreach (TextMeshProUGUI text in xpTexts) text.text = $"New Hat: {currentXP}/{maxXP}";
    }

    Coroutine fillXPBarRoutine;
    IEnumerator FillXPBar()
    {
        while (xpBarFillAmount != xpBarFillTargetAmount)
        {
            xpBarFillAmount = Mathf.Lerp(xpBarFillAmount, xpBarFillTargetAmount, xpBarFillSpeed * Time.deltaTime);
            //if ((xpBarFillAmount - xpBarFillTargetAmount).Abs() < 0.02f) xpBarFillAmount = xpBarFillTargetAmount;

            xpBarImage.fillAmount = xpBarFillAmount;
            yield return null;
        }
    }
}
