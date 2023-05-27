using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRope : MonoBehaviour
{
    #region Fields

    // Переменные для хранения объёктов веревки
    Transform links;
    public GameObject firstSegment;
    GameObject rope;
    GameObject newRope;
    GameObject emptyGameObjectPrefab;
    //Переменные для хранения первичной позиции и поворота
    Vector3 linkOneOriginalPosition;
    Quaternion linkOneOriginalRotation;
    //Новая верёвка и отступ при спавне
    Vector2 spawnLocation;
    float spawnLocationOffset;
    //Переменные для присоединения позиций
    float ropeAttachPosition;
    float segmentSize;
    //Это основные компоненты физики веревки
    HingeJoint2D hingeJoint2D;
    DistanceJoint2D distanceJoint2D;
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
        links = transform.GetChild(1);
        linkOneOriginalPosition = firstSegment.transform.position;
        linkOneOriginalRotation = firstSegment.transform.rotation;
        emptyGameObjectPrefab = (GameObject)Resources.Load("Prefabs/Objects/Rope/AutoRopeAttachPosition");
        distanceJoint2D = transform.GetChild(0).GetComponent<DistanceJoint2D>();
        spawnLocationOffset = firstSegment.GetComponent<SpriteRenderer>().bounds.size.y - 0.01f;
        BuildRope();
        ConnectRope();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireCube(firstSegment.transform.position, new Vector2(0, firstSegment.GetComponent<SpriteRenderer>().bounds.size.y - ropeAttachPositionOffset));
    }
    void BuildRope()
    {
        AssignRopePositions(firstSegment.gameObject);
        for (int i = 0; i < ropeSegments - 1; i++)
        {
            spawnLocation = (Vector2)links.GetChild(i).transform.position - new Vector2(0, spawnLocationOffset);
            var checkGround = Physics2D.Raycast(spawnLocation, Vector2.down, firstSegment.GetComponent<SpriteRenderer>().bounds.size.y + 1f, groundLayer);
            if (i < ropeSegments - 2 && !checkGround)
            {
                rope = (GameObject)Resources.Load("Prefabs/Objects/Rope/AutoAssignRopeLink");
            }
            else
            {
                rope = (GameObject)Resources.Load("Prefabs/Objects/Rope/AutoAssignLastRope");
                ropeSegments = i + 2;
                i = ropeSegments + 1;
                LastRope = rope;
            }
            newRope = Instantiate(rope, spawnLocation, Quaternion.identity,links);
            newRope.transform.localScale = firstSegment.transform.localScale;
            newRope.GetComponent<SpriteRenderer>().sprite = firstSegment.GetComponent<SpriteRenderer>().sprite;
            newRope.GetComponent<SpriteRenderer>().color =firstSegment.GetComponent<SpriteRenderer>().color;
            AssignRopePositions(newRope);
        }
    }
    void ConnectRope()
    {
        for (int i = 0; i < ropeSegments; i++)
        {
            rope = links.GetChild(i).gameObject;
            hingeJoint2D = rope.GetComponent<HingeJoint2D>();
            if (i < ropeSegments - 1)
            {
                hingeJoint2D.connectedBody = links.GetChild(i + 1).gameObject.GetComponent<Rigidbody2D>();
            }
            else
            {
                //hingeJoint2D.connectedBody = player.GetComponent<Rigidbody2D>();
                distanceJoint2D.connectedBody = rope.GetComponent<Rigidbody2D>();
            }
        }
    }
    void AssignRopePositions(GameObject newRope)
    {      
        ropeAttachPosition = firstSegment.GetComponent<SpriteRenderer>().bounds.extents.y - ropeAttachPositionOffset / 2;
        segmentSize =firstSegment.GetComponent<SpriteRenderer>().bounds.size.y - ropeAttachPositionOffset;
        for (int i = 0; i < numberOfRopeAttachPositions; i++)
        {
            Instantiate(emptyGameObjectPrefab, new Vector2(newRope.transform.position.x,
            newRope.transform.position.y + ropeAttachPosition), Quaternion.identity, newRope.transform);
            ropeAttachPosition -= segmentSize / (numberOfRopeAttachPositions - 1);
        }
    }
}
