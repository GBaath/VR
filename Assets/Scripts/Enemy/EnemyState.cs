using UnityEngine;

public interface IEnemyState {
    IEnemyState Attack(Enemy enemy);

    IEnemyState Attack2(Enemy enemy);

    IEnemyState Chase(Enemy enemy);

    IEnemyState Cheer(Enemy enemy);

    IEnemyState Confuse(Enemy enemy);

    IEnemyState Dance(Enemy enemy);

    IEnemyState Die(Enemy enemy);

    IEnemyState Idle(Enemy enemy);

    IEnemyState Surprise(Enemy enemy);

    IEnemyState Update(Enemy enemy);
}

public abstract class BaseEnemyState {
    protected bool startOfState = true;

    protected void ResetAllTriggers(Enemy enemy) {
        foreach (var param in enemy.Animator.parameters) {
            if (param.type == AnimatorControllerParameterType.Trigger) {
                enemy.Animator.ResetTrigger(param.name);
            }
        }
    }

    protected IEnemyState ChangeState(IEnemyState newState, Enemy enemy, bool smoothTransition = false) {
        if (newState != null) {
            startOfState = true;
            enemy.animTimer = 0;
            ResetAllTriggers(enemy);
            if (!smoothTransition) {
                enemy.Animator.SetFloat(enemy.EnemyData.animSpeed, enemy.animTimer);
            }
            enemy.FieldOfView.currentRadiusIncrease = 0;
            enemy.attackAnimationLoops = 0;
            enemy.canDamage = false;
            enemy.previousState = (IEnemyState)this;
            return newState;
        } else { Debug.LogError("Couldn't find a state to change to!"); return (IEnemyState)this; }
    }

    protected float AnimationLength(AnimationClip animationClip, float animationSpeed = 1) {
        return animationClip.length / animationSpeed;
    }

    protected bool AnimationEnded(Enemy enemy, AnimationClip animationClip, float animationSpeed = 0) {
        if (animationSpeed == 0) {
            animationSpeed = enemy.animationSpeed;
        }
        if (enemy.animTimer >= AnimationLength(animationClip, animationSpeed)) {
            enemy.animTimer = 0;
            return true;
        } else { return false; }
    }

    protected void AnimateState(Animator animator, string trigger, float speed = 1) {
        animator.SetTrigger(trigger);
        animator.speed = speed;
    }

    protected bool PreviousStateEquals(Enemy enemy, IEnemyState state) {
        if (enemy.previousState.ToString() == state.ToString()) { return true; } else { return false; }
    }
}

public class IdleEnemyState : BaseEnemyState, IEnemyState {
    IEnemyState IEnemyState.Attack(Enemy enemy) => ChangeState(new SurprisedEnemyState(), enemy, true);

    IEnemyState IEnemyState.Attack2(Enemy enemy) => ChangeState(new SurprisedEnemyState(), enemy, true);

    IEnemyState IEnemyState.Chase(Enemy enemy) => ChangeState(new SurprisedEnemyState(), enemy, true);

    IEnemyState IEnemyState.Cheer(Enemy enemy) => ChangeState(new CheerEnemyState(), enemy);

    IEnemyState IEnemyState.Confuse(Enemy enemy) => this;

    IEnemyState IEnemyState.Dance(Enemy enemy) => ChangeState(new DanceEnemyState(), enemy);

    IEnemyState IEnemyState.Die(Enemy enemy) => ChangeState(new DeadEnemyState(), enemy);

    IEnemyState IEnemyState.Idle(Enemy enemy) => this;

    IEnemyState IEnemyState.Surprise(Enemy enemy) => ChangeState(new SurprisedEnemyState(), enemy, true);

    IEnemyState IEnemyState.Update(Enemy enemy) {
        if (startOfState) {
            startOfState = false;

            enemy.animTimer = 1;
            AnimateState(enemy.Animator, enemy.EnemyData.idleTrigger);
            if (!PreviousStateEquals(enemy, new ConfusedEnemyState())) { AnimateState(enemy.Animator, enemy.EnemyData.idleTrigger); }

            enemy.Animator.SetFloat(enemy.EnemyData.animSpeed, enemy.animTimer);
        }
        return this;
    }
}

public class SurprisedEnemyState : BaseEnemyState, IEnemyState {
    IEnemyState IEnemyState.Attack(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.surprisedAnimation)) { return ChangeState(new AttackEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Attack2(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.surprisedAnimation)) { return ChangeState(new Attack2EnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Chase(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.surprisedAnimation)) { return ChangeState(new ChaseEnemyState(), enemy, true); } else { return this; }
    }

    IEnemyState IEnemyState.Cheer(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.surprisedAnimation)) { return ChangeState(new CheerEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Confuse(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.surprisedAnimation)) { return ChangeState(new ConfusedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Dance(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.surprisedAnimation)) { return new DanceEnemyState(); } else { return this; }
    }

    IEnemyState IEnemyState.Die(Enemy enemy) => ChangeState(new DeadEnemyState(), enemy);

    IEnemyState IEnemyState.Idle(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.surprisedAnimation)) { return ChangeState(new ChaseEnemyState(), enemy, true); } else { return this; }
    }

    IEnemyState IEnemyState.Surprise(Enemy enemy) => this;

    IEnemyState IEnemyState.Update(Enemy enemy) {
        if (startOfState) {
            startOfState = false;

            AnimateState(enemy.Animator, enemy.EnemyData.surpriseTrigger);
        }
        enemy.Animator.SetFloat(enemy.EnemyData.animSpeed, enemy.animTimer);
        enemy.TurnTowardsTarget(enemy.chaseTurnSpeedMultiplier);
        return this;
    }
}

public class ChaseEnemyState : BaseEnemyState, IEnemyState {
    IEnemyState IEnemyState.Attack(Enemy enemy) => ChangeState(new AttackEnemyState(), enemy, true);

    IEnemyState IEnemyState.Attack2(Enemy enemy) => ChangeState(new Attack2EnemyState(), enemy, true);

    IEnemyState IEnemyState.Chase(Enemy enemy) => this;

    IEnemyState IEnemyState.Cheer(Enemy enemy) => ChangeState(new CheerEnemyState(), enemy);

    IEnemyState IEnemyState.Confuse(Enemy enemy) => ChangeState(new ConfusedEnemyState(), enemy);

    IEnemyState IEnemyState.Dance(Enemy enemy) => ChangeState(new DanceEnemyState(), enemy);

    IEnemyState IEnemyState.Die(Enemy enemy) => ChangeState(new DeadEnemyState(), enemy);

    IEnemyState IEnemyState.Idle(Enemy enemy) => ChangeState(new ConfusedEnemyState(), enemy);

    IEnemyState IEnemyState.Surprise(Enemy enemy) => ChangeState(new SurprisedEnemyState(), enemy);

    IEnemyState IEnemyState.Update(Enemy enemy) {
        if (startOfState) {
            startOfState = false;

            AnimateState(enemy.Animator, enemy.EnemyData.chaseTrigger);
        }
        enemy.Animator.SetFloat(enemy.EnemyData.animSpeed, enemy.animTimer);
        enemy.TurnTowardsTarget(enemy.chaseTurnSpeedMultiplier);
        enemy.Chase();
        return this;
    }
}

public class AttackEnemyState : BaseEnemyState, IEnemyState {
    IEnemyState IEnemyState.Attack(Enemy enemy) => this;

