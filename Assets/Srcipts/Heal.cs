using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// This is a Heal Gameobject and it will heal Player when Player collide with it.
/// </summary>
public class Heal : MonoBehaviour
{
    public float rotationSpeed = 40f;
    public float healAmount = 40f;
    AudioSource healSound;
    public GameObject ppv; //apply processing layer when player get heal

    // Start is called before the first frame update
    void Start()
    {
        healSound = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotating this object on zxis
        this.transform.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Heal(healAmount);// call heal method in playerController class
            healSound.PlayOneShot(healSound.clip);// play heal sound clip
            ppv.GetComponent<PostProcessVolume>().enabled = true;// enable post processing to indicate player got heal
            StartCoroutine(DestroyHeal());//start coroutine function to destory this gameobject after 0.5s
        }
    }

    private IEnumerator DestroyHeal()
    {
        yield return new WaitForSeconds(0.5f);// wait for 0.5 seconds
        ppv.GetComponent<PostProcessVolume>().enabled = false;// disable post processing to indicate player heal finished
        Destroy(this.gameObject);
    }
}
