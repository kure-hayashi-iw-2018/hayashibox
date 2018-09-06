using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;

public class SampleSpeechToText : MonoBehaviour {

    [SerializeField]
    private AudioClip m_AudioClip = new AudioClip();
    private SpeechToText m_SpeechToText = new SpeechToText();

    IEnumerator Start()
    {
        // 音声をマイクから3秒間取得する
        Debug.Log("Start record");
        var audioSources = GetComponent<AudioSource>();
        audioSources.clip = Microphone.Start(null, true, 10, 44100);
        audioSources.loop = false;
        audioSources.spatialBlend = 0.0f;
        yield return new WaitForSeconds(3f);
        Microphone.End(null); // 集音開始
        Debug.Log("Finish record");

        // 録音したか確かめるため録音内容を再生
        audioSources.Play();

        // SpeechToText を日本語指定して録音音声を文字に変換
        m_SpeechToText.RecognizeModel = "ja-JP_BroadbandModel";
        m_SpeechToText.Recognize(audioSources.clip, HandleOnRecognize);
    }

    void HandleOnRecognize(SpeechRecognitionEvent result)
    {
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    string text = alt.transcript;
                    Debug.Log(string.Format("{0} ({1}, {2:0.00})\n", text, res.final ? "Final" : "Interim", alt.confidence));
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
