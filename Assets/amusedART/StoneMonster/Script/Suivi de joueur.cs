using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Référence au transform du joueur
    public float followSpeed = 5f; // Vitesse de suivi

    void Update()
    {
        if (player != null)
        {
            // Déplace le dragon vers le joueur
            transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);

            // Optionnel : faire tourner le dragon vers le joueur
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * followSpeed);
        }
    }
}
