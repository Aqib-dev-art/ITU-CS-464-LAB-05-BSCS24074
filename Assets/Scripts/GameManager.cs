using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Base game (Lecture 06)")]
    [SerializeField] int targetScore = 15;      // > 10 so the red coin can still appear at score 10
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject winPanel;

    [Header("Lab 05 tasks")]
    [SerializeField] GameObject coinPrefab;     // same prefab the Spawner uses
    [SerializeField] Transform player;
    [SerializeField] TMP_Text bonusText;        // hidden until a blue coin is collected
    [SerializeField] float arenaHalfSize = 7f;
    [SerializeField] int bonusCount = 50;
    [SerializeField] float bonusRadius = 5f;

    int score;
    int bonusCollected;
    int redSpawnScore;        // random 1..10
    bool redSpawned;          // only ONE red coin ever

    void Awake() => Instance = this;

    void Start()
    {
        redSpawnScore = 1;    // 1..10 inclusive
        bonusText.gameObject.SetActive(false);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();

        // Task 1: spawn the red coin once, when score reaches the random value
        if (!redSpawned && score >= redSpawnScore) SpawnRedCoin();

        if (score >= targetScore) Win();
    }

    void UpdateUI() => scoreText.text = $"Score: {score} / {targetScore}";

    // ---- Task 1 ----
    void SpawnRedCoin()
    {
        redSpawned = true;
        Vector3 pos = new Vector3(
            Random.Range(-arenaHalfSize, arenaHalfSize), 0.5f,
            Random.Range(-arenaHalfSize, arenaHalfSize));
        MakeCoin(pos, CoinType.Red, Color.red);
    }

    // ---- Task 2 ----
    public void RedCoinCollected()
    {
        for (int i = 0; i < bonusCount; i++)
        {
            float a = i * Mathf.PI * 2f / bonusCount;
            Vector3 pos = player.position +
                new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * bonusRadius;
            pos.y = 0.5f;
            MakeCoin(pos, CoinType.Blue, Color.blue);
        }
    }

    // ---- Task 3 ----
    public void BlueCoinCollected()
    {
        bonusCollected++;
        bonusText.gameObject.SetActive(true);
        bonusText.text = $"BonusCoins: {bonusCollected}/{bonusCount}";
    }

    // Not parented to the Spawner, so special coins don't count against maxCoins
    void MakeCoin(Vector3 pos, CoinType type, Color color)
    {
        GameObject c = Instantiate(coinPrefab, pos, Quaternion.identity);
        c.GetComponent<Coin>().type = type;
        c.GetComponent<Renderer>().material.color = color;
    }

    void Win() { winPanel.SetActive(true); Time.timeScale = 0f; }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
