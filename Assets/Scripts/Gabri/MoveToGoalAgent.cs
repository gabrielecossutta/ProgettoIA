using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgents : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer flootMeshRender;
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-3f,+1f),0, Random.Range(4f, -4f));
        targetTransform.localPosition = new Vector3(Random.Range(-2f,-6f),0, Random.Range(4f, -4f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float moveSpeed = 5f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
        SetReward(1f);
            flootMeshRender.material = winMaterial;
        EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
        SetReward(-1f);
            flootMeshRender.material = loseMaterial;
        EndEpisode();
        }
    }
}
