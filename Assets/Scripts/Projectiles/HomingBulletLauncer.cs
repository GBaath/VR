using UnityEngine;

public class HomingBulletLauncer : MonoBehaviour
{
    [SerializeField] bool trigger;
    [SerializeReference] GameObject homingBullet;

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            Instantiate(homingBullet, transform.position, Quaternion.identity);
        }
    }
}
