using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerAimWeapon : MonoBehaviour {

    // public event EventHandler<OnShootEventArgs> OnShoot;
    // public class OnShootEventArgs : EventArgs {
    //     public Vector3 gunEndPointPosition;
    //     public Vector3 shootPosition;
    //     public Vector3 shellPosition;
    // }

    private PlayerLookAt playerLookAt;
    private Transform aimTransform;
    public Transform aimGunEndPointTransform;
    private Transform aimShellPositionTransform;
    private Animator aimAnimator;
    private Player playerScript;
    

    [Header("Projectile")]
    [SerializeField] GameObject grapplePrefab;
    public float projectileSpeed = 10f;
    public Vector3 shootPosition;

    private void Awake() {
        playerLookAt = GetComponent<PlayerLookAt>();
        aimTransform = transform.Find("Aim");
        aimAnimator = aimTransform.GetComponent<Animator>();
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
        aimShellPositionTransform = aimTransform.Find("ShellPosition");
        playerScript = gameObject.GetComponent<Player>();
    }

    private void Update() {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming() {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - aimTransform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90) {
            aimLocalScale.y = -1f;
        } else {
            aimLocalScale.y = +1f;
        }
        aimTransform.localScale = aimLocalScale;

        // playerLookAt.SetLookAtPosition(mousePosition);
    }

    private void HandleShooting() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            aimAnimator.SetTrigger("Shoot");
            shootPosition = mousePosition;

            if (playerScript.has_projectile_item_grappled) {
                // shoot grappled item
                playerScript.shootItemFromHere(aimGunEndPointTransform);
            }
            else {  
                Debug.Log("no item equipped, shooting grapple");
                // shoot grapple
                // destroy previous grapple
                GameObject abortedGrapple = GameObject.FindWithTag("Grapple");
                if (abortedGrapple) { 
                    Destroy(abortedGrapple); 
                }

                // shoot grapple
                GameObject grapple = Instantiate(
                    grapplePrefab, 
                    aimGunEndPointTransform.position, 
                    Quaternion.identity) as GameObject;
            }  
        }
    }

}
