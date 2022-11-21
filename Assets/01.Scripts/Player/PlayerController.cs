﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SD
{

    public class PlayerController : MonoBehaviour
    {

        [Header("Player Setting")]
        public float turnSpeed = 10f;
        public float walkSpeed = 3f;
        private float spd = 3f;
        public float runSpeed = 5f;
        public bool stopMoverment = false;

        public bool moving { get; set; }

        float m_Horizontal, m_Vertical;
        private Vector3 m_MoveVector;
        private Rigidbody m_Rigidbody;

        [HideInInspector]
        public Animator m_Animator;
        private Quaternion m_Rotation = Quaternion.identity;
        private Transform camTrans;
        private Vector3 camForward;

        private Vector3 offset;
        private float inputSpeed = 0f;

        private bool isSnow = false;
        private GameObject snowObj = null;

        void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Animator = GetComponent<Animator>();
            camTrans = Camera.main.transform;
        }


        void FixedUpdate()
        {

            //input 
            m_Horizontal = Input.GetAxis(Const.Horizontal);
            m_Vertical = Input.GetAxis(Const.Vetical);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                spd = runSpeed;
                inputSpeed = Mathf.Lerp(inputSpeed, 1, Time.deltaTime);
            }
            else
            {
                spd = walkSpeed;
                inputSpeed = (moving) ? 0.5f * Mathf.Clamp01(Mathf.Abs(m_Horizontal) + Mathf.Abs(m_Vertical)) : Mathf.Lerp(inputSpeed, 0, Time.deltaTime);
            }

            if (Input.GetMouseButtonDown(0))
            {
                stopMoverment = true;
                m_Animator.SetTrigger(Const.Dig);
            }

            // move vector 
            if (camTrans != null)
            {
                camForward = Vector3.Scale(camTrans.forward, new Vector3(1, 0, 1).normalized);
                m_MoveVector = m_Vertical * camForward + m_Horizontal * camTrans.right;
                m_MoveVector.Normalize();
            }
            //animation    


            bool has_H_Input = !Mathf.Approximately(m_Horizontal, 0);
            bool has_V_Input = !Mathf.Approximately(m_Vertical, 0);

            if (!stopMoverment) moving = has_H_Input || has_V_Input;
            else moving = false;

            m_Animator.SetBool(Const.Moving, moving);
            m_Animator.SetFloat(Const.Speed, inputSpeed);

            //move and rotate
            if (moving)
            {
                Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_MoveVector, turnSpeed * Time.deltaTime, 0f);
                m_Rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredForward), turnSpeed);
                m_Rigidbody.MoveRotation(m_Rotation);
                m_Rigidbody.MovePosition(m_Rigidbody.position + inputSpeed * m_MoveVector * spd * Time.deltaTime);
            }

        }

        public void ableMove()
        {
            stopMoverment = false;
            if(snowObj != null) snowObj.SetActive(false);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Snow"))
            {
                isSnow = true;
                snowObj = collision.gameObject;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.CompareTag("Snow"))
            {
                isSnow = false;
                snowObj = null;
            }
        }
    }
}

