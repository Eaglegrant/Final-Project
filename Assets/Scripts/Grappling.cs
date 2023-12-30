using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovementAdvanced pm;
    public Transform cam;
    public Transform gunTip;
    public Animator trigger;
    private Vector3 triggerStart;
    public LayerMask whatIsGrappleable;
    public Image reticle;
    public LineRenderer lr;
    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;
    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;
    private bool grappling;
    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovementAdvanced>();
    }
    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;
        if (pm.activeGrapple) return;
        grappling = true;
        pm.freeze = true;
        RaycastHit hit;
        if(Physics.Raycast(cam.position,cam.forward,out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
        trigger.SetBool("Fire", true);
        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }
    private void ExecuteGrapple()
    {
        pm.freeze = false;
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;
        if(grapplePointRelativeYPos < 0)
        {
            highestPointOnArc = overshootYAxis;
        }
        pm.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(StopGrapple), 1f);
    }
    public void StopGrapple()
    {
        trigger.SetBool("Reload", true);
        trigger.SetBool("Fire", false);
        grappling = false;
        grapplingCdTimer = grapplingCd;
        lr.enabled = false;
        pm.freeze = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(grappleKey)) StartGrapple();
        if (grapplingCdTimer > 0) grapplingCdTimer -= Time.deltaTime;
        else
        {
            trigger.SetBool("Reload",false);
        }
    }
    private void FixedUpdate()
    {
        if (Physics.Raycast(cam.position, cam.forward, maxGrappleDistance, whatIsGrappleable) && (grapplingCdTimer <=0) && (pm.activeGrapple == false))
        {
            reticle.color = Color.red;
        }
        else
        {
            reticle.color = Color.black;
        }
    }
    private void LateUpdate()
    {
        if (grappling)
        {
            lr.SetPosition(0, gunTip.position);
        }
    }
    private void OnDisable()
    {
        reticle.color = Color.black;
        trigger.SetBool("Reload", false);
        trigger.SetBool("Fire", false);
    }
}
