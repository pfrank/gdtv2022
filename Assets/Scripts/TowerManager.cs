using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private Tower[] towers;

    public Tower[] Towers
    {
        get
        {
            return towers;
        }
    }
}
