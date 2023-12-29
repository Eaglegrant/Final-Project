using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Variables
    public CharacterController controller;
    public Animator anim;
    public AudioClip RunningSound;
    AudioSource audioSource;

    public float speed = 4;
    public float rotSpeed = 90;
    public float rot = 0f;
    public float gravity = 8;
    public bool running = false;

    public Vector3 moveDir = Vector3.zero;

    // Starting Function
    void Start()
    {
        controller = GetComponent<CharacterController> ();
        anim = GetComponent<Animator> ();
        audioSource = GetComponent<AudioSource>();
    }

    // Game Loop
    // Update is called once per frame
    void Update()
    {

        // When player presses Up Arrow, character moves forward.
        // When player presses Right or Left Arrow keys, character rotates direction.
        if (controller.isGrounded){
            if (Input.GetKey(KeyCode.W)){
                anim.SetInteger("condition", 1);
                running = true;
                moveDir = new Vector3 (0, 0, 1);
                moveDir *= speed;
                moveDir = transform.TransformDirection(moveDir);
                if(audioSource != null){
                    audioSource.clip = RunningSound;
                    audioSource.Play();
                }
            } else {
                anim.SetInteger("condition", 0);
                moveDir = new Vector3 (0, 0, 0);
                running = false;
                if(audioSource != null && audioSource.isPlaying){
                    audioSource.Stop();
                }
            }

            if (Input.GetKey(KeyCode.Space)){
                anim.SetInteger("jump", 1);
                if (running){
                    moveDir = new Vector3 (0, 2, 1);
                } else {
                    moveDir = new Vector3 (0, 1, 0);
                }
                moveDir *= speed;
                moveDir = transform.TransformDirection(moveDir);
            } else {
                anim.SetInteger("jump", 0);
            }

            if (Input.GetKey(KeyCode.D)){
                rot += rotSpeed * Time.deltaTime;
                transform.eulerAngles = new Vector3 (0, rot, 0);
            }

            if (Input.GetKey(KeyCode.A)){
                rot -= rotSpeed * Time.deltaTime;
                transform.eulerAngles = new Vector3 (0, rot, 0);
            }
        }

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
    }
}
