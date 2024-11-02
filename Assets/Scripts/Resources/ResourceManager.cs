using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Resources currentResource;
    public int fossilCount = 0;
    public float refundRate = 1f;

    public bool CanAfford(Resources cost)
    {
        return cost <= currentResource;
    }

    public void Charge(Resources cost)
    {
        currentResource -= cost;
    }

    public void Refund(Resources cost)
    {
        currentResource += cost * refundRate;
    }
}
