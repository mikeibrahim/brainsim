using UnityEngine;

public class Agent : MonoBehaviour
{
    private UKS_Data data;

    // Getters
    public UKS_Data GetData() => data;
    // Setters
    public void SetData(UKS_Data data) => this.data = data;
}
