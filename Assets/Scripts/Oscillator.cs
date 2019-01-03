using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

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
        if(period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period; //Grows continually from 0

        const float tau = Mathf.PI * 2; //About 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);
       
        moveFactor = rawSinWave / 2f + 0.5f;

        Vector3 displacement = moveFactor * movementVector;
        transform.position = startingPos + displacement;
    }
}
