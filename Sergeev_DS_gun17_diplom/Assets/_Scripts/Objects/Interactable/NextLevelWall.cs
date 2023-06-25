using UnityEngine;
using UnityEngine.SceneManagement;

namespace  Metroidvania.Common.Objects
{
    public class NextLevelWall : MonoBehaviour
    {
        [SerializeField] private string nextLevelName;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.GetComponentInParent<Player.Player>() == null) return;
            SceneManager.LoadScene(nextLevelName);
        }
    }
    
}
