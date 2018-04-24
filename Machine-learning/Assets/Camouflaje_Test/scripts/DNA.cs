using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MachineLearning.Camouflage
{
    public class DNA : MonoBehaviour
    {

        //gene for colour
        public float r;
        public float g;
        public float b;
        public float size;
        bool dead = false;
        public float timeToDie = 0f; //to record how much time live. The fittest live more.
        SpriteRenderer sRenderer;
        Collider2D sCollider;

        // Use this for initialization
        void Start()
        {
            sRenderer = GetComponent<SpriteRenderer>();
            sCollider = GetComponent<Collider2D>();
            sRenderer.color = new Color(r, g, b);
        }

        private void OnMouseDown()
        {
            dead = true;
            timeToDie = PopulationManager.elapsed;
            Debug.Log("Dead At:" + timeToDie);
            //we dont destroy them cause we need their data
            sRenderer.enabled = false;
            sCollider.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}