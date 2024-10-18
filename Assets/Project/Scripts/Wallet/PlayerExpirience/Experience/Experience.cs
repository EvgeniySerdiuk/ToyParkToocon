using Project.Scripts;
using UnityEngine;

public class Experience : MonoBehaviour
{
    private const int amountExp = 1;

    private float delay;

    private bool used;
    
    private void OnTriggerStay(Collider other)
    {
        delay += Time.deltaTime;

        if (delay < 0.5f) return;
        transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 10 * Time.deltaTime);

        if(transform.position == other.transform.position)
        {
            if (other.TryGetComponent<PlayerTag>(out var tag))
            {
                tag.AddExp(amountExp);
                delay = 0;
                used = true;
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if(!used)
            GlobalEvents.AddExp(amountExp);
    }

    // public void LastBreath()
    // {
    //     GlobalEvents.AddExp(amountExp);
    // }
}
