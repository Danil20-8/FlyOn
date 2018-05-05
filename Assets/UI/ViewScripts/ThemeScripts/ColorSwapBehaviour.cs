using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ColorSwapBehaviour : StateMachineBehaviour {

    [SerializeField]
    ThemeElementType type;

    Color from;
    Image image;

    Color to;

    float progress = 1f;

    static System.Action onEnter;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        image = animator.GetComponent<Image>();
        from = image.color;

        to = ThemeManagerBehaviour.GetColor(type);

        progress = 0;

    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (progress >= 1f)
            return;
        progress += Time.deltaTime * stateInfo.speed;

        if (progress >= 1f)
        {
            image.color = to;
        }
        else {
            image.color = Color.Lerp(from, to, progress);
        }
        
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
