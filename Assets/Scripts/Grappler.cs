using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// linerenderer based on: https://youtu.be/P-UscoFwaE4

public class Grappler : MonoBehaviour
{

    public LineRenderer lineRenderer;
    private float speed;
    private Vector3 target;
    private PlayerAimWeapon playerAimWeapon;
    private GameObject player;
    private bool moveOut = true;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = true;
        player = GameObject.Find("Player");
        playerAimWeapon = player.GetComponent<PlayerAimWeapon>();
        target = playerAimWeapon.shootPosition;
        speed = playerAimWeapon.projectileSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        HandleLineRenderer();
    }

    void HandleLineRenderer() {
        lineRenderer.SetPosition(0, playerAimWeapon.aimGunEndPointTransform.position);
        lineRenderer.SetPosition(1, transform.position);
        //Debug.Log("gun endpoint position: " + playerAimWeapon.aimGunEndPointTransform.position);
        //Debug.Log("grapple position: " + transform.position);
    }

    void Move() {
        if (!moveOut) {
            target = playerAimWeapon.aimGunEndPointTransform.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        if (transform.position == target) {
            if (moveOut){
                target = playerAimWeapon.aimGunEndPointTransform.position;  // move back towards player
                moveOut = false;
            } else {
                Destroy(gameObject);
            }

            
        }
        else {      // move towards player

        }
        
    }
}
