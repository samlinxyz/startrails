using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    Rigidbody rb;
    public bool shot = false;
    public Transform obstacles;

    private Vector3 startPosition;
    public Transform aim;

    public TrailRenderer transientTrail;

    public Vector3 hello;
    public GameManager game;
    public float velSensitivity;

    public Pointer arrowhead;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;

        transientTrail.emitting = false;
        transform.GetChild(0).GetComponent<TrailRenderer>().emitting = false;
    }

    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump") && !shot) { ShootBall(); }

        if (shot)
        {
            for (int i = 0; i < obstacles.childCount; i++)
            {
                Transform obstacle = obstacles.GetChild(i);
                float mass = obstacle.GetComponent<Rigidbody>().mass;
                Vector3 r =  obstacle.position - transform.position;
                rb.AddForce(mass * r.normalized / r.sqrMagnitude);
            }
        }

    }

    public void ShootBall()
    {
        transientTrail.emitting = true;
        transform.GetChild(transform.childCount-1).GetComponent<TrailRenderer>().emitting = true;

        rb.velocity = aim.position - transform.position;
        rb.velocity *= velSensitivity;
        shot = true;

        arrowhead.CreateGhost();
    }

    public void ReturnBall()
    {
        transform.position = startPosition;

        //  Instiantiate a new trail
        transform.GetChild(transform.childCount - 1).GetComponent<Trail>().NewTrail();
        if (transform.childCount > 2)
        {
            transform.GetChild(0).GetComponent<Trail>().DimAndDestroySelf();
        }
    }



    public void StopBall()
    {
        rb.velocity = Vector3.zero;
        transform.position = transform.position + Vector3.back; //  Temporary (?), for erasing the last bit of trail.
        shot = false;
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Finish")
        {
            StopBall();
            //  Animate the ball to do something
        }
        if (collider.transform.parent = obstacles) { RetryShot(); }
    }

    public void RetryShot()
    {
        //  Stop the trails from emitting
        //  Return ball to starting position and zero the velocity
        StartCoroutine(ReturnToStart());
    }

    IEnumerator ReturnToStart()
    {
        transientTrail.emitting = false;
        transform.GetChild(transform.childCount - 1).GetComponent<TrailRenderer>().emitting = false;

        StopBall();

        game.IncrementShotCount();

        yield return new WaitForSeconds(1);

        //  Return Ball to starting position and zero its velocity.
        ReturnBall();

    }


}
