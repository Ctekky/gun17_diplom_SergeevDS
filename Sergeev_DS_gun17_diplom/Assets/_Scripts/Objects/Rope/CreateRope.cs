﻿using UnityEngine;

public class CreateRope : MonoBehaviour
{
    #region Fields
    // Переменные для хранения объёктов веревки
    private Transform _links;
    public GameObject firstSegment;
    private GameObject _rope;
    private GameObject _newRope;

    private GameObject _emptyGameObjectPrefab;
    //Переменные для хранения первичной позиции и поворота
    private Vector3 _linkOneOriginalPosition;

    private Quaternion _linkOneOriginalRotation;
    //Новая верёвка и отступ при спавне
    private Vector2 _spawnLocation;

    private float _spawnLocationOffset;
    //Переменные для присоединения позиций
    private float _ropeAttachPosition;

    private float _segmentSize;
    //Это основные компоненты физики веревки
    private HingeJoint2D _hingeJoint2D;

    private DistanceJoint2D _distanceJoint2D;
    //Настройки для верёвки
    [Header("Rope Settings")]
    [Range(2, 50, order = 1)] [Tooltip("Количество сегментов")] [Delayed] 
    public int ropeSegments;
    public bool hideLastRope;
    public bool autoAssignRopeAttachPositions;
    [Min(2)]
    public int numberOfRopeAttachPositions;
    public float ropeAttachPositionOffset;
    public float ropeLinksMass = 50;
    public float ropeLinksGravityScale = 1;
    public bool enableRopeCollisions;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    public GameObject LastRope { get; private set; }
    #endregion
    void Awake()
    {
        _links = transform.GetChild(1);
        _linkOneOriginalPosition = firstSegment.transform.position;
        _linkOneOriginalRotation = firstSegment.transform.rotation;
        _emptyGameObjectPrefab = (GameObject)Resources.Load("Prefabs/Objects/Rope/AutoRopeAttachPosition");
        _distanceJoint2D = transform.GetChild(0).GetComponent<DistanceJoint2D>();
        _spawnLocationOffset = firstSegment.GetComponent<SpriteRenderer>().bounds.size.y / 2 - 0.01f;
        BuildRope();
        ConnectRope();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireCube(firstSegment.transform.position, new Vector2(0, firstSegment.GetComponent<SpriteRenderer>().bounds.size.y - ropeAttachPositionOffset));
    }

    private void BuildRope()
    {
        for (var i = 0; i < ropeSegments - 1; i++)
        {
            var additionalOffset = Mathf.Abs(_links.GetChild(i).GetComponent<RopeLinks>().transform.position.y -
                                   _links.GetChild(i).GetComponent<RopeLinks>().ConnectionPoint.position.y) - 0.01f;
            _spawnLocation = (Vector2)_links.GetChild(i).GetComponent<RopeLinks>().ConnectionPoint.position - new Vector2(0, additionalOffset);
            var checkGround = Physics2D.Raycast(_spawnLocation, Vector2.down, firstSegment.GetComponent<SpriteRenderer>().bounds.size.y * 2f, groundLayer);
            if (i < ropeSegments - 2 && !checkGround)
            {
                _rope = (GameObject)Resources.Load("Prefabs/Objects/Rope/AutoAssignRopeLink");
            }
            else
            {
                _rope = (GameObject)Resources.Load("Prefabs/Objects/Rope/AutoAssignLastRope");
                ropeSegments = i + 2;
                i = ropeSegments + 1;
                LastRope = _rope;
            }
            _newRope = Instantiate(_rope, _spawnLocation, Quaternion.identity,_links);
            _newRope.transform.localScale = firstSegment.transform.localScale;
            _newRope.GetComponent<SpriteRenderer>().sprite = firstSegment.GetComponent<SpriteRenderer>().sprite;
            _newRope.GetComponent<SpriteRenderer>().color =firstSegment.GetComponent<SpriteRenderer>().color;
        }
    }

    private void ConnectRope()
    {
        for (var i = 0; i < ropeSegments; i++)
        {
            _rope = _links.GetChild(i).gameObject;
            _hingeJoint2D = _rope.GetComponent<HingeJoint2D>();
            if (i < ropeSegments - 1)
            {
                _hingeJoint2D.connectedBody = _links.GetChild(i + 1).gameObject.GetComponent<Rigidbody2D>();
            }
            else
            {
                _distanceJoint2D.connectedBody = _rope.GetComponent<Rigidbody2D>();
            }
        }
    }
}
