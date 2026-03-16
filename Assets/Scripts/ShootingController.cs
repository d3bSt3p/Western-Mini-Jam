using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform lineStartPoint;
    [SerializeField] private Transform armPivot;
    
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
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateCrosshairPosition();
        FaceArmToCrosshair();
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

    private void FaceArmToCrosshair()
    {
        Vector3 toCrosshair = crosshairPos - armPivot.position;
        float angle = Mathf.Atan2(toCrosshair.y, toCrosshair.x) * Mathf.Rad2Deg;
        armPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            RaycastHit2D hit = Physics2D.Raycast(crosshairPos,  Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("shootable"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            
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
