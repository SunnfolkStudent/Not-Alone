using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
 [HideInInspector] public bool jump;
 [HideInInspector] public bool longJump;
 [HideInInspector] public Vector2 moveVector;
 [HideInInspector] public bool run;
 [HideInInspector] public bool dash;
 
 void Update()
   {
  //Move
  moveVector.x = (Keyboard.current.aKey.isPressed ? -1f : 0f) + (Keyboard.current.dKey.isPressed ? 1f : 0f);
  moveVector.y = (Keyboard.current.sKey.isPressed ? 0f : -1f) + (Keyboard.current.wKey.isPressed ? 0f : 1f);

  //Run
  run = Keyboard.current.shiftKey.isPressed;
  
  //Jump
  jump = Keyboard.current.spaceKey.wasPressedThisFrame;
  longJump = Keyboard.current.spaceKey.isPressed;
  
  //Dash
  dash = Keyboard.current.altKey.wasPressedThisFrame;
  }
}