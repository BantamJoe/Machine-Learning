using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MachineLearning.SmartMovement
{
    public class Brain : MonoBehaviour
    {

        public int DNALength = 2; //the length of the DNA string
        public float timeAlive;
        public float timeWalking;
        public DNA dna;
        public GameObject eyes;
        bool seeGround = true;
        bool alive = true;

        //this ethan just going to follow the invisible bot
        public GameObject ethanPrefab;
        GameObject ethan;

        private void OnDestroy()
        {
            Destroy(ethan);
        }

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
            //forward
            //1 left
            //2 right
            dna = new DNA(DNALength, 3);
            timeAlive = 0;
            alive = true;
            ethan = Instantiate(ethanPrefab, this.transform.position, this.transform.rotation);
            ethan.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().target = this.transform;
            //one gene is what to do when see platform
            //the other is what to do when dont see platform
        }

        private void Update()
        {
            Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
            seeGround = false;

            RaycastHit hit;
            if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
            {
                if (hit.collider.tag == "platform")
                {
                    seeGround = true;
                }
            }

            timeAlive = PopulationManager.elapsed;

            //read DNA
            float turn = 0;
            float move = 0;

            if (seeGround)
            {
                //make v relative to character and always move forward
                if (dna.GetGene(0) == 0) move = 1;
                else if (dna.GetGene(0) == 1) turn = -90;
                else if (dna.GetGene(0) == 2) turn = 90;
            }
            else {
                if (dna.GetGene(1) == 0) move = 1;
                else if (dna.GetGene(1) == 1) turn = -90;
                else if (dna.GetGene(1) == 2) turn = 90;
            }

            this.transform.Translate(0, 0, move * 0.1f);
            this.transform.Rotate(0, turn, 0);

            //the ones who moves more, are fittest
            if (dna.GetGene(0) == 0 && alive)
            {
                timeWalking += Time.deltaTime;
            }
        }
    }
}