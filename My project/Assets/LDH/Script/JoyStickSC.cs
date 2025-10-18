using UnityEngine;

public class JoyStickSC : MonoBehaviour
{
    public float speed = 5;
    public DynamicJoystick dynamicJoystick;
   
    public PlayerMove playermove;
    public void Update()
    {

        float h = dynamicJoystick.Horizontal;
        float v = dynamicJoystick.Vertical;
        playermove.MoveByJoystick(h, v);
    }

}
