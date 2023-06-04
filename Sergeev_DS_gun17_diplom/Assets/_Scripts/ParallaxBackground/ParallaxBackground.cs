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
        _xPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void Update() {
        var cameraPosition = playerCamera.transform.position;
        var distanceMoved = cameraPosition.x * (1 - parallaxEffect);
        var distanceToMove = cameraPosition.x * parallaxEffect;
        var transform1 = transform;
        transform1.position = new Vector3(_xPosition + distanceToMove, transform1.position.y);
        if(distanceMoved > _xPosition + _length) _xPosition += _length;
        else if(distanceMoved < _xPosition - _length) _xPosition -= _length;
    }
}
}

