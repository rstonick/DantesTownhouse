using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        GetComponent<Rigidbody>().AddForce(movement * speed);
    }
}