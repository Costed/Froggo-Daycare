using UnityEngine;

public class FrogSpawner : MonoBehaviour
{
    public static FrogSpawner Instance;

    [SerializeField] GameObject frogPrefab;
    [SerializeField] GameObject jarPrefab;
    [SerializeField] float frogSpawnDelay;
    float frogSpawnTime;

    [SerializeField] AnimationCurve frogSpawnTimeCurve;
    [SerializeField] bool spawnFrogs;

    bool frogSpawned = true;
    Jar lastJar;

    [SerializeField] AudioClip frogArriveClip;
    [SerializeField] ParticleSystem frogPoof;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!spawnFrogs || GameManager.IsPaused) return;

        frogSpawnTime += Time.deltaTime;

        frogSpawnDelay = frogSpawnTimeCurve.Evaluate((Time.timeSinceLevelLoad / 60f));

        if (frogSpawnTime >= frogSpawnDelay)
        {
            frogSpawnTime -= frogSpawnDelay;
            //SpawnFrog();
            SpawnJar();
            SoundManager.Instance.PlayPooledSoundAtPosition(frogArriveClip, transform.position);
        }
    }

    void SpawnJar()
    {
        if (!frogSpawned) lastJar.Activate(true);
        frogSpawned = false;
        lastJar = Instantiate(jarPrefab, transform.position, transform.rotation).GetComponent<Jar>();
    }

    public void SpawnFrog(Transform jarTransform, bool spawnAngry = false)
    {
        frogSpawned = true;
        Frog frog = Instantiate(frogPrefab, jarTransform.position, jarTransform.rotation).GetComponent<Frog>();
        frogPoof.gameObject.transform.position = jarTransform.position;
        frogPoof.Play();

        GameManager.IncreaseFrogCount();
        if (spawnAngry) frog.stats.ForceSetStatsToZero();
    }
}
