//using System.Collections.Generic;
//using System.Collections;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.Pool;

//public class Flamethrower : MonoBehaviour
//{
//    [SerializeField] private ParticleSystem ShootingSystem;
//    [SerializeField] private ParticleSystem OnFireSystemPrefab;
//    [SerializeField] private FlamethrowerRadius AttackRadius;
//    [SerializeField] private int BurningDPS = 5;
//    [SerializeField] private float BurnDuration = 3f;

//    private ObjectPool<ParticleSystem> OnFirePool;

//    private Dictionary<Enemy, ParticleSystem> EnemyParticleSystems = new();

//    private void Awake()
//    {
//        OnFirePool = new ObjectPool<ParticleSystem>(CreateOnFireSystem);
//        AttackRadius.OnEnemyEnter += StartDamagingEnemy;
//        AttackRadius.OnEnemyExit += StopDamagingEnemy;
//    }

//    private ParticleSystem CreateOnFireSystem()
//    {
//        return Instantiate(OnFireSystemPrefab);
//    }

//    private void StartDamagingEnemy(Enemy Enemy)
//    {
//        if (Enemy.TryGetComponent<IBurnable>(out IBurnable burnable))
//        {
//            burnable.StartBurning(BurningDPS);
//            Enemy.Health.OnDeath += HandleEnemyDeath;
//            ParticleSystem onFireSystem = OnFirePool.Get();
//            onFireSystem.transform.SetParent(Enemy.transform, false);
//            onFireSystem.transform.localPosition = Vector3.zero;
//            ParticleSystem.MainModule main = onFireSystem.main;
//            main.loop = true;
//            EnemyParticleSystems.Add(Enemy, onFireSystem);
//        }
//    }

//    private void HandleEnemyDeath(HealthProperty health)
//    {
//        health.OnDeath -= HandleEnemyDeath;
//        if (EnemyParticleSystems.ContainsKey(health))
//        {
//            StartCoroutine(DelayedDisableBurn(health, EnemyParticleSystems[health], BurnDuration));
//            EnemyParticleSystems.Remove(health);
//        }
//    }

//    private IEnumerator DelayedDisableBurn(Health health, ParticleSystem Instance, float Duration)
//    {
//        ParticleSystem.MainModule main = Instance.main;
//        main.loop = false;
//        yield return new WaitForSeconds(Duration);
//        Instance.gameObject.SetActive(false);
//        if (health.TryGetComponent<IBurnable>(out IBurnable burnable))
//        {
//            burnable.StopBurning();
//        }
//    }

//    private void StopDamagingEnemy(HealthProperty health)
//    {
//        health.OnDeath -= HandleEnemyDeath;
//        if (EnemyParticleSystems.ContainsKey(health))
//        {
//            StartCoroutine(DelayedDisableBurn(health, EnemyParticleSystems[health], BurnDuration));
//            EnemyParticleSystems.Remove(health);
//        }
//    }

//    private void Update()
//    {
//        if (Mouse.current.leftButton.isPressed)
//        {
//            Shoot();
//        }
//        else
//        {
//            StopShooting();
//        }
//    }

//    private void Shoot()
//    {
//        ShootingSystem.gameObject.SetActive(true);
//        AttackRadius.gameObject.SetActive(true);
//    }

//    private void StopShooting()
//    {
//        AttackRadius.gameObject.SetActive(false);
//        ShootingSystem.gameObject.SetActive(false);
//    }
//}