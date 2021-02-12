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

            // destroy previous grapple
            GameObject abortedGrapple = GameObject.FindWithTag("Grapple");
            if (abortedGrapple) { 
                Destroy(abortedGrapple); 
            }

            // shoot grapple
            shootPosition = mousePosition;
            GameObject grapple = Instantiate(
                grapplePrefab, 
                aimGunEndPointTransform.position, 
                Quaternion.identity) as GameObject;
            // grapple.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            // previous line will just shoot straight up: needs to shoot towards mouse
            
            
            // original code
            //OnShoot?.Invoke(this, new OnShootEventArgs { 
            //     gunEndPointPosition = aimGunEndPointTransform.position,
            //     shootPosition = mousePosition,
            //     shellPosition = aimShellPositionTransform.position,
            //});
        }
    }

}
