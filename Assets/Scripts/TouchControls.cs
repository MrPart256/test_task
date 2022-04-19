using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class TouchControls : MonoBehaviour
{
    Planet playerSelectedPlanet;
    Planet selectedPlanet;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
        Touch.onFingerMove += OnFingerMove;
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
        Touch.onFingerMove -= OnFingerMove;
    }
   
    private void OnFingerDown(Finger finger)
    {
       
        
        if (hitInfo().collider != null)
        {
           
            if (hitInfo().collider.gameObject.CompareTag("PlayerPlanet"))
            {
                playerSelectedPlanet = hitInfo().collider.GetComponent<Planet>();
                playerSelectedPlanet.SelectPlanet();
            }
        }
     
    }

    private void OnFingerUp(Finger finger)
    {  
        if (selectedPlanet != null && hitInfo().collider !=null)
        {
            playerSelectedPlanet.SendBattleships(selectedPlanet.transform);

            selectedPlanet = null;
        }
        if (playerSelectedPlanet != null)
        {
            playerSelectedPlanet.DeselectPlanet();
            playerSelectedPlanet = null;
        }

    }

    private void OnFingerMove(Finger finger)
    {

        if (hitInfo().collider != null)
        {

            if (hitInfo().collider.gameObject.CompareTag("NeutralPlanet") || hitInfo().collider.gameObject.CompareTag("PlayerPlanet"))
            {
                selectedPlanet = hitInfo().collider.GetComponent<Planet>();
               
            }
           
        }
    }

    private RaycastHit2D hitInfo()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Touch.activeFingers[0].currentTouch.screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Camera.main.transform.forward);
        return hit;
    }
   
}