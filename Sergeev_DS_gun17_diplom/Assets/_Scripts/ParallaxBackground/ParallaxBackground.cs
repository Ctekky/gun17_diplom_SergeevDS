using Cinemachine;
using UnityEngine;

namespace Metroidvania.Common
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        [SerializeField] private float parallaxEffect;
        private float _xPosition;
        private float _length;

        private void Start()
        {
            var position = transform.position;
            _xPosition = position.x;
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Update()
        {
            var cameraPosition = playerCamera.transform.position;
            var distanceMoved = cameraPosition.x * (1 - parallaxEffect);
            var distanceToMove = cameraPosition.x * parallaxEffect;
            var transform1 = transform;
            //transform1.position.y
            transform1.position = new Vector3(_xPosition + distanceToMove, cameraPosition.y);
            if (distanceMoved > _xPosition + _length) _xPosition += _length;
            else if (distanceMoved < _xPosition - _length) _xPosition -= _length;
        }
    }
}