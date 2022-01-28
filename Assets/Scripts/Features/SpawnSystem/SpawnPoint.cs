using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private bool isFree;
    public bool IsFree
    {
        get { return this.isFree; }
        set { isFree = value; }
    }
}
