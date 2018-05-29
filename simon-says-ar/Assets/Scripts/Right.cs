using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Right : MonoBehaviour {

    //public Vector3 source;
    //public Vector3 target;
    //public float overTime;

    //public IEnumerator MoveObject()
    //{
    //    float startTime = Time.time;
    //    while (Time.time < startTime + overTime)
    //    {
    //        transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
    //        yield return null;
    //    }
    //    transform.position = target;
    //}

    // animate the game object from -1 to +1 and back
    public float minimum = -1.0F;
    public float maximum = 1.0F;
    private int count = 0;

    // starting value for the Lerp
    static float t = 0.0f;

    void Update()
    {
        if (count < 2)
        {
            Debug.Log("Count is" + count);
            // animate the position of the game object...
            transform.position = new Vector3(Mathf.Lerp(minimum, maximum, t), 0, 0);

            // .. and increate the t interpolater
            t += 0.5f * Time.deltaTime;

            // now check if the interpolator has reached 1.0
            // and swap maximum and minimum so game object moves
            // in the opposite direction.
            if (t > 1.0f)
            {
                count += 1;
                float temp = maximum;
                maximum = minimum;
                minimum = temp;
                t = 0.0f;
            }
        }
    }
}
