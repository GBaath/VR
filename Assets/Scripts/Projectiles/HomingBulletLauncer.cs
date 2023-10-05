using UnityEngine;

public class HomingBulletLauncer : MonoBehaviour
{
    [SerializeField] bool trigger;
    [SerializeField] float dmg = 1;
    [SerializeReference] GameObject homingBullet;

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            GameObject newBullet = Instantiate(homingBullet, transform.position, Quaternion.identity);
            newBullet.TryGetComponent(out ProjectileDamage pd);
            pd.damage = dmg;
        }
    }
}
