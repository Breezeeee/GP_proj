using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairNode
{
    public Vector3 prePos, curPos;
    public Transform pointTrans;
}

public class HairPoint : MonoBehaviour
{
    public float length = 0.75f;
    public float g = 9.8f;
    public float m = 0.1f;
    public float deltaT = 0.1f;
    public float D = 0.95f;
    public Vector3 Force = new Vector3(0, 0, 0);
    private float timer = 0.0f;

    public Vector3 rootPos;
    public List<HairNode> hairNodes;

    private Transform Hair;
    private Transform Head;
    private LineRenderer selfLinerender;

    void Verlet(HairNode hairNode)
    {
        timer += Time.deltaTime;
        Vector3 at = new Vector3(0, -g, 0);
        at += Force * Mathf.Abs(Mathf.Sin(10 * timer * Mathf.Deg2Rad));

        if (timer >= 360.0f)
        {
            timer -= 360.0f;
        }

        Vector3 nextPos = hairNode.curPos + D * (hairNode.curPos - hairNode.prePos) +
            at * Mathf.Pow(deltaT, 2);

        hairNode.prePos = hairNode.curPos;
        hairNode.curPos = nextPos;
    }

    void collideSphere(HairNode hairNode)
    {
        float distance = Vector3.Distance(hairNode.curPos, Head.position);
        if (distance < 1.15)
        {
            double newX = 1.15 / distance * (hairNode.curPos.x - Head.position.x) + Head.position.x;
            double newY = 1.15 / distance * (hairNode.curPos.y - Head.position.y) + Head.position.y;
            double newZ = 1.15 / distance * (hairNode.curPos.z - Head.position.z) + Head.position.z;

            Vector3 newPos = new Vector3((float)newX, (float)newY, (float)newZ);
            hairNode.curPos = newPos;
        }
    }

    void lengthConstraint(HairNode hairNode1, HairNode hairNode2, int index)
    {
        float distance = Vector3.Distance(hairNode1.curPos, hairNode2.curPos);
        Vector3 newPos1 = hairNode1.curPos + (hairNode2.curPos - hairNode1.curPos) * 
            (distance - Mathf.Pow(length, (hairNodes.Count - 1 - index))) / (2 * distance);
        Vector3 newPos2 = hairNode2.curPos - (hairNode2.curPos - hairNode1.curPos) * 
            (distance - Mathf.Pow(length, (hairNodes.Count - 1 - index))) / (2 * distance);

        hairNode1.curPos = newPos1;
        hairNode2.curPos = newPos2;
    }

    void ChangeForce()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Force.y += 0.2f;
            Force.y = Force.y > 1.0f ? 1.0f : Force.y;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Force.y -= 0.2f;
            Force.y = Force.y < -1.0f ? -1.0f : Force.y;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Force.x += 0.2f;
            Force.x = Force.x > 1.0f ? 1.0f : Force.x;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Force.x -= 0.2f;
            Force.x = Force.x < -1.0f ? -1.0f : Force.x;
        }
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            Force.y += 0.2f;
            Force.y = Force.y > 1.0f ? 1.0f : Force.y;
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            Force.y -= 0.2f;
            Force.y = Force.y < -1.0f ? -1.0f : Force.y;
        }
    }

    void Start()
    {
        Hair = this.GetComponent<Transform>();
        Head = GameObject.FindGameObjectWithTag("Head").GetComponent<Transform>();
        hairNodes = new List<HairNode>();
        for (int i = 0; i < transform.childCount; i++)
        {
            HairNode hairNode = new HairNode();
            hairNode.pointTrans = transform.GetChild(i);
            hairNode.curPos = transform.GetChild(i).position;
            hairNode.prePos = transform.GetChild(i).position;
            hairNodes.Add(hairNode);
        }
        rootPos = hairNodes[0].curPos;
        selfLinerender = this.GetComponent<LineRenderer>();
        //Debug.Log(hairNodes.Count);
        for (int i = 0; i < hairNodes.Count; i++)
        {
            selfLinerender.SetPosition(i, hairNodes[i].curPos);
        }
        //Debug.Log(selfLinerender.positionCount);
    }

    void Update()
    {
        ChangeForce();

        for (int i = 0; i < hairNodes.Count; i++)
        {
            Verlet(hairNodes[i]);
        }
        for (int i = 1; i < hairNodes.Count; i++)
        {
            collideSphere(hairNodes[i]);
        }
        for (int i = 0; i < hairNodes.Count - 1; i++)
        {
            lengthConstraint(hairNodes[i], hairNodes[i + 1], i);
        }
        hairNodes[0].curPos = rootPos;
        hairNodes[0].prePos = rootPos;

        for (int i = 0; i < hairNodes.Count; i++)
        {
            selfLinerender.SetPosition(i, hairNodes[i].curPos);
            hairNodes[i].pointTrans.position = hairNodes[i].curPos;
        }
    }
}
