using UnityEngine;

public class NoteMove : MonoBehaviour
{
    public int NoteSpeed = 5;
    // Update is called once per frame
    void Update()
    {
        transform.position -= transform.forward * Time.deltaTime * NoteSpeed;
    }
}
