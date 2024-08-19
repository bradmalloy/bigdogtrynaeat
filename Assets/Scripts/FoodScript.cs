using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class FoodScript : MonoBehaviour {
    [SerializeField] public int score;

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log(this.gameObject.name + " was touched, destroying");
            Destroy(gameObject);
        }
        

    }


}
