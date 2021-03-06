using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileItem : MonoBehaviour
{

    
    private PlayerAimWeapon playerAimWeapon;
    private float speed;
    private Vector3 target;
    private GameObject player;
    public bool has_been_shot = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (has_been_shot){
            Move();
        }
    }

    private void Move() {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        if (transform.position == target) {
                Destroy(gameObject);
        }
    }

    public void MovementSetup() {
        player = GameObject.Find("Player");
        playerAimWeapon = player.GetComponent<PlayerAimWeapon>();
        target = playerAimWeapon.shootPosition;
        speed = playerAimWeapon.projectileSpeed;
    }
}
