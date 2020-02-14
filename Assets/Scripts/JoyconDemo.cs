using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyconDemo : MonoBehaviour
{

    public JoyconManager.JoyconType joyconType;
    public float[] stick;
    public Vector3 gyro = Vector3.zero;
    public float gyroMagnitude;
    public Vector3 accel = Vector3.zero;
    public float accelMagnitude;
    public Quaternion orientation;
    public Vector3 rotation;
    public Joycon joycon;
    Vector3 rotationOffset = new Vector3(0, 180, 0);

    JoyconManager joyconManager;
    List<Joycon> joycons;

    void Start()
    {
        joyconManager = JoyconManager.Instance;
        joycon = joyconManager.GetJoycon(joyconType);

        // get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;
    }

    // Update is called once per frame
    void Update()
    {

        // make sure the Joycon only gets checked if attached
        //if (joycon != null)
        //{
            // GetButtonDown checks if a button has been pressed (not held)
            if (joycon.GetButtonDown(Joycon.Button.SHOULDER_1))
            {
                // Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
                joycon.Recenter();
            }

            stick = joycon.GetStick();

            // Gyro values: x, y, z axis values (in radians per second)
            gyro = joycon.GetGyro();
            gyroMagnitude = gyro.magnitude;

            // Accel values:  x, y, z axis values (in Gs)
            accel = joycon.GetAccel();
            accelMagnitude = accel.magnitude;

            // fix rotation
            orientation = joycon.GetVector();
            orientation = new Quaternion(orientation.x, orientation.z, orientation.y, orientation.w);
            Quaternion quat = Quaternion.Inverse(orientation);
            Vector3 rot = quat.eulerAngles;
            rot += rotationOffset;
            orientation = Quaternion.Euler(rot);
            rotation = orientation.eulerAngles;

            // rumble info
            // https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md
            // The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
            // (Useful for dynamically changing rumble values.)
            // Then call SetRumble(0,0,0) when you want to turn it off.
        //}
    }
}