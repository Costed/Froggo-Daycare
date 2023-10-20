using UnityEngine;

public class HelperText : MonoBehaviour
{
    float time;

    void Awake()
    {
        if (GameManager.TutorialShown)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > 60f)
        {
            GameManager.TutorialShown = true;

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
