using System.Collections.Generic;
using UnityEngine;

public interface IDragonState {
    IDragonState Attack(Dragon dragon);
    IDragonState Die(Dragon dragon);
    IDragonState Idle(Dragon dragon);
    IDragonState Update(Dragon dragon);
}

public abstract class BaseDragonState {
    protected bool startOfState = true;
    protected IDragonState ChangeState(IDragonState newState, Dragon dragon) {
        if (newState != null) {
            dragon.animProgress = 0;
            return newState;
        } else {
            Debug.LogError("Couldn't find a state to change to!");
            return (IDragonState)this;
        }
    }
    protected float AnimationLength(AnimationClip animationClip, float animationSpeed = 1) {
        return animationClip.length / animationSpeed;
    }
    protected bool AnimationEnded(Dragon dragon, AnimationClip animationClip, float animationSpeed = 1) {
        if (dragon.animProgress >= AnimationLength(animationClip, animationSpeed)) {
            dragon.animProgress = 0;
            return true;
        } else { return false; }
    }
    protected void AnimateState(Dragon dragon, string trigger) {
        dragon.Animator.SetTrigger(trigger);
    }
    protected bool PreviousStateEquals(Dragon dragon, IDragonState state) {
        if (dragon.previousState.ToString() == state.ToString()) { return true; } else { return false; }
    }
}

public class IdleDragonState : BaseDragonState, IDragonState {
    IDragonState IDragonState.Attack(Dragon dragon) {
        throw new System.NotImplementedException();
    }

    IDragonState IDragonState.Die(Dragon dragon) {
        throw new System.NotImplementedException();
    }

    IDragonState IDragonState.Idle(Dragon dragon) {
        throw new System.NotImplementedException();
    }

    IDragonState IDragonState.Update(Dragon dragon) {
        if (dragon.animProgress >= 2) {
            return ChangeState(new PrepareAttackDragonState(), dragon);
        } else {
            return this;
        }
    }
}

public class PrepareAttackDragonState : BaseDragonState, IDragonState {
    IDragonState IDragonState.Attack(Dragon dragon) {
        throw new System.NotImplementedException();
    }

    IDragonState IDragonState.Die(Dragon dragon) {
        throw new System.NotImplementedException();
    }

    IDragonState IDragonState.Idle(Dragon dragon) {
        throw new System.NotImplementedException();
    }

    IDragonState IDragonState.Update(Dragon dragon) {
        if (dragon.animProgress >= 2) {
            return ChangeState(new AttackDragonState(), dragon);
        } else {
            return this;
        }
    }
}

public class AttackDragonState : BaseDragonState, IDragonState {
    IDragonState IDragonState.Attack(Dragon dragon) {
        throw new System.NotImplementedException();
    }

    IDragonState IDragonState.Die(Dragon dragon) {
        throw new System.NotImplementedException();
    }

    IDragonState IDragonState.Idle(Dragon dragon) {
        throw new System.NotImplementedException();
    }

    IDragonState IDragonState.Update(Dragon dragon) {
        if (startOfState) {
            startOfState = false;
            dragon.LaunchFireballs();
        }
        if (dragon.attackAnimation == null || AnimationEnded(dragon, dragon.attackAnimation)) {
            return ChangeState(new IdleDragonState(), dragon);
        } else {
            return this;
        }
    }
}

public class DeadDragonState : BaseDragonState, IDragonState {
    IDragonState IDragonState.Attack(Dragon dragon) => this;

    IDragonState IDragonState.Die(Dragon dragon) => this;

    IDragonState IDragonState.Idle(Dragon dragon) => this;

    IDragonState IDragonState.Update(Dragon dragon) => this;
}

public class FireballData {
    public GameObject fireball;
    public GameObject shadow = null;
    public float speed;
    public Vector3 targetPos;
    public Vector3 destination;
    public float destinationDelta = Mathf.Infinity;
}

public class Dragon : MonoBehaviour, IDamageable {
    [SerializeReference] GameObject fireballProjectile;
    [SerializeReference] GameObject fireballShadow;
    [SerializeField] float fireballSpeed = 4;
    [SerializeReference] Transform firePoint;
    [SerializeReference] Collider fireArea;
    [SerializeReference] Animator animator;
    public AnimationClip idleAnimation;
    public AnimationClip prepareAttackAnimation;
    public AnimationClip attackAnimation;
    public AnimationClip dieAnimation;
    [SerializeField] protected string currentState = new IdleDragonState().ToString();

