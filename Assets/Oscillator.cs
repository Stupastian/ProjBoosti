using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;

    [Range(0,1)]
    [SerializeField]
    float moveFactor; //0 for not moved 1 if moved

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //set movementfactor automatically

        Vector3 displacement = moveFactor * movementVector;
        transform.position = startingPos + displacement;
    }
}
