using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speedUpagrade = 2f;
    public float normalSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
    IEnumerator PowerUp()
    {
        speedUpagrade = 2f;
        yield return new WaitForSeconds(5f);
        speedUpagrade = 1f;
    }

}
