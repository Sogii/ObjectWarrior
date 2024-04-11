
// Add Newtonsoft package in package manager -> add by name -> com.unity.nuget.newtonsoft-json


using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ObjectDetection : MonoBehaviour
{

    public class ChatResponse
    {
        public List<ChatResponseChoice> choices { get; set; }
        public int created { get; set; }
        public string id { get; set; }
        public string model { get; set; }
        public string @object { get; set; }
        public ChatResponseUsage usage { get; set; }
    }

    public class ChatResponseChoice
    {
        public string finish_reason { get; set; }
        public int index { get; set; }
        public ChatResponseMessage message { get; set; }
        public object logprobs { get; set; }
    }

    public class ChatResponseMessage
    {
        public string content { get; set; }
        public string role { get; set; }
    }

    public class ChatResponseUsage
    {
        public int completion_tokens { get; set; }
        public int prompt_tokens { get; set; }
        public int total_tokens { get; set; }
    }



    public class VisionResponse
    {
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public VisionResponseUsage usage { get; set; }
        public List<VisionResponseChoice> choices { get; set; }
    }

    public class VisionResponseChoice
    {
        public VisionResponseMessage message { get; set; }
        public string finish_reason { get; set; }
        public int index { get; set; }
    }

    public class VisionResponseMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class VisionResponseUsage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }



    public class DallEResponse
    {
        public int created { get; set; }
        public List<DallEMessage> data { get; set; }
    }

    public class DallEMessage
    {
        public string revised_prompt { get; set; }
        public string url { get; set; }
    }


    public class BGRemoveRequestResponse
    {
        public BGRemoveRequestResponseLinks _links { get; set; }
    }
    public class BGRemoveRequestResponseLinks
    {
        public BGRemoveRequestResponseSelf self { get; set; }
    }

    public class BGRemoveRequestResponseSelf
    {
        public string href { get; set; }
    }






    public class BGRemoveRequestStatus
    {
        public string jobID { get; set; }
        public string status { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string input { get; set; }
        public BGRemoveRequestStatusOptions options { get; set; }
        public BGRemoveRequestStatusMetadata metadata { get; set; }
        public BGRemoveRequestStatusLinks _links { get; set; }
        public BGRemoveRequestStatusOutput output { get; set; }
    }
    public class BGRemoveRequestStatusLinks
    {
        public BGRemoveRequestStatusSelf self { get; set; }
    }

    public class BGRemoveRequestStatusMask
    {
        public string format { get; set; }
    }

    public class BGRemoveRequestStatusMetadata
    {
        public BGRemoveRequestStatusService service { get; set; }
        public BGRemoveRequestStatusModel model { get; set; }
    }

    public class BGRemoveRequestStatusModel
    {
        public string classification { get; set; }
        public string universal { get; set; }
    }

    public class BGRemoveRequestStatusOptions
    {
        public string optimize { get; set; }
    }

    public class BGRemoveRequestStatusOutput
    {
        public string href { get; set; }
        public string storage { get; set; }
        public bool overwrite { get; set; }
        public BGRemoveRequestStatusMask mask { get; set; }
    }

    public class BGRemoveRequestStatusSelf
    {
        public string href { get; set; }
    }

    public class BGRemoveRequestStatusService
    {
        public string version { get; set; }
    }









    public class BGCropRequestStatus
    {
        public string jobId { get; set; }
        public List<BGCropRequestStatusOutput> outputs { get; set; }
        public BGCropRequestStatusLinks _links { get; set; }
    }
    public class BGCropRequestStatusLinks
    {
        public List<BGCropRequestStatusRendition> renditions { get; set; }
        public BGCropRequestStatusSelf self { get; set; }
    }

    public class BGCropRequestStatusOutput
    {
        public string input { get; set; }
        public string status { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public BGCropRequestStatusLinks _links { get; set; }
    }

    public class BGCropRequestStatusRendition
    {
        public string href { get; set; }
        public string storage { get; set; }
        public string type { get; set; }
    }

    public class BGCropRequestStatusSelf
    {
        public string href { get; set; }
    }




    public class AdobeTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }













    // BEGIN VARIABLES

    //keys
    string openaikey = "";
    string adobekey = "";
    string adobetoken = "";
    string adobesecret = "";
    //object properties
    // string object_image_url = "https://www.taylorguitars.com/sites/default/files/styles/guitar_mobile_upright/public/2022-02-08/Academy%2010-Front.png?itok=cXQ4jVGc";
    int object_pixel_width = 64;
    int object_pixel_height = 64;
    public int minimal_height = 24;
    public int maximum_height = 128;
    public string[] object_properties;
    public bool is_object_generation_done = false;
    public bool is_upgrade_generation_done = false;
    bool is_object_properties_generated;
    public RawImage display;
    public RawImage webcamFeed;
    Texture2D generated_object_image;
    Texture2D scaled_object_image;
    public Sprite generated_sprite;

    //api responses
    DallEResponse gpt_dalle_response;
    ChatResponse gpt_chat_response;
    VisionResponse gpt_vision_response;
    BGRemoveRequestResponse bgremove_request_response;
    BGRemoveRequestResponse bgcrop_request_response;
    BGRemoveRequestStatus bgremove_request_status;

    //webcam
    public WebCamTexture webcamTexture;
    public bool picture_taken;

    // END VARIABLES
    public static ObjectDetection instance;
    private string initialPrompt;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        openaikey = PlayerPrefs.GetString("OpenAIKey");
        adobekey = PlayerPrefs.GetString("AdobeKey");
        adobesecret = PlayerPrefs.GetString("AdobeSecret");
        Debug.Log("OpenAIKey: " + openaikey);
        Debug.Log("AdobeKey: " + adobekey);
        Debug.Log("AdobeSecret: " + adobesecret);
    }
    // BEGIN START UPDATE

    void Start()
    {
        picture_taken = false;
        InitiateWebcam();
    }

    // Update is called once per frame

    public void StartGenerationButton()
    {
        if (picture_taken == false)
        {
            is_object_generation_done = false;

            // Start webcam and take picture
            Texture2D webcam_picture = GetTexture2DFromWebcamTexture(webcamTexture);
            webcamTexture.Pause();
            picture_taken = true;

            // Convert picture to base64 string
            string base64Img;
            base64Img = System.Convert.ToBase64String(webcam_picture.EncodeToPNG());

            StartCoroutine(generateAdobeToken());
            StartCoroutine(identifyObject(base64Img));
        }
    }

    public Item GetGeneratedObjectItem()
    {
        is_object_generation_done = false;
        Item generated_item = RandomGenerationUtility.GenerateItemFromAIOutput(object_properties, generated_sprite);
        generated_item.UpgradeText = initialPrompt;

        return generated_item;
    }

    // BEGIN NORMAL FUNCTIONS
    public static Texture2D Resize(Texture2D source, int newWidth, int newHeight)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Point;
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D nTex = new Texture2D(newWidth, newHeight);
        nTex.filterMode = FilterMode.Point;
        nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        nTex.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        print("Scaled down image to " + newWidth + "x" + newHeight);
        return nTex;
    }

    public static Sprite ConvertToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    public static double widthToHeightRatio(int width, int height)
    {
        double ratio = (double)width / height;
        return ratio;
    }

    public Texture2D GetTexture2DFromWebcamTexture(WebCamTexture webCamTexture)
    {
        // Create new texture2d
        Texture2D tx2d = new Texture2D(webCamTexture.width, webCamTexture.height);
        // Gets all color data from web cam texture and then Sets that color data in texture2d
        tx2d.SetPixels(webCamTexture.GetPixels());
        // Applying new changes to texture2d
        tx2d.Apply();
        return tx2d;
    }

    public void InitiateWebcam()
    {
        picture_taken = false;
        webcamTexture = new WebCamTexture();
        webcamFeed.texture = webcamTexture;
        webcamTexture.Play();
        print("Webcam launched");
    }

    // END NORMAL FUNCTIONS


    // BEGIN ASYNC FUNCTIONS

    // BEGIN OBJECT GENERATE FUNCTIONS
    IEnumerator identifyObject(string object_to_identify_image_url)
    {
        string object_instruction = "Identify the main object in this image. Only answer in the following format: {identified_main_object,identified_main_object_color}";
        string json = "{\"model\": \"gpt-4-vision-preview\",\"messages\": [{\"role\": \"user\",\"content\": [{\"type\": \"text\",\"text\": \"" + object_instruction + "\"},{\"type\": \"image_url\",\"image_url\": {\"url\": \"data:image/png;base64," + object_to_identify_image_url + "\"}}]}],\"max_tokens\": 300}";
        // print(json);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        print("Sending request to identify the object...");

        using (var www = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + openaikey);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", " text/plain");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                print("Succes!");
                VisionResponse result = JsonConvert.DeserializeObject<VisionResponse>(www.downloadHandler.text);
                // print(result);
                gpt_vision_response = result;
                string result_string = gpt_vision_response.choices[0].message.content;

                result_string = result_string.Replace("{", "").Replace("}", "");
                initialPrompt = result_string;
                string[] result_words = result_string.Split(',');
                for (int i = 0; i < result_words.Length; i++)
                {
                    Debug.Log(result_words[i]);
                }


                StartCoroutine(getGeneratedImage(result_words));
                StartCoroutine(getObjectProperties(result_words));
            }
        }
    }




    IEnumerator getObjectProperties(string[] object_words)
    {
        string content_to_send = "A combat system in a fantasy game has a player using a " + object_words[1] + " " + object_words[0] + ". Choose the action of the object by choosing properties from the following options:    actionElement: - fire - water - ice - nature - poison - normal - sound - ground - electric - explosive  actionType: - damage - healing  actionAmount: value from 7-15  The actionElement decides on what element the object is. actionType decides on what type of action the object is able to do and actionAmount decides on the strength of the action. Also generate a funny name for the " + object_words[1] + " " + object_words[0] + " called objectName and a catchy name for the action called actionName. Finally, calculate the height of the object in pixels in comparison to the player as objectHeight. The player height is 48 pixels.  ONLY answer in the following format: {objectName,objectHeight,actionName,actionElement,actionType,actionAmount}";
        string json = "{\"model\": \"gpt-3.5-turbo-0125\",\"messages\": [{\"role\": \"user\",\"content\": \"" + content_to_send + "\"}]}";
        // print(json);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        print("Generating properties of item...");

        using (var www = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + openaikey);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", " text/plain");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                print("Succes!");
                ChatResponse result = JsonConvert.DeserializeObject<ChatResponse>(www.downloadHandler.text);
                // print(result);
                gpt_chat_response = result;
                string result_string = gpt_chat_response.choices[0].message.content;

                result_string = result_string.Replace("{", "").Replace("}", "");
                object_properties = result_string.Split(',');
                for (int i = 0; i < object_properties.Length; i++)
                {
                    print(object_properties[i]);
                }
                object_pixel_height = Int32.Parse(Regex.Replace(object_properties[1], "[^.0-9]", ""));
                // StartCoroutine(getGeneratedImage(object_properties));
            }
        }
    }





    IEnumerator getGeneratedImage(string[] object_words)
    {

        string json = "{\"model\": \"dall-e-3\",\"prompt\": \"A clean image of a " + object_words[1] + " " + object_words[0] + " with a FULLY white #FFF background. ONLY create the object itself. Make sure the object is FULLY visible and create a black outline around the object. DO NOT create shadow and reflection. Use the pixel art style with a pixel size of 32.\",\"n\": 1,\"size\": \"1024x1024\",\"style\": \"vivid\"}";
        // print(json);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        print("Sending request to generate an image...");

        using (var www = new UnityWebRequest("https://api.openai.com/v1/images/generations", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + openaikey);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", " text/plain");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                print("Succesfully generated an image!");
                // print(www.downloadHandler.text);
                DallEResponse result = JsonConvert.DeserializeObject<DallEResponse>(www.downloadHandler.text);
                // // print(result);
                gpt_dalle_response = result;
                print(gpt_dalle_response.data[0].url);
                StartCoroutine(removeImageBackground(gpt_dalle_response.data[0].url));
            }
        }
    }



    IEnumerator generateAdobeToken()
    {
        string datastring = "client_id=" + adobekey + "&client_secret=" + adobesecret + "&grant_type=client_credentials&scope=openid,AdobeID,read_organizations";
        var data = Encoding.UTF8.GetBytes(datastring);

        using (var www = new UnityWebRequest("https://ims-na1.adobelogin.com/ims/token/v3", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(data);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                print("Token generated.");
                // print(www.downloadHandler.text);
                AdobeTokenResponse result = JsonConvert.DeserializeObject<AdobeTokenResponse>(www.downloadHandler.text);
                print(result.access_token);
                adobetoken = result.access_token;
            }
        }
    }




    IEnumerator removeImageBackground(string image_to_process)
    {
        string output_url = "https://m21.blob.core.windows.net/pixelapp/bg-removed.jpg?sp=racwdli&st=2024-03-29T17:41:05Z&se=2024-07-06T00:41:05Z&sv=2022-11-02&sr=c&sig=7uwBIFo5KQfnty%2B3NWRQjLrQecVAVnDj6IczUqhbj1Y%3D";
        string json = "{\"input\":{\"href\":\"" + image_to_process + "\",\"storage\":\"external\"},\"output\":{\"href\":\"" + output_url + "\",\"storage\":\"azure\",\"mask\":{\"format\":\"soft\"}}}";
        print(json);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        print("Sending request to delete background...");

        using (var www = new UnityWebRequest("https://image.adobe.io/sensei/cutout", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + adobetoken);
            www.SetRequestHeader("x-api-key", adobekey);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
                StartCoroutine(GetRemoteTexture(image_to_process, object_pixel_width, object_pixel_height));
            }
            else
            {
                print("Succes submitted request!");
                // print(www.downloadHandler.text);
                BGRemoveRequestResponse result = JsonConvert.DeserializeObject<BGRemoveRequestResponse>(www.downloadHandler.text);
                // print(result.links[0]);
                bgremove_request_response = result;
                // print(gpt_dalle_response.data[0].url);
                StartCoroutine(checkStatusOfImage(bgremove_request_response._links.self.href, "bgremove"));
            }
        }
    }




    IEnumerator cropImage(string image_to_process)
    {
        string input_url = "https://m21.blob.core.windows.net/pixelapp/bg-removed.jpg?sp=racwdli&st=2024-03-29T17:41:05Z&se=2024-07-06T00:41:05Z&sv=2022-11-02&sr=c&sig=7uwBIFo5KQfnty%2B3NWRQjLrQecVAVnDj6IczUqhbj1Y%3D";
        string output_url = "https://m21.blob.core.windows.net/pixelapp/object-cropped.jpg?sp=racwdli&st=2024-03-29T17:41:05Z&se=2024-07-06T00:41:05Z&sv=2022-11-02&sr=c&sig=7uwBIFo5KQfnty%2B3NWRQjLrQecVAVnDj6IczUqhbj1Y%3D";
        string json = "{\"inputs\":[{\"href\":\"" + input_url + "\",\"storage\":\"azure\"}],\"options\": {\"unit\":\"Pixels\",\"width\":4,\"height\":4},\n \"outputs\":[{\"href\":\"" + output_url + "\",\"storage\":\"azure\",\"type\":\"image/png\"}]}";
        // print(json);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        print("Cropping image...");

        using (var www = new UnityWebRequest("https://image.adobe.io/pie/psdService/productCrop", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + adobetoken);
            www.SetRequestHeader("x-api-key", adobekey);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                print("Succesfully submitted request!");
                // print(www.downloadHandler.text);
                BGRemoveRequestResponse result = JsonConvert.DeserializeObject<BGRemoveRequestResponse>(www.downloadHandler.text);
                // print(result.links[0]);
                bgcrop_request_response = result;
                // print(gpt_dalle_response.data[0].url);
                StartCoroutine(checkStatusOfImage(bgcrop_request_response._links.self.href, "crop"));
            }
        }
    }


    IEnumerator checkStatusOfImage(string status_url, string process_type)
    {
        // print(json);
        print("Checking status...");

        using (var www = new UnityWebRequest(status_url, "GET"))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + adobetoken);
            www.SetRequestHeader("x-api-key", adobekey);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // print("Succes!");
                // print(www.downloadHandler.text);
                yield return new WaitForSeconds(2);
                // string status = "";
                if (process_type == "bgremove")
                {
                    BGRemoveRequestStatus result = JsonConvert.DeserializeObject<BGRemoveRequestStatus>(www.downloadHandler.text);
                    string status = result.status;
                    if (status == "succeeded")
                    {
                        StartCoroutine(cropImage(result._links.self.href));
                    }
                    else
                    {
                        StartCoroutine(checkStatusOfImage(bgremove_request_response._links.self.href, "bgremove"));
                    }
                }
                else if (process_type == "crop")
                {
                    BGCropRequestStatus result = JsonConvert.DeserializeObject<BGCropRequestStatus>(www.downloadHandler.text);
                    string status = result.outputs[0].status;
                    if (status == "succeeded")
                    {
                        StartCoroutine(GetRemoteTexture(result.outputs[0]._links.renditions[0].href, object_pixel_width, object_pixel_height));
                    }
                    else
                    {
                        StartCoroutine(checkStatusOfImage(bgcrop_request_response._links.self.href, "crop"));
                    }
                }

            }
        }
    }



    IEnumerator GetRemoteTexture(string url, int object_width, int object_height)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                generated_object_image = DownloadHandlerTexture.GetContent(www);
                // display.texture = generated_object_image;
                // print(generated_object_image.width + " " + generated_object_image.height);
                double width_multiplier = widthToHeightRatio(generated_object_image.width, generated_object_image.height);
                // print(width_multiplier);
                if (object_height < minimal_height)
                {
                    object_height = minimal_height;
                }
                else if (object_height > maximum_height)
                {
                    object_height = maximum_height;
                }

                int new_width = Convert.ToInt32(object_height * width_multiplier);
                // print(new_width);

                yield return scaled_object_image = Resize(generated_object_image, new_width, object_height);

                // display.texture = scaled_object_image;
                // display.SetNativeSize();

                generated_sprite = ConvertToSprite(scaled_object_image);
                print("Sprite succesfully generated");

                is_object_generation_done = true;
                is_upgrade_generation_done = true;
            }
        }
    }

    // END OBJECT GENERATE FUNCTIONS











    // BEGIN UPGRADE FUNCTIONS
    public Item GetGeneratedUpgrade(Item itemToUpgrade)
    {
        Item item = new Item()
        {
            ItemName = itemToUpgrade.ItemName,
            ItemDescription = itemToUpgrade.ItemDescription,
            UpgradeText = itemToUpgrade.UpgradeText,
            ItemImage = generated_sprite,
            Abilities = new List<Ability>()
        };

        item.Abilities.Add(itemToUpgrade.Abilities[0]);

        item.Abilities[0].UpgradeAbility(upgrade_value);
        item.Abilities[0].UpgradeReason = upgrade_reason;

        item.Abilities[0].AbilityDescription = item.Abilities[0].GetDescription();
        item.ItemDescription = RandomGenerationUtility.GenerateItemDescription(item);


        itemToUpgrade.AddItemUpgrade(item);
        RandomGenerationUtility.GenerateItemDescription(itemToUpgrade);
        return itemToUpgrade;
    }
    // public int upgrade_value;
    // public string upgrade_reason;


    IEnumerator getObjectUpgradeSmall(string user_story)
    {
        is_upgrade_generation_done = false;
        string content_to_send = "A player in a fantasy game wants to upgrade his weapon. The value of the upgrade is decided by the amount of sentimental value the player expresses with a story. The user submits the following story: " + user_story + "  The value that is generated from the sentimental value of this story can range from -2 to 4. STRICTLY and ONLY answer in the following format: {upgradeValue}.";
        string json = "{\"model\": \"gpt-3.5-turbo-0125\",\"messages\": [{\"role\": \"user\",\"content\": \"" + content_to_send + "\"}]}";
        // print(json);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        print("Generating properties of item...");

        using (var www = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + openaikey);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", " text/plain");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                print("Succes!");
                ChatResponse result = JsonConvert.DeserializeObject<ChatResponse>(www.downloadHandler.text);
                // print(result);
                gpt_chat_response = result;
                string result_string = gpt_chat_response.choices[0].message.content;

                result_string = result_string.Replace("{", "").Replace("}", "");
                int upgrade_value = Int32.Parse(result_string);
                // for(int i = 0; i < object_properties.Length; i++)
                // {
                //     print(object_properties[i]);
                // }
                // int upgrade_value = Int32.Parse(object_properties[0]);
                // StartCoroutine(getGeneratedImage(object_properties));

                // FUNCTION TO UPGRADE ITEM GOES HERE
            }
        }
    }

    public int upgrade_value;
    public string upgrade_reason;


    public IEnumerator getObjectUpgradeBig(string original_object, string object_name, string user_story)
    {
        is_upgrade_generation_done = false;
        string chat_content_to_send = "A player in a fantasy game wants to upgrade his weapon. The value of the upgrade is decided by the amount of sentimental value the player expresses with a story. The user submits the following story: " + user_story + "  The value that is generated from the sentimental value of this story can range from -2 to 4. STRICTLY and ONLY answer in the following format: {upgradeReason:upgradeValue}.";
        string chat_json = "{\"model\": \"gpt-3.5-turbo-0125\",\"messages\": [{\"role\": \"user\",\"content\": \"" + chat_content_to_send + "\"}]}";
        // print(json);
        var chat_jsonBytes = Encoding.UTF8.GetBytes(chat_json);
        print("Generating properties of item...");

        using (var www = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(chat_jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + openaikey);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", " text/plain");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                print("Succes!");
                ChatResponse result = JsonConvert.DeserializeObject<ChatResponse>(www.downloadHandler.text);
                // print(result);
                gpt_chat_response = result;
                string result_string = gpt_chat_response.choices[0].message.content;

                result_string = result_string.Replace("{", "").Replace("}", "");
                string[] upgrade_properties = result_string.Split(':');
                for (int i = 0; i < upgrade_properties.Length; i++)
                {
                    print(upgrade_properties[i]);
                }
                int randomValue = new System.Random().Next(-2, 11); // Generates a random number between -2 and 10

                if (!Int32.TryParse(upgrade_properties[1], out upgrade_value))
                {
                    upgrade_value = randomValue;
                }
                else
                {
                    upgrade_reason = upgrade_properties[0];
                }
                upgrade_reason = upgrade_properties[0];
                StartCoroutine(getGeneratedImageUpgrade(original_object, object_name));
            }
        }
    }




    IEnumerator getGeneratedImageUpgrade(string original_object, string object_name)
    {

        string json = "{\"model\": \"dall-e-3\",\"prompt\": \"A clean image of a " + original_object + " called " + object_name + " with a FULLY white #FFF background. ONLY create the object itself. Make sure the object is FULLY visible and create a black outline around the object. DO NOT create shadow and reflection. Use the pixel art style with a pixel size of 32.\",\"n\": 1,\"size\": \"1024x1024\",\"style\": \"vivid\"}";
        // print(json);
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        print("Sending request to generate an image...");

        using (var www = new UnityWebRequest("https://api.openai.com/v1/images/generations", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonBytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + openaikey);
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", " text/plain");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                print("Succesfully generated an image!");
                // print(www.downloadHandler.text);
                DallEResponse result = JsonConvert.DeserializeObject<DallEResponse>(www.downloadHandler.text);
                // // print(result);
                gpt_dalle_response = result;
                print(gpt_dalle_response.data[0].url);
                StartCoroutine(removeImageBackground(gpt_dalle_response.data[0].url));
            }
        }
    }

    // END UPGRADE FUNCTIONS



    // END ASYNC FUNCTIONS



}
