using System;

[Serializable]
public class ResourceAmount
{
    public ResourceData Resource;
    public int Amount;

    public ResourceAmount(ResourceData resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }
}
