using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour
{
    public SpringType springType = SpringType.Single;
    public float springForce = 500.0f;

    private float baseForce = 250.0f;

    private float CalculateForce(float mass)
    {
        float force = (baseForce * mass) + (springForce * mass);
        return force;
    }

    private void ApplyForce(GameObject theObject)
    {
        float force = CalculateForce(theObject.rigidbody2D.mass);

        theObject.rigidbody2D.AddForce(new Vector2(0, force));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Rigidbody2D>() && col.relativeVelocity.y > 0)
        {
            string tag = col.gameObject.tag;
            switch (springType)
            {
                case SpringType.Single:
                    if (tag == "Sketch" || tag == "Tracy")
                        ApplyForce(col.gameObject);
                    break;
                case SpringType.Team:
                    if (tag == "Team")
                        ApplyForce(col.gameObject);
                    break;
                case SpringType.SketchOnly:
                    if (tag == "Sketch")
                        ApplyForce(col.gameObject);
                    break;
                case SpringType.TracyOnly:
                    if (tag == "Tracy")
                        ApplyForce(col.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
