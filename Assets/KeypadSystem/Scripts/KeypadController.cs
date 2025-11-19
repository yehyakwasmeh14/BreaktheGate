using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeypadController : MonoBehaviour
{
    [Header("Door Settings")]
    public DoorController door;
    public string password = "1234";
    public int passwordLimit = 4;

    [Header("UI")]
    public Text passwordText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    void Start()
    {
        if (passwordText != null)
        {
            passwordText.text = "";
        }
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 0f;
            }
        }
    }

    public void PasswordEntry(string number)
    {
        if (number == "Clear")
        {
            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
            Clear();
            return;
        }
        else if (number == "Enter")
        {
            Enter();
            return;
        }

        int length = passwordText.text.ToString().Length;
        if (length < passwordLimit)
        {
            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
            passwordText.text = passwordText.text + number;
        }
    }

    public void Clear()
    {
        if (passwordText != null)
        {
            passwordText.text = "";
            passwordText.color = Color.white;
        }
    }

    void Enter()
    {
        if (passwordText.text == password)
        {
            if (door != null)
            {
                door.lockedByPassword = false;
            }

            if (audioSource != null && correctSound != null)
            {
                audioSource.PlayOneShot(correctSound);
            }

            if (passwordText != null)
            {
                passwordText.color = Color.green;
            }

            if (door != null)
            {
                door.OpenClose();
            }

            StartCoroutine(WaitAndClear());
        }
        else
        {
            if (audioSource != null && wrongSound != null)
            {
                audioSource.PlayOneShot(wrongSound);
            }

            if (passwordText != null)
            {
                passwordText.color = Color.red;
            }

            StartCoroutine(WaitAndClear());
        }
    }

    IEnumerator WaitAndClear()
    {
        yield return new WaitForSeconds(0.75f);
        Clear();
    }
}
