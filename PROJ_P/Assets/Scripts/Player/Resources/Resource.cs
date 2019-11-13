using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : ScriptableObject
{
    public float Value { get; protected set; } = 1;
    protected Color resourceMeter;
    protected Image resourceImage;
    public string name;


    public virtual void DrainResource(PlayerAttack activeAttack) {

        Value -= activeAttack.GetCastCost() / 100;
        CorrectValue();
        UpdateFillAmount();

    }

    public virtual void DrainResource(float amount)
    {
        Value -= amount / 100;
        //If round is used, rage will not decrease
        Value = Mathf.Clamp(Value, 0, 1);
        UpdateFillAmount();
    }

    public void DrainResourceRounded(float amount)
    {
        Value -= amount / 100;
        CorrectValue();
        UpdateFillAmount();
    }

    public virtual void CacheComponents(Image resourceImage)
    {
        this.resourceImage = resourceImage;
        

    }

    protected virtual void SetResourceColor() {

        resourceImage.color = resourceMeter;
    }

    public virtual void IncreaseResource(float resource)
    {
        Value += resource;
        CorrectValue();
        UpdateFillAmount();
    }

    private void UpdateFillAmount()
    {
        if (resourceImage != null)
        {
            resourceImage.fillAmount = Value;
        }
    }
    private void CorrectValue()
    {
        Value = Mathf.Clamp(Value, 0, 1);
        Value = Mathf.Round(Value * 100) / 100;
    }
}
