using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetsScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float torqueSpeed;
    [SerializeField] internal float xSpawnDirection;
    [SerializeField] private Vector3 whereThrow;
    [SerializeField] private float minY = 0.5f;
    [SerializeField] private float maxY = 0.8f;
    [SerializeField] private int hits = 1;
    [SerializeField] private ParticleSystem particleDestroy;

    private Rigidbody _rigidbody;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        whereThrow = new Vector3(xSpawnDirection, Random.Range(minY, maxY), 0);
        _rigidbody.AddForce(whereThrow * speed, ForceMode.Impulse);
        Vector3 torque = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        _rigidbody.AddTorque(torque * torqueSpeed, ForceMode.Impulse);
        Destroy(gameObject, 5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!gameObject.CompareTag("BadTarget"))
            {
                if (GameManager.Instance.life > 1)
                {
                    GameManager.Instance.life--;
                    PopupManager.Instance.ChangeLifeText();
                }
                else
                {
                    GameManager.Instance.LoseOrWinMenu();
                }
            }           
            Destroy(gameObject);            
        }
    }

    public void ReactToHit()
    {
        if (gameObject.CompareTag("BadTarget"))
        {
            if (GameManager.Instance.life > 1)
            {
                GameManager.Instance.life--;
                PopupManager.Instance.ChangeLifeText();
                Instantiate(particleDestroy, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
                return;
            }
            else
            {
                GameManager.Instance.LoseOrWinMenu();
            }         
        }
        hits--;
        PlayerHitsTarget();
        if (hits <=0)
        {
            Instantiate(particleDestroy, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
            GameManager.Instance.TakePoint();
        }
    }

 
    public void PlayerHitsTarget()
    {

        Renderer renderer = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>();
        Color currentColor = renderer.material.color;
        renderer.material.color = Color.black;
    }
}
