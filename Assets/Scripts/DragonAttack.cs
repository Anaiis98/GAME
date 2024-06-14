using UnityEngine;

public class DragonAttack : MonoBehaviour
{
    private GameObject player;
    private bool hasAttacked = false;

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    void Update()
    {
        if (player != null && !hasAttacked)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.05f);
        }
    }

    public void Attack()
    {
        if (player != null)
        {
            hasAttacked = true;
            Debug.Log("Dragon has attacked the player!");
            // Logic to stop the game or show game over screen
            // For example:
            // player.GetComponent<PlayerController>().Die();
            // or
            // Time.timeScale = 0; // Pause the game
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Attack();
        }
    }
}