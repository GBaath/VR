using UnityEngine;

public interface IScorpionState {
    IScorpionState Flee(Scorpion scorpion);
    IScorpionState Update(Scorpion scorpion);
}

public abstract class ScorpionBaseState {
    protected IScorpionState ChangeState(IScorpionState newState, Scorpion scorpion) {
        if (newState != null) {
            //scorpion.animProgress = 0;
            return newState;
        } else {
            Debug.LogError("Couldn't find a state to change to!");
            return (IScorpionState)this;
        }
    }
}

public class CarelessScorpionState : ScorpionBaseState, IScorpionState {
    IScorpionState IScorpionState.Flee(Scorpion scorpion) => ChangeState(new FleeingScorpionState(), scorpion);

    IScorpionState IScorpionState.Update(Scorpion scorpion) => this;
}

public class FleeingScorpionState : ScorpionBaseState, IScorpionState {
    IScorpionState IScorpionState.Flee(Scorpion scorpion) => this;

    IScorpionState IScorpionState.Update(Scorpion scorpion) {
        scorpion.Flee();
        return this;
    }
}

public class Scorpion : MonoBehaviour {
    //[HideInInspector] public float animProgress = 0;
    [SerializeField] protected string currentState = new CarelessScorpionState().ToString();
    IScorpionState state = new FleeingScorpionState();
    [SerializeField] float movementSpeed = 1;
    [SerializeField] GameObject target;

    private void Start() {
        if (!target) {
            target = Camera.main.gameObject;
        }
    }

    private void Update() {
        currentState = state.ToString();
        state = state.Update(this);
    }

    public void Flee() {
        transform.LookAt(new Vector3(-target.transform.position.x, transform.position.y, -target.transform.position.z));
        //transform.Translate(movementSpeed * Time.deltaTime * transform.forward, Space.World);
    }
}
