using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

public class PredatorTelegraph : ActionTask
{
    public float moveSpeed;
    public BBParameter<float> defaultMoveSpeed;
    public BBParameter<float> turnSpeed;
    public BBParameter<Transform> location;
    public BBParameter<Transform> theHomie;
    public BBParameter<float> speedBoostDistance;
    public BBParameter<float> boostedMoveSpeed;
    public BBParameter<float> stopDistance;
    public float timePassed;

    public BBParameter<Animator> animator;

    protected override void OnExecute()
    {
        timePassed = 0f;
        
        animator.value.SetBool("chargingUp", true);
    }

    protected override void OnUpdate()
    {
        ////ENEMY CHARGES UP HERE

        Vector3 direction = SteeringUtility.Flee(agent.transform.position, location.value.position);
        agent.transform.Translate(moveSpeed * Time.deltaTime * direction, Space.World);

        Quaternion desiredRotation = Quaternion.LookRotation(-direction);
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, desiredRotation, turnSpeed.value * Time.deltaTime);


        timePassed += Time.deltaTime;
        if (timePassed > 0.5)
        {
            animator.value.SetBool("chargingUp", false);
            EndAction(true);
        }
    }
}