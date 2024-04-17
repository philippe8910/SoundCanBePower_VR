using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpeechToText : MonoBehaviour
{
    // OpenAI API 访问密钥
    private string apiKey = "YOUR_OPENAI_API_KEY";

    // OpenAI API 端点
    private string apiUrl = "https://api.openai.com/v1/engines/davinci/completions";

    // 准备要发送的语音数据
    // 这里假设你已经从语音输入中获取了语音数据，这里简化为字符串
    private string voiceData = "YOUR_VOICE_DATA";

    // Unity 启动时调用
    void Start()
    {
        StartCoroutine(TranscribeSpeech());
    }

    // 发送语音数据到 OpenAI API
    IEnumerator TranscribeSpeech()
    {
        // 准备要发送的 JSON 数据
        string json = "{\"prompt\": \"" + voiceData + "\", \"max_tokens\": 150}";

        // 创建一个新的 UnityWebRequest 对象
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        // 设置请求头部，包括 API 访问密钥
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // 设置请求体为 JSON 数据
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // 发送请求
        yield return request.SendWebRequest();

        // 检查是否出现错误
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // 从 API 响应中提取文本数据
            string response = request.downloadHandler.text;
            Debug.Log("Transcribed Text: " + response);
            
            // 在这里你可以将转录的文本显示在 Unity 中的 UI 上
        }
    }
}
