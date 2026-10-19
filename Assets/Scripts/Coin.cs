using UnityEngine;

public enum CoinType { Normal, Red, Blue }

// On the coin prefab. Collider must have Is Trigger ticked.
public class Coin : MonoBehaviour
{
    public CoinType type = CoinType.Normal;
    [SerializeField] int value = 1;
    [SerializeField] float spinSpeed = 90f;

    void Update() => transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Coin touched by {other.name}");
        if (!other.CompareTag("Player")) return;

        switch (type)
        {
            case CoinType.Normal: GameManager.Instance.AddScore(value); break;
            case CoinType.Red:    GameManager.Instance.RedCoinCollected(); break;
            case CoinType.Blue:   GameManager.Instance.BlueCoinCollected(); break;
        }
        Destroy(gameObject);
    }
}
