using UnityEngine;

public interface IScorpionState {
    IScorpionState Careless(Scorpion scorpion);
    IScorpionState Flee(Scorpion scorpion);
    IScorpionState Update(Scorpion scorpion);
}

public abstract class ScorpionBaseState {
    protected IScorpionState ChangeState(IScorpionState newState, Scorpion scorpion) {
        if (newState != null) {
            return newState;
        } else {
            Debug.LogError("Couldn't find a state to change to!");
            return (IScorpionState)this;
        }
    }
}

public class CarelessScorpionState : ScorpionBaseState, IScorpionState {
    IScorpionState IScorpionState.Careless(Scorpion scorpion) => this;

    IScorpionState IScorpionState.Flee(Scorpion scorpion) => ChangeState(new FleeingScorpionState(), scorpion);

    IScorpionState IScorpionState.Update(Scorpion scorpion) {
        
        return this;
    }
}

public class FleeingScorpionState : ScorpionBaseState, IScorpionState {
    IScorpionState IScorpionState.Careless(Scorpion scorpion) => ChangeState(new CarelessScorpionState(), scorpion);

    IScorpionState IScorpionState.Flee(Scorpion scorpion) => this;

    IScorpionState IScorpionState.Update(Scorpion scorpion) {
        scorpion.Flee();
        return this;
    }
}

public class Scorpion : MonoBehaviour, IDamageable {
    [SerializeField] protected string currentState = new CarelessScorpionState().ToString();
    IScorpionState state = new CarelessScorpionState();
    [SerializeField] float movementSpeed = 1;
    public GameObject target;
    [SerializeReference] FieldOfView fov;

    MaterialPropertyBlock propertyBlock;
    new SkinnedMeshRenderer[] renderer;
    MeshRenderer[] meshRenderer;
    float currentMaterialColor = 0f;

    void IDamageable.TakeDamage(float amount, bool isDead) {
        fov.canSeeTarget = true;
        SetDmgFlash();
    }

    void IDamageable.Die(float destroyDelay) {
        Destroy(gameObject);
    }

    void SetDmgFlash() {
        foreach (var skinnedMeshRenderer in renderer) {
            currentMaterialColor = 0.4f;
            propertyBlock.SetColor("_EmissionColor", new Color(currentMaterialColor, currentMaterialColor, currentMaterialColor));
            propertyBlock.SetFloat("_EmissionIntensity", currentMaterialColor);
            skinnedMeshRenderer.SetPropertyBlock(propertyBlock);
        }
        foreach (var item in meshRenderer) {
            currentMaterialColor = 0.4f;
            propertyBlock.SetColor("_EmissionColor", new Color(currentMaterialColor, currentMaterialColor, currentMaterialColor));
            propertyBlock.SetFloat("_EmissionIntensity", currentMaterialColor);
            item.SetPropertyBlock(propertyBlock);
        }
    }
    void LessenDmgFlash() {
        foreach (var skinnedMeshRenderer in renderer) {
            currentMaterialColor = Mathf.MoveTowards(currentMaterialColor, 0, 1.25f * Time.deltaTime);
            propertyBlock.SetColor("_EmissionColor", new Color(currentMaterialColor, currentMaterialColor, currentMaterialColor));
            propertyBlock.SetFloat("_EmissionIntensity", currentMaterialColor);
            skinnedMeshRenderer.SetPropertyBlock(propertyBlock);
        }
        foreach (var item in meshRenderer) {
            currentMaterialColor = Mathf.MoveTowards(currentMaterialColor, 0, 1.25f * Time.deltaTime);
            propertyBlock.SetColor("_EmissionColor", new Color(currentMaterialColor, currentMaterialColor, currentMaterialColor));
            propertyBlock.SetFloat("_EmissionIntensity", currentMaterialColor);
            item.SetPropertyBlock(propertyBlock);
        }
    }

    private void Start() {
        if (!target) {
            target = Camera.main.gameObject;
        }
        fov.target = target;
        renderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var skinnedMeshRenderer in renderer) {
            propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_EmissionColor", Color.black);
            propertyBlock.SetFloat("_EmissionIntensity", 0);
            skinnedMeshRenderer.SetPropertyBlock(propertyBlock);
        }
        meshRenderer = GetComponentsInChildren<MeshRenderer>();
        foreach (var item in meshRenderer) {
            propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_EmissionColor", Color.black);
            propertyBlock.SetFloat("_EmissionIntensity", 0);
            item.SetPropertyBlock(propertyBlock);
        }
    }

    private void Update() {
        currentState = state.ToString();
        state = state.Update(this);

        if (fov.canSeeTarget) {
            state = state.Flee(this);
        } else {
            state = state.Careless(this);
        }

        LessenDmgFlash();
    }

    public void Flee() {
        Vector3 targetPosition = new(target.transform.position.x, transform.position.y, target.transform.position.z);
        Vector3 newPosition = transform.position - targetPosition;
        Quaternion lookRotation = Quaternion.LookRotation(newPosition);
        transform.SetPositionAndRotation(transform.position + movementSpeed * Time.deltaTime * transform.forward,
            Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * movementSpeed));
    }
}
