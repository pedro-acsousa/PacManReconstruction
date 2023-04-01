using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{

    public static AudioClip coin;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        coin = Resources.Load<AudioClip>("coin");
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        coin = Resources.Load<AudioClip>("coin");
        audioSrc = GetComponent<AudioSource>();

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSrc.PlayOneShot(coin);
            gameObject.SetActive(false);
        }
    }
}
