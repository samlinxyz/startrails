using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    public Transform ball;
    public Vector3 lastMousePosition;


    public bool aiming;

    public float sensitivity;
    public Vector3 deltaMousePosition;
    public Vector3 clickPosition;
    public float maxVelocity;

    public float maxArrowheadEdgeLength;
    public LineRenderer arrowhead;

    // Start is called before the first frame update
    void Start()
    {
        arrowhead = transform.GetChild(0).GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aiming && !ball.GetComponent<GolfBall>().shot)
        {

            
            deltaMousePosition = (Vector3)Input.GetTouch(0).position - clickPosition;

            //  Just put this into desmos and you'll see what I mean.
            Vector3 arrow = maxVelocity * sensitivity * deltaMousePosition / (sensitivity * deltaMousePosition.magnitude + maxVelocity);
            transform.position = arrow + ball.position;

            //  For drawing the initial velocity vector.

            //  //  Drawing the lines.
            float arrowheadEdgeLength = Mathf.Clamp(arrow.magnitude, 0f, 1f) * maxArrowheadEdgeLength;
            arrow = Mathf.Clamp(arrow.magnitude, 1f, Mathf.Infinity) * arrow.normalized;
            Vector3[] arrowVertices = new Vector3[3] { ball.position, ball.position, ball.position };
            for (int i = 0; i < arrowVertices.Length; i++) {
                arrowVertices[i] = ball.position + arrow; }
            arrowVertices[0] += arrowheadEdgeLength * Vector3.Cross(arrow.normalized, Vector3.back) - arrowheadEdgeLength * arrow.normalized;
            arrowVertices[2] += -arrowheadEdgeLength * Vector3.Cross(arrow.normalized, Vector3.back) - arrowheadEdgeLength * arrow.normalized;
            arrowhead.SetPositions(arrowVertices);

            //  //  Setting the alpha of the line.


        }

        if (Input.GetMouseButtonDown(0) && !ball.GetComponent<GolfBall>().shot)
        {
            aiming = true;
            clickPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            aiming = false;
            //  transform.position = 1000f * Vector3.back;
            arrowhead.SetPositions(new Vector3[] { ball.position + 1000f * Vector3.back, ball.position + 1000f * Vector3.back, ball.position + 1000f * Vector3.back });
        }

        
    }

    public void CreateGhost()
    {
        if (transform.childCount > 1)
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        Transform ghost = Instantiate(transform.GetChild(0), transform);

        Gradient newGradient = new Gradient();
        newGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.5f, 0f), new GradientAlphaKey(0.5f, 1f) }
            );

        ghost.GetComponent<LineRenderer>().colorGradient = newGradient;

    }

}
