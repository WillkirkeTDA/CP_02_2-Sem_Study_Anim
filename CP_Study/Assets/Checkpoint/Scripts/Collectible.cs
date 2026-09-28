using UnityEngine;
using UnityEngine.UI; 

public class Collectible : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private static int totalScore = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            totalScore += 1;
            scoreText.text = "Fruits: " + totalScore;

            Destroy(gameObject);
        }
    }
}