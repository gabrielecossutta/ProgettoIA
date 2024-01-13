using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodButton : MonoBehaviour
{
    public event EventHandler OnUsed;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material greenDarkMaterial;
    public FoodSpawner FoodSpawner;

    public MeshRenderer buttonMeshRenderer;
    public Transform buttonTransform;
    private bool canUseButton;

    private void Awake()
    {
        Debug.Log("reset");
        buttonTransform = transform.Find("Button");
        buttonMeshRenderer = buttonTransform.GetComponent<MeshRenderer>();
        canUseButton = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetButton();
    }

    public bool CanUseButton()
    {
        return canUseButton;
    }
    public void UseButton()
    {
        if (canUseButton)
        {
            buttonMeshRenderer.material = greenDarkMaterial;
            buttonTransform.localScale = new Vector3(.5f, .2f, .5f);
            canUseButton = false;
            FoodSpawner.SpawnFood();
            OnUsed?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ResetButton()
    {
        canUseButton = true;
        buttonMeshRenderer.material = greenMaterial;
        buttonTransform.localScale = new Vector3(.5f, .5f, .5f);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, UnityEngine.Random.Range(-2f,+2f));
    }
}
