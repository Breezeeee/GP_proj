using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TransformFollow : MonoBehaviour {

    public Transform followedObject;
    public Vector3 offset;

    [System.Serializable]
    public class FixPosition {
        public bool fixX;
        public bool fixY;
        public bool fixZ;
    }
    public FixPosition fixPosition;

    private Vector3 tmpPosition;

    void Start()
    {
        
    }

    void Update()
    {
        //// make the current object follow the followed object
        tmpPosition.x = fixPosition.fixX ? 0 : followedObject.position.x;
        tmpPosition.y = fixPosition.fixY ? 0 : followedObject.position.y;
        tmpPosition.z = fixPosition.fixZ ? 0 : followedObject.position.z;
        transform.position = tmpPosition + offset;

        transform.rotation = followedObject.rotation;
    }
}
