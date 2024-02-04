using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class VoiceManager : NetworkBehaviour
{
    public AudioSource audioSource; // For playing network audio
    public AudioSource microphoneSource; // For microphone input
    public int sampleRate = 32000;
    public GameObject voiceIcon;
    public string selectedMic;
    public Button micButton;
    public Text buttonText;
    private int currentMicIndex = 0;

    public float sendInterval = 0f;
    public float lastSendTime = 0;
    public int lastSamplePosition = 0;

    public bool ssss;
    private void FixedUpdate()
    {
        ssss = isOwned;
    }
    private AudioClip remoteVoiceClip;


    // void sss(){
    //     NetworkIdentity sss;
    //     sss.AssignClientAuthority()
    // }
    void Awake()
    {
        remoteVoiceClip = AudioClip.Create("RemoteVoice", sampleRate * 2, 1, sampleRate, false); // Set the stream argument to false
        audioSource.clip = remoteVoiceClip;
        audioSource.loop = true;
        audioSource.Play();
    }


    // Start is called before the first frame update
    void Start()
    {
        voiceIcon.SetActive(false);

        if (Microphone.devices.Length > 0)
        {
            selectedMic = selectedMic == "" ? Microphone.devices[0] : selectedMic;
            StartMicrophone();
        }

        // CmdAssignAuthority();

    }


    [Command(channel = 2)]
    void CmdAssignAuthority()
    {
        // Assign authority to the local player
        gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }



    bool IsSpeaking(float[] samples, float threshold = 0.01f)
    {
        foreach (float sample in samples)
        {
            if (Mathf.Abs(sample) > threshold)
            {
                return true;
            }
        }
        return false;
    }

    void UpdateMicButtonLabel()
    {
        if (Microphone.devices.Length > 0)
        {
            buttonText.text = "[C]   -   " + Microphone.devices[currentMicIndex];
        }
        else
        {
            buttonText.text = "[C]   -   " + "No Microphone Found";
        }
    }

    public void ChangeMicrophone()
    {
        currentMicIndex = (currentMicIndex + 1) % Microphone.devices.Length;
        selectedMic = Microphone.devices[currentMicIndex];
        StartMicrophone();
        UpdateMicButtonLabel();
    }


    void StartMicrophone()
    {
        microphoneSource.Stop();
        microphoneSource.clip = Microphone.Start(selectedMic, true, 5, sampleRate);
        while (!(Microphone.GetPosition(selectedMic) > 0)) { } // Wait until microphone starts
        microphoneSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = isOwned ? 0.0f : 1.0f;

        // if (!isOwned)
        // {
        //     return;
        // }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeMicrophone();
        }

        if (Microphone.devices.Length <= 0 || !Microphone.IsRecording(selectedMic))
        {
            return; // No microphone or not recording
        }

        int microphonePosition = Microphone.GetPosition(selectedMic);

        if (microphonePosition < lastSamplePosition)
        {
            lastSamplePosition = 0; // Handle looping
        }

        int sampleSize = microphonePosition - lastSamplePosition;

        if (sampleSize > 0)
        {
            float[] samples = new float[sampleSize * microphoneSource.clip.channels];
            microphoneSource.clip.GetData(samples, lastSamplePosition);
            if (IsSpeaking(samples))
            {
                Debug.Log(samples.Length);
                voiceIcon.SetActive(true);
                CmdSendData(samples);
                lastSendTime = Time.time;
            }

            else
            {
                voiceIcon.SetActive(false);
            }

            lastSamplePosition = microphonePosition;
        }
    }


    [Command(channel = 2)]
    void CmdSendData(float[] samples)
    {
        RpcPlaySound(samples);
    }

    [ClientRpc(includeOwner = false, channel = 2)]
    void RpcPlaySound(float[] samples)
    {
        if (isOwned) return; // Don't play own voice back
        Debug.Log("Calllll      " + samples.Length);
        AudioClip clip = AudioClip.Create("RemoteVoice", samples.Length, 1, sampleRate, false);
        clip.SetData(samples, 0);
        audioSource.clip = clip;
        audioSource.Play();
    }

}