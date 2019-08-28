using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/State")]
public class State : ScriptableObject
{
    public Action[] actions;
    public Transition[] transitions;

    public void UpdateState(StateController controller) {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateController controller){
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    private void CheckTransitions(StateController controller){
        if(transitions == null) return;
        for (int i = 0; i < transitions.Length; i++)
        {
            
            if(transitions[i].decision.Decide(controller)){
                controller.TransitionState(transitions[i].trueState);                
            }
            else{
                controller.TransitionState(transitions[i].falseState);
            }
        }
    }
    
}
