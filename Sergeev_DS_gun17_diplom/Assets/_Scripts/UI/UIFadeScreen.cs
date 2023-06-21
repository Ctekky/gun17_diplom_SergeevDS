using UnityEngine;
using UnityEngine.Serialization;


namespace Metroidvania.UI
{
    public class UIFadeScreen : MonoBehaviour
    { 
        [SerializeField] private Animator animator;
        
        public void FadeIn() => animator.SetTrigger("FadeIn");
        public void FadeOut() => animator.SetTrigger("FadeOut");
    }
    
}
