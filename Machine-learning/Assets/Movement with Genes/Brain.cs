using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace MachineLearning.Movement
{
    /// <summary>
    /// A Brain sits between a character and his DNA. Read DNA and decide what to do.
    /// </summary>
    /// 
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class Brain : MonoBehaviour {

        public int DNALength = 1; //the length of the DNA string
        public float timeAlive;
        //challenge
        public float distance;
        Vector3 originalPosition;
        public DNA dna;
        private ThirdPersonCharacter m_Character;
        private Vector3 m_Move;
        private bool m_Jump;
        bool alive = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "dead")
            {
                alive = false;
                
            }
        }

        public void Init()
        {
            //initialise DNA
            //0 forward
            //1 back
            //2 left
            //3 right
            //4 jump
            //5 crouch
            dna = new DNA(DNALength, 6);
            m_Character = GetComponent<ThirdPersonCharacter>();
            timeAlive = 0;
            alive = true;
            originalPosition = transform.position;
        }

        private void FixedUpdate()
        {
            //read DNA
            float h = 0;
            float v = 0;
            bool crouch = false;

            int gene = dna.GetGene(0);

            switch (gene) {
                case 0: v = 1; break;
                case 1: v = -1; break;
                case 2: h = -1; break;
                case 3: h = 1; break;
                case 4: m_Jump = true; break;
                case 5: crouch = true; break;
            }

            m_Move = v * Vector3.forward + h * Vector3.right;
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
            if (alive)
            {
                timeAlive += Time.deltaTime;
                distance = Vector3.Distance(originalPosition, transform.position);
            }
        }
    }
}