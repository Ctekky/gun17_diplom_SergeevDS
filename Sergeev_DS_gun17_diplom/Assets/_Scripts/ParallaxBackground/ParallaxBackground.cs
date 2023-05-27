using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Metroidvania.Common
{
public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private float parallaxEffect;
    private float xPositon;
    private float length;

    private void Start()
    {
        xPositon = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    private void Update() {
        float distanceMoved = playerCamera.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = playerCamera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPositon + distanceToMove, transform.position.y);
        if(distanceMoved > xPositon + length) xPositon += length;
        else if(distanceMoved < xPositon - length) xPositon -= length;
    }
}
}

