using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public static AudioClip powerup;
    static AudioSource audioSource;



    // Start is called before the first frame update
    void Start()
    {
        powerup = Resources.Load<AudioClip>("powerup");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        powerup = Resources.Load<AudioClip>("powerup");
        audioSource = GetComponent<AudioSource>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(powerup);
            gameObject.SetActive(false);
        }
    }
}
