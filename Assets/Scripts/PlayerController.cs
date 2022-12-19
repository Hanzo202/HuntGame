using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject gun;
    [SerializeField] private bool isRecharge;
    [SerializeField] private float rechargeTime;
    [SerializeField] private ParticleSystem shotParticle;


    private void Start()
    {
        isRecharge = false;
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.GameIsWorking)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                gun.transform.LookAt(hit.point);
            }
            if (Input.GetMouseButtonDown(0) && !isRecharge)
            {
                HitObject(hit);
            }
        }     
    }

    private void HitObject(RaycastHit hit)
    {
        shotParticle.Play();
        AudioManager.Instance.ShotSound();
        StartCoroutine(Recharge());
        GameObject hitObject = hit.transform.gameObject;
        TargetsScript target = hitObject.GetComponent<TargetsScript>();
        if (target != null)
        {
            target.ReactToHit();
        }
    }

    IEnumerator Recharge()
    {
        isRecharge = true;
        yield return new WaitForSeconds(rechargeTime);
        isRecharge = false;
    }
}
