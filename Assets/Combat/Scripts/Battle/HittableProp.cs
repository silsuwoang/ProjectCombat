using UnityEngine;

public class HittableProp : MonoBehaviour
{
    [SerializeField] private HealthComponent healthComponent;
    
    [SerializeField] private float maxHP;
    [SerializeField] private GameObject normalObject;
    [SerializeField] private GameObject brokenObject;
    
    private void Start()
    {
        healthComponent.Init(maxHP, null,
            () =>
            {
                if (normalObject)
                {
                    normalObject.SetActive(false);
                }
                if (brokenObject)
                {
                    brokenObject.SetActive(true);
                }

                GetComponent<Collider>().enabled = false;
            }, null);
    }
}