    IEnemyState IEnemyState.Attack2(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new Attack2EnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Chase(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new ChaseEnemyState(), enemy, true); } else { return this; }
    }

    IEnemyState IEnemyState.Cheer(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new ConfusedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Confuse(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new ConfusedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Dance(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new ConfusedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Die(Enemy enemy) => ChangeState(new DeadEnemyState(), enemy);

    IEnemyState IEnemyState.Idle(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new ConfusedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Surprise(Enemy enemy) => this;

    IEnemyState IEnemyState.Update(Enemy enemy) {
        if (startOfState) {
            startOfState = false;
            AnimateState(enemy.Animator, enemy.EnemyData.attackTrigger);
            enemy.canDamage = true;
            enemy.FieldOfView.currentRadiusIncrease = enemy.FieldOfView.radiusIncrease;
        }

        if (!enemy.FieldOfView.canSeeTarget) { enemy.canDamage = false; }

        if (enemy.animTimer >= enemy.attackAnimationImpactTime * AnimationLength(enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier) && enemy.canDamage) {
            enemy.canDamage = false;
            enemy.Attack();
        }

        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { enemy.canDamage = true; }

        enemy.Animator.SetFloat(enemy.EnemyData.animSpeed, enemy.animTimer + enemy.attackAnimationLoops);
        enemy.TurnTowardsTarget();
        return this;
    }
}

public class Attack2EnemyState : BaseEnemyState, IEnemyState {
    IEnemyState IEnemyState.Attack(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new AttackEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Attack2(Enemy enemy) => this;

    IEnemyState IEnemyState.Chase(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new ChaseEnemyState(), enemy, true); } else { return this; }
    }

    IEnemyState IEnemyState.Cheer(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new ConfusedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Confuse(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new ConfusedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Dance(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new ConfusedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Die(Enemy enemy) => ChangeState(new DeadEnemyState(), enemy);

    IEnemyState IEnemyState.Idle(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { return ChangeState(new ConfusedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Surprise(Enemy enemy) => this;

    IEnemyState IEnemyState.Update(Enemy enemy) {
        if (startOfState) {
            startOfState = false;
            AnimateState(enemy.Animator, enemy.EnemyData.attackTrigger);
            enemy.canDamage = true;
            enemy.FieldOfView.currentRadiusIncrease = enemy.FieldOfView.radiusIncrease;
        }

        if (!enemy.FieldOfView.canSeeTarget) { enemy.canDamage = false; }

        if (enemy.animTimer >= enemy.attackAnimationImpactTime * AnimationLength(enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier) && enemy.canDamage) {
            enemy.canDamage = false;
            enemy.Attack();
        }

        if (AnimationEnded(enemy, enemy.EnemyData.attackAnimation, enemy.EnemyData.AttackSpeedMultiplier)) { enemy.canDamage = true; }

        enemy.Animator.SetFloat(enemy.EnemyData.animSpeed, enemy.animTimer + enemy.attackAnimationLoops);
        enemy.TurnTowardsTarget();
        return this;
    }
}

public class ConfusedEnemyState : BaseEnemyState, IEnemyState {
    IEnemyState IEnemyState.Attack(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.confusedAnimation)) { return ChangeState(new SurprisedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Attack2(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.confusedAnimation)) { return ChangeState(new SurprisedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Chase(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.confusedAnimation)) { return ChangeState(new SurprisedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Cheer(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.confusedAnimation)) { return ChangeState(new CheerEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Confuse(Enemy enemy) => this;

    IEnemyState IEnemyState.Dance(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.confusedAnimation)) { return ChangeState(new DanceEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Die(Enemy enemy) => ChangeState(new DeadEnemyState(), enemy);

    IEnemyState IEnemyState.Idle(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.confusedAnimation)) { return ChangeState(new IdleEnemyState(), enemy, true); } else { return this; }
    }

    IEnemyState IEnemyState.Surprise(Enemy enemy) {
        if (AnimationEnded(enemy, enemy.EnemyData.confusedAnimation)) { return ChangeState(new SurprisedEnemyState(), enemy); } else { return this; }
    }

    IEnemyState IEnemyState.Update(Enemy enemy) {
        if (startOfState) {
            startOfState = false;

            AnimateState(enemy.Animator, enemy.EnemyData.confuseTrigger);
        }
        enemy.Animator.SetFloat(enemy.EnemyData.animSpeed, enemy.animTimer / AnimationLength(enemy.EnemyData.confusedAnimation));
        return this;
    }
}

public class DanceEnemyState : BaseEnemyState, IEnemyState {
    IEnemyState IEnemyState.Attack(Enemy enemy) {
        return new SurprisedEnemyState();
    }

    IEnemyState IEnemyState.Attack2(Enemy enemy) {
        return new SurprisedEnemyState();
    }

    IEnemyState IEnemyState.Chase(Enemy enemy) {
        return new SurprisedEnemyState();
    }

    IEnemyState IEnemyState.Cheer(Enemy enemy) {
        return new CheerEnemyState();
    }

    IEnemyState IEnemyState.Confuse(Enemy enemy) {
        return new SurprisedEnemyState();
    }

    IEnemyState IEnemyState.Dance(Enemy enemy) => this;

    IEnemyState IEnemyState.Die(Enemy enemy) {
        return new DeadEnemyState();
    }

    IEnemyState IEnemyState.Idle(Enemy enemy) {
        return new IdleEnemyState();
    }

    IEnemyState IEnemyState.Surprise(Enemy enemy) {
        return new SurprisedEnemyState();
    }

    IEnemyState IEnemyState.Update(Enemy enemy) {
        return this;
    }
}

public class CheerEnemyState : BaseEnemyState, IEnemyState {
    IEnemyState IEnemyState.Attack(Enemy enemy) {
        return new SurprisedEnemyState();
    }

    IEnemyState IEnemyState.Attack2(Enemy enemy) {
        return new SurprisedEnemyState();
    }

    IEnemyState IEnemyState.Chase(Enemy enemy) {
        return new SurprisedEnemyState();
    }

    IEnemyState IEnemyState.Cheer(Enemy enemy) => this;

    IEnemyState IEnemyState.Confuse(Enemy enemy) {
        return new SurprisedEnemyState();
    }

    IEnemyState IEnemyState.Dance(Enemy enemy) {
        return new DanceEnemyState();
    }

    IEnemyState IEnemyState.Die(Enemy enemy) {
        return new DeadEnemyState();
    }

    IEnemyState IEnemyState.Idle(Enemy enemy) {
        return new IdleEnemyState();
    }

    IEnemyState IEnemyState.Surprise(Enemy enemy) {
        return new SurprisedEnemyState();
    }

    IEnemyState IEnemyState.Update(Enemy enemy) {
        return this;
    }
}

public class DeadEnemyState : BaseEnemyState, IEnemyState {
    IEnemyState IEnemyState.Attack(Enemy enemy) => this;

    IEnemyState IEnemyState.Attack2(Enemy enemy) => this;

    IEnemyState IEnemyState.Chase(Enemy enemy) => this;

    IEnemyState IEnemyState.Cheer(Enemy enemy) => this;

    IEnemyState IEnemyState.Confuse(Enemy enemy) => this;

    IEnemyState IEnemyState.Dance(Enemy enemy) => this;

    IEnemyState IEnemyState.Die(Enemy enemy) {
        // Remain dead if dead
        return this;
    }

    IEnemyState IEnemyState.Idle(Enemy enemy) => this;

    IEnemyState IEnemyState.Surprise(Enemy enemy) => this;

    IEnemyState IEnemyState.Update(Enemy enemy) {
        // Decay
        return this;
    }
}