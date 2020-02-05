using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Dweiss;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer), typeof(Rigidbody))]
public class DrawLine : MonoBehaviour {
    private LineRenderer _lr;
    private Rigidbody _rb;

    public float timeBeteenStep = 1;
    public int stepCount = 30;
    
    public Vector3 addedV, addedF;

    public KeyCode keyToActivate = KeyCode.Space;

    void Start() {
        _lr = GetComponent<LineRenderer>();
        _rb = GetComponent<Rigidbody>();
        Debug.Log("Press " + keyToActivate + " to shooot ");
    }

    private void AddPower()
    {
        Debug.LogWarning("Change V: " + addedV + " F: " + addedF);
        CalcTime();

        _rb.velocity += addedV;


        addedV = Vector3.zero;
        _rb.AddForce(addedF, ForceMode.Force);
        addedF = Vector3.zero;


    }


    private void CalcTime()
    {

        Vector3[] t = _rb.CalculateTime(new Vector3(0, 0, 0),
            addedV, addedF);

        var timeT = new Vector3[]{
            new Vector3(Time.time + t[0].x, Time.time + t[0].y, Time.time + t[0].z),
            new Vector3(Time.time + t[1].x, Time.time + t[1].y, Time.time + t[1].z)
        };
        Debug.LogWarning(Time.time + ": assuming no drag touch in (0,0,0) occures in those 2 time stamps:  " + timeT[0] + ", " + timeT[1]);
    }

    private void DrawMovementLine()
    {
        var res = _rb.CalculateMovement(stepCount, timeBeteenStep, addedV, addedF);

        _lr.positionCount = stepCount + 1;
        _lr.SetPosition(0, transform.position);
        for (int i = 0; i < res.Length; ++i)
        {
            _lr.SetPosition(i+1, res[i]);
        }

    }
	void Update () {

        DrawMovementLine();

        if (Input.GetKeyDown(keyToActivate))
        {
            AddPower();
        }
    }
}
