// using Mirror;
// using UnityEngine;
// using UnityEngine.Audio;

// public class VoiceManager2 : NetworkBehaviour
// {
//     [Header("Settings")]
//     public AudioMixerGroup audioMixerGroup; // Set this in the Unity Editor

//     // This command is called on the server to broadcast the player's voice to others
//     [Command]
//     private void CmdSendVoiceData(float[] audioData)
//     {
//         RpcReceiveVoiceData(audioData);
//     }

//     // This RPC is called on all clients to receive voice data and play it
//     [ClientRpc]
//     private void RpcReceiveVoiceData(float[] audioData)
//     {
//         if (!isLocalPlayer)
//         {
//             // If this is not the local player, play the received voice data
//             PlayVoiceData(audioData);
//         }
//     }

//     // Play the received voice data
//     private void PlayVoiceData(float[] audioData)
//     {
//         // Create an AudioClip from the received audio data
//         // AudioClip receivedAudio = WavUtility.ToAudioClip(audioData, 0, audioData.Length, 0, 16, false);

//         // // Play the audio using an AudioSource (you may need to create an AudioSource component in your GameObject)
//         // AudioSource audioSource = GetComponent<AudioSource>();
//         // if (audioSource != null)
//         // {
//         //     audioSource.outputAudioMixerGroup = audioMixerGroup;
//         //     audioSource.clip = receivedAudio;
//         //     audioSource.Play();
//         // }
//     }

//     // Use this method to send the player's voice data to the server
//     private void SendVoiceData(float[] audioData)
//     {
//         if (isLocalPlayer)
//         {
//             CmdSendVoiceData(audioData);
//         }
//     }

//     // You need to call this method whenever you have new audio data to send
//     private void UpdateVoice()
//     {
//             float[] samples = new float[sampleSize * microphoneSource.clip.channels];
//             microphoneSource.clip.GetData(samples, lastSamplePosition);
//         float[] audioData = GetAudioData(); // Implement this method to get audio data from your microphone or other sources
//         SendVoiceData(audioData);
//     }

//     private void Update()
//     {
//         // Call this method in your update loop to update voice data
//         UpdateVoice();
//     }
// }