    [HideInInspector] public IDragonState previousState = new IdleDragonState();
    [HideInInspector] public float animProgress = 0;
    IDragonState state = new IdleDragonState();
    readonly List<FireballData> activeFireballDatas = new();

    MaterialPropertyBlock propertyBlock;
    new SkinnedMeshRenderer renderer;
    float currentMaterialColor = 0f;

    public Animator Animator {
        get { return animator; }
    }

    public virtual void TakeDamage(float amount, bool isDead) {
        SetDmgFlash();
    }

    public virtual void Die(float destroyDelay) {
        state = state.Die(this);
    }

    void SetDmgFlash() {
        currentMaterialColor = 0.4f;
        propertyBlock.SetColor("_EmissionColor", new Color(currentMaterialColor, currentMaterialColor, currentMaterialColor));
        propertyBlock.SetFloat("_EmissionIntensity", currentMaterialColor);
        renderer.SetPropertyBlock(propertyBlock);
    }
    void LessenDmgFlash() {
        currentMaterialColor = Mathf.MoveTowards(currentMaterialColor, 0, 1.25f * Time.deltaTime);
        propertyBlock.SetColor("_EmissionColor", new Color(currentMaterialColor, currentMaterialColor, currentMaterialColor));
        propertyBlock.SetFloat("_EmissionIntensity", currentMaterialColor);
        renderer.SetPropertyBlock(propertyBlock);
    }
    protected void Start() {
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_EmissionColor", Color.black);
        propertyBlock.SetFloat("_EmissionIntensity", 0);
        renderer.SetPropertyBlock(propertyBlock);
    }

    public static Vector3 RandomPointInBounds(Bounds bounds) {
        Vector3 point = new(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z));
        return point;
    }

    private void Update() {
        currentState = state.ToString();
        state = state.Update(this);
        animProgress += Time.deltaTime;
        LessenDmgFlash();

        foreach (FireballData fireballData in activeFireballDatas) {
            if (fireballData.fireball == null) {
                if (fireballData.shadow != null) {
                    Destroy(fireballData.shadow);
                }
                activeFireballDatas.Remove(fireballData);
                return;
            }
            if (fireballData.fireball.transform.position == fireballData.destination) {
                Destroy(fireballData.fireball);
                Destroy(fireballData.shadow);
                return;
            }
            if (fireballData.fireball.transform.position == fireballData.targetPos) {
                FallFromSky(fireballData);
            } else {
                fireballData.fireball.transform.position = new Vector3(
                    Mathf.MoveTowards(fireballData.fireball.transform.position.x, fireballData.targetPos.x, fireballData.speed * Time.deltaTime),
                    Mathf.MoveTowards(fireballData.fireball.transform.position.y, fireballData.targetPos.y, fireballData.speed * Time.deltaTime),
                    Mathf.MoveTowards(fireballData.fireball.transform.position.z, fireballData.targetPos.z, fireballData.speed * Time.deltaTime));
                if (fireballData.shadow != null) {
                    fireballData.shadow.transform.localScale = new Vector3(
                        1.5f - Vector3.Distance(fireballData.fireball.transform.position, fireballData.destination) / fireballData.destinationDelta, 0,
                        1.5f - Vector3.Distance(fireballData.fireball.transform.position, fireballData.destination) / fireballData.destinationDelta);
                }
            }
        }
    }

    public void LaunchFireballs() {
        GameObject newFireball = Instantiate(fireballProjectile, firePoint.position, Quaternion.identity);
        Vector3 fireballImpactArea = RandomPointInBounds(fireArea.bounds);

        FireballData newFireballData = new();
        newFireballData.fireball = newFireball;
        newFireballData.speed = fireballSpeed;
        newFireballData.targetPos = new(fireballImpactArea.x, fireballImpactArea.y + 7, fireballImpactArea.z);
        newFireballData.destination = new(fireballImpactArea.x, fireballImpactArea.y, fireballImpactArea.z);
        newFireball.transform.LookAt(newFireballData.targetPos);
        activeFireballDatas.Add(newFireballData);
    }

    void FallFromSky(FireballData fireballData) {
        fireballData.targetPos = fireballData.destination;
        fireballData.speed = fireballSpeed / 4;
        GameObject newShadow = Instantiate(fireballShadow, fireballData.destination, Quaternion.identity);
        fireballData.destinationDelta = Vector3.Distance(fireballData.fireball.transform.position, fireballData.destination);
        newShadow.transform.localScale = Vector3.zero;
        fireballData.shadow = newShadow;
    }
}
