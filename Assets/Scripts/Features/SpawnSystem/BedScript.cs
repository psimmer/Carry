using UnityEngine;

public class BedScript : MonoBehaviour
{
    [SerializeField] private bool isFree;
    public bool IsFree
    {
        get { return this.isFree; }
        set { isFree = value; }
    }
}
