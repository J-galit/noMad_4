using NodeCanvas.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveToWithTimer : ActionTask
{
    public float moveSpeed;
    public BBParameter<float> defaultMoveSpeed;
    public BBParameter<float> turnSpeed;
    public BBParameter<Transform> location;
    public BBParameter<Transform> theHomie;
    public BBParameter<float> speedBoostDistance;
    public BBParameter<float> boostedMoveSpeed;
    public BBParameter<float> stopDistance;
    public BBParameter<float> timer;
    public float timePassed;

    protected override void OnExecute()
    {
        //used to speed up player and ally NPC
        boostedMoveSpeed.value = moveSpeed * 2;
        timePassed = 0f;
    }

    protected override void OnUpdate()
    {
        

        Vector3 direction = SteeringUtility.Seek(agent.transform.position, location.value.position);
        agent.transform.Translate(moveSpeed * Time.deltaTime * direction, Space.World);

        Quaternion desiredRotation = Quaternion.LookRotation(direction);
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, desiredRotation, turnSpeed.value * Time.deltaTime);



        if (Vector3.Distance(agent.transform.position, theHomie.value.position) < stopDistance.value)
        {
            //When close enough both the player and NPC gain a movement boost
            moveSpeed = boostedMoveSpeed.value;
            //clamps max move speed
            moveSpeed = Mathf.Clamp(moveSpeed, defaultMoveSpeed.value, 10f);

        }
        else
        {
            moveSpeed = defaultMoveSpeed.value;
        }

        timePassed += Time.deltaTime;
        if (timePassed > timer.value)
        {
            EndAction(true);
        }
    }
}
