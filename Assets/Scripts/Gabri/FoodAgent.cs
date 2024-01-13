using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class FoodAgent : Agent
{
    public event EventHandler OnAteFood;
    public event EventHandler OnEpisodeBeginEvent;

    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private FoodButton foodButton;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    private Rigidbody agentRigidbody;

    private void Awake()
    {
        agentRigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(UnityEngine.Random.Range(-2.5f,+2.5f),0, UnityEngine.Random.Range(-2f, 2f));
        OnEpisodeBeginEvent?.Invoke(this, EventArgs.Empty);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(foodButton.CanUseButton()?1:0);
        Vector3 dirToFoodButton = (foodButton.transform.localPosition - transform.localPosition).normalized;
        sensor.AddObservation(dirToFoodButton.x);
        sensor.AddObservation(dirToFoodButton.z);
        sensor.AddObservation(foodSpawner.HasFoodSpawned() ? 1 : 0);
        if (foodSpawner.HasFoodSpawner())
        {
            Vector3 dirToFood = (foodSpawner.GetLastFoodTransform().localPosition - transform.localPosition).normalized;
            sensor.AddObservation(dirToFood.x);
            sensor.AddObservation(dirToFood.z);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
        
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveX = actions.DiscreteActions[0];//0 = dont move , 1 left, 2 rigth
        int moveZ = actions.DiscreteActions[1];//0 = dont move , 1 back, 2 forward
        Vector3 addForce = new Vector3(0, 0, 0);
        switch(moveX)
        {
            case 0 : addForce.x = 0f; break;
            case 1 : addForce.x = -1f; break;
            case 2 : addForce.x = 1f; break;
        }
        switch(moveZ)
        {
            case 0 : addForce.z = 0f; break;
            case 1 : addForce.z = -1f; break;
            case 2 : addForce.z = 1f; break;
        }
        float moveSpeed = 5f;
        agentRigidbody.velocity = addForce * moveSpeed + new Vector3(0, agentRigidbody.velocity.y, 0);
        bool isUseButtonDown = actions.DiscreteActions[2] == 1;
        if (isUseButtonDown)
        {
            Collider[] colliderArray = Physics.OverlapBox(transform.position, Vector2.one * .5f);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<FoodButton>(out FoodButton foodButton))
                {
                    if (foodButton.CanUseButton())
                    {
                        foodButton.UseButton();
                        AddReward(1f);
                    }
                }
            }
        }
        AddReward(-1f / MaxStep);
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        switch ( Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")))
        {
            case -1: discreteActions[0] = 1; break;
            case  0: discreteActions[0] = 0; break;
            case +1: discreteActions[0] = 2; break;
        }
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical")))
        {
            case -1: discreteActions[1] = 1; break;
            case  0: discreteActions[1] = 0; break;
            case +1: discreteActions[1] = 2; break;
        }
        discreteActions[2] = Input.GetKey(KeyCode.E) ? 1 : 0;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Food>(out Food food))
        {
            AddReward(1F);
            Destroy(food.gameObject);
            OnAteFood?.Invoke(this, EventArgs.Empty);
            foodButton.ResetButton();
            EndEpisode();
        }
    }
}
