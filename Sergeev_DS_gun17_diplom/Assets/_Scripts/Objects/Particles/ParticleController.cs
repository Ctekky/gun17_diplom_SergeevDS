using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private void OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
