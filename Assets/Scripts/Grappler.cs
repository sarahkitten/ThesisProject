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
    private Player playerScript;
    private GameObject player;
    private bool moveOut = true;
    private bool collided = false;
    private Collider2D grappledObjCollider;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = true;
        player = GameObject.Find("Player");
        playerAimWeapon = player.GetComponent<PlayerAimWeapon>();
        playerScript = player.GetComponent<Player>();
        target = playerAimWeapon.shootPosition;
        speed = playerAimWeapon.projectileSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        PlayerMove();
        HandleLineRenderer();
    }

    void HandleLineRenderer() {
        lineRenderer.SetPosition(0, playerAimWeapon.aimGunEndPointTransform.position);
        lineRenderer.SetPosition(1, transform.position);
        //Debug.Log("gun endpoint position: " + playerAimWeapon.aimGunEndPointTransform.position);
        //Debug.Log("grapple position: " + transform.position);
    }

    void Move() {
        if (!collided) {
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
        }
    }

    void PlayerMove() {
        if (collided) {
            player.transform.position = Vector3.MoveTowards(player.transform.position, target, Time.deltaTime * speed);
            if (transform.position == target) {
                Debug.Log("Grapple destroyed because player reached target");
                playerScript.set_collision_as_equipped_projectile(grappledObjCollider.gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Grapplable") {
            grappledObjCollider = collision;
            Debug.Log("Grapple collision registered with " + collision.tag);
            collided = true;
            target = collision.transform.position;
            // Here if the object can be used as a projectile it should revolve around the player
            // first refactor the code and make sure it still works
            
        }
        else if (collision.tag == "Player") {
            if (collided) {
                Debug.Log("Grapple destroyed because player collided with grapple");
                playerScript.set_collision_as_equipped_projectile(grappledObjCollider.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
