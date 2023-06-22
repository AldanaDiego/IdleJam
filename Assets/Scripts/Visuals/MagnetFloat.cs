using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetFloat : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _rotateSpeed;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        _rotateSpeed = new Vector3(Random.Range(0, 25), Random.Range(0, 25), Random.Range(0, 25));
    }

    private void Update()
    {
        _transform.Rotate(_rotateSpeed * Time.deltaTime);
    }
}
