using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO.Ports;
using System;

// Based on the guide https://www.alanzucconi.com/2015/10/07/how-to-integrate-arduino-with-unity/

public class Serial : MonoBehaviour {

    [Tooltip("Constantly read data from serial port")]
    public bool ReadFromSerialLoop = true;
    private bool reading = false;
    [Tooltip("Amount of time to read from serial port")]
    public float ReadMS = 100f;

    public string SerialReadString;
    public float SerialReadFloat = 0f;

    /* The serial port where the Arduino is connected. */
    [Tooltip("The serial port where the Arduino is connected")]
    public string port = "COM4";
    /* The baudrate of the serial port. */
    [Tooltip("The baudrate of the serial port")]
    public int baudrate = 9600;

    SerialPort stream;

	// Use this for initialization
	void Start () {
        stream = new SerialPort(port, baudrate);
        stream.ReadTimeout = 50;
        stream.Open();
    }

    public void WriteToArduino(string message)
    {
        Debug.Log("Writing serial message " + message);
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

    public void ArduinoPing()
    {
        WriteToArduino("PING");
        ArduinoReadAsync();
    }

    public void ArduinoEcho()
    {
        WriteToArduino("ECHO");
        ArduinoReadAsync();
    }

    public void ArduinoReadAsync(float timeout = 10000f)
    {
        StartCoroutine(AsynchronousReadFromArduino
            ((string s) => SerialReadString = s,
            () => Debug.Log("Serial error"), // "Error!"),     // Error callback
            timeout                             // Timeout (milliseconds)
            ));
    }

    private void Update()
    {
        if (ReadFromSerialLoop && !reading)
        {
            // Debug.Log("Starting new read loop");
            ArduinoReadAsync(ReadMS);
        }
        if(SerialReadString.Length > 0) float.TryParse(SerialReadString.Substring(0, SerialReadString.LastIndexOf("cm")), out SerialReadFloat);
    }

    public string ReadFromArduino(int timeout = 0)
    {
        stream.ReadTimeout = timeout;
        try
        {
            return stream.ReadLine();
        }
        catch (TimeoutException e)
        {
            return null;
        }
    }

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        reading = true;

        do
        {
            try
            {
                // Debug.Log("Trying to read from Arduino");
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                // Debug.Log("Serial from Arduino timed out");
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                // Debug.Log("Received dataString " + dataString);
                reading = false;
                yield break; // Terminates the Coroutine
            }
            else
                yield return null; // Wait for next frame

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        reading = false;

        if (fail != null)
            fail();
        yield return null;
    }

    private void OnDisable()
    {
        Close();
    }

    public void Close()
    {
        stream.Close();
    }
}
