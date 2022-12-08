using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public Rigidbody bulletPrefab;
    public GameObject cursor;
    public Transform shootPoint;
    public LayerMask maskLayer;

    public Transform bulletHolder;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        LaunchProjectile();
    }

    void LaunchProjectile()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100f, maskLayer))
        {
            cursor.SetActive(true);
            cursor.transform.position = hit.point+Vector3.up*0.1f;

            Vector3 Vo = CalculateVelocity(hit.point, shootPoint.position, 1f);

            bulletHolder.rotation = Quaternion.LookRotation(Vo);

            if (Input.GetMouseButtonDown(0))
            {
                Rigidbody obj = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                obj.velocity = Vo;
            }
        }
        else
        {
            cursor.SetActive(false);
        }
    }
    
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;


        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;
        
        float Vxz = Sxz / time;
        float Vy = Sy / time + .5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;


    }
    
}
