using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCar : MonoBehaviour {

    public float Distance = 0f;
    public float Scale = 17.8885630499f;
    public bool allow0 = true;

    private Serial serial;

    private void Start()
    {
        serial = FindObjectOfType<Serial>();
    }

    // Update is called once per frame
    void Update () {
        if (allow0 || serial.SerialReadFloat > 0f) transform.position = new Vector3(0f, 0f, serial.SerialReadFloat * Scale);
	}
}
