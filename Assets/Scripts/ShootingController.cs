using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform lineStartPoint;
    
    [Header("Balancing")]
    [SerializeField] private float shootCooldown = 0.1f;
    [SerializeField] private float effectLength = 0.1f;
    private bool canShoot = true;
    
    private Camera mainCamera;
    private Vector3 crosshairPos;

    private void Start()
    {
        mainCamera = Camera.main;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        UpdateCrosshairPosition();
        HandleShooting();
    }

    private void UpdateCrosshairPosition()
    {
        Vector3 inputPos = Input.mousePosition;
        inputPos.z = mainCamera.nearClipPlane;
        crosshairPos = mainCamera.ScreenToWorldPoint(inputPos);
        
        crosshair.transform.localPosition = crosshairPos;
        lineRenderer.SetPosition(0, lineStartPoint.position);
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            // i felt like using coroutines to handle the timers idk
            StartCoroutine(ShotCooldown());
            StartCoroutine(ShootEffect());
        }
    }

    private IEnumerator ShotCooldown()
    {
        canShoot = false;
        crosshair.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(shootCooldown);
        crosshair.GetComponent<Renderer>().enabled = true;
        canShoot = true;
    }

    private IEnumerator ShootEffect()
    {
        // show shot
        lineRenderer.SetPosition(1, crosshairPos);
        lineRenderer.enabled = true;
        
        yield return new WaitForSeconds(effectLength);
        lineRenderer.enabled = false;
    }
}
