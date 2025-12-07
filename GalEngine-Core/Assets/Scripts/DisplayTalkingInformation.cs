using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTalkingInformation : MonoBehaviour {
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI talkingText;
    public int talkingValueOutputSpeed = 50; // 为了方便，此处的单位为ms
    public CanvasGroup choicesBoxCanvasGroup0;
    public CanvasGroup choicesBoxCanvasGroup1;
    public CanvasGroup choicesBoxCanvasGroup2;
    public CanvasGroup choicesBoxCanvasGroup3;
    public CanvasGroup choicesBoxCanvasGroup4;
    public TextMeshProUGUI choicesBox0Text;
    public TextMeshProUGUI choicesBox1Text;
    public TextMeshProUGUI choicesBox2Text;
    public TextMeshProUGUI choicesBox3Text;
    public TextMeshProUGUI choicesBox4Text;

    private bool _isChoosing = false;
    private TextAsset _jsonData;
    private int ?_ordinalNumber = null;
    private JArray _plotJsonData;
    private bool _isInTextOutput; // 用于显示是否处于逐字输出迭代器中，为了使跳过判定迭代器拥有退出条件
    private bool _isNextOrSkipKeyDown; // 用于检测空格或向下键是否按下
    private int _tempTalkingValueOutputSpeed; // 临时存储Speed值，用于跳过逐字输出中使用

    private void Awake() {
        _jsonData = Resources.Load<TextAsset>("tempTest/data");
        _plotJsonData = JArray.Parse(_jsonData.text);
    }

    private void Start() {
        TextMeshProUGUI[] textMashProList = GetComponentsInChildren<TextMeshProUGUI>(); 
        FontResourceManager fontResourceManager = new FontResourceManager();
        fontResourceManager.SetTMProFont(textMashProList);
        StartCoroutine(WaitFontSetDone(fontResourceManager));
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow)) {
            if (!_isChoosing) {
                _isNextOrSkipKeyDown = true;
            }
        } else {
            _isNextOrSkipKeyDown = false;
        }
        
        //Debug.Log(_ordinalNumber);
    }

    IEnumerator WaitFontSetDone(FontResourceManager fontResourceManager) {
        while (!fontResourceManager.IsSetDone) {
            yield return null;
        }
        StartCoroutine(TraverseJson(_plotJsonData));
    }

    private IEnumerator TraverseJson(JArray tempJsonData) {
        for (int i = 0; i < tempJsonData.Count; i++) // 遍历JSON的第一层数组，并调用显示名字及内容迭代器直到结束
            yield return StartCoroutine(DisplaySpeakerNameAndValue(tempJsonData, i));
    }

    private IEnumerator WaitNextKeyDown() {
        while (!_isNextOrSkipKeyDown) yield return null;

        while (_isNextOrSkipKeyDown) yield return null;
    }

    private IEnumerator TraverseChoice(JArray choiceJson) {
        for (int i = 0; i < choiceJson.Count; i++) {
            int ordinalId = (int)choiceJson[i]["ordinal"];
            Debug.Log($"{ordinalId}  {choiceJson[i]["value"]}");
            if (ordinalId == 0) {
                choicesBox0Text.text = (string)choiceJson[i]["value"];
                yield return StartCoroutine(CanvasFadeIn(choicesBoxCanvasGroup0, 0.1f));
            }else if (ordinalId == 1) {
                choicesBox1Text.text = (string)choiceJson[i]["value"];
                yield return StartCoroutine(CanvasFadeIn(choicesBoxCanvasGroup1, 0.1f));
            }else if (ordinalId == 2) {
                choicesBox2Text.text = (string)choiceJson[i]["value"];
                yield return StartCoroutine(CanvasFadeIn(choicesBoxCanvasGroup2, 0.1f));
            }else if (ordinalId == 3) {
                choicesBox3Text.text = (string)choiceJson[i]["value"];
                yield return StartCoroutine(CanvasFadeIn(choicesBoxCanvasGroup3, 0.1f));
            }else if (ordinalId == 4) {
                choicesBox4Text.text = (string)choiceJson[i]["value"];
                yield return StartCoroutine(CanvasFadeIn(choicesBoxCanvasGroup4, 0.1f));
            }
            yield return null;
        }
    }

    private IEnumerator DisplaySpeakerNameAndValue(JArray jsonData, int nodeId) {
        int[] jsonIndex = new int[2];
        jsonIndex[0] = nodeId;
        if (jsonIndex[0] == (int)jsonData[jsonIndex[0]]["node"]) {
            string nodeType = (string)jsonData[jsonIndex[0]]["type"];
            if (nodeType == "branch") {
                JArray choiceJson = (JArray)jsonData[jsonIndex[0]]["choice"];
                yield return StartCoroutine(TraverseChoice(choiceJson));
                yield return StartCoroutine(WaitOrdinalNumberNoNull());
                #region 使所有选择框渐隐

                if (choicesBoxCanvasGroup0.alpha != 0) {
                    yield return StartCoroutine(CanvasFadeOut(choicesBoxCanvasGroup0, 0.1f));
                }
                
                if (choicesBoxCanvasGroup1.alpha != 0) {
                    yield return StartCoroutine(CanvasFadeOut(choicesBoxCanvasGroup1, 0.1f));
                }
                
                if (choicesBoxCanvasGroup2.alpha != 0) {
                    yield return StartCoroutine(CanvasFadeOut(choicesBoxCanvasGroup2, 0.1f));
                }
                
                if (choicesBoxCanvasGroup3.alpha != 0) {
                    yield return StartCoroutine(CanvasFadeOut(choicesBoxCanvasGroup3, 0.1f));
                }
                
                if (choicesBoxCanvasGroup4.alpha != 0) {
                    yield return StartCoroutine(CanvasFadeOut(choicesBoxCanvasGroup4, 0.1f));
                }

                #endregion

                try {
                    jsonIndex[1] = (int)_ordinalNumber;
                } catch(NullReferenceException) {
                    Debug.Log("ERROR: 选择项为Null!");
                    yield break;
                }
                Debug.Log(_ordinalNumber);
                _ordinalNumber = null;
                Debug.Log(_ordinalNumber);
                JArray jsonBranch;
                try {
                    jsonBranch = (JArray)choiceJson[jsonIndex[1]]["branch"];
                } catch {
                    Debug.Log("ERROR: JSON文件不合法，branch下不是Array");
                    yield break;
                }
                yield return StartCoroutine(TraverseJson(jsonBranch));
            } else if (nodeType == "node") {
                string nameValue = (string)jsonData[jsonIndex[0]]["character"];
                string talkValue = (string)jsonData[jsonIndex[0]]["value"];
                Debug.Log($"Name:{nameValue}  Value:{talkValue}");
                nameText.text = nameValue;
                yield return StartCoroutine(OutputTalkingValue(talkValue, talkingValueOutputSpeed));
                yield return StartCoroutine(WaitNextKeyDown());
            }
        } else {
            Debug.Log("ERROR: JSON格式错误，请确保ID编号正确");
        }
    }

    IEnumerator WaitOrdinalNumberNoNull() {
        _ordinalNumber = null;
        _isChoosing = true;
        _isNextOrSkipKeyDown = false;
        Input.ResetInputAxes();
        yield return null;
        while (_ordinalNumber == null) {
            yield return null;
        }

        _isChoosing = false;
    }

    private IEnumerator OutputTalkingValue(string value, int speed) {
        _isInTextOutput = true;
        _tempTalkingValueOutputSpeed = speed;
        StartCoroutine(GetSkipAndSetTempSpeed());
        talkingText.text = "";
        char[] valueArray = value.ToCharArray();
        foreach (char word in valueArray) {
            talkingText.text += word;
            yield return null;
            speed = _tempTalkingValueOutputSpeed;
            float speedF = speed / 1000f;
            yield return new WaitForSeconds(speedF);
        }

        _tempTalkingValueOutputSpeed = talkingValueOutputSpeed;
        _isInTextOutput = false;
    }

    private IEnumerator GetSkipAndSetTempSpeed() {
        while (_isInTextOutput) {
            if (_isNextOrSkipKeyDown) _tempTalkingValueOutputSpeed = 0;
            yield return null;
        }
    }

    private IEnumerator WaitSkipKeyUp() {
        while (!(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow))) yield return null;
    }

    private IEnumerator CanvasFadeIn(CanvasGroup canvas, float speed) {
        canvas.blocksRaycasts = true;
        float time = 0f;
        float startAlpha = 0f;
        float endAlpha = 1f;

        while (time <= speed) {
            time += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, time / speed);
            canvas.alpha = currentAlpha;
            yield return null;
        }

        canvas.alpha = 1f;
    }

    private IEnumerator CanvasFadeOut(CanvasGroup canvas, float speed) {
        float time = 0f;
        float startAlpha = 1f;
        float endAlpha = 0f;

        while (time <= speed) {
            time += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, time / speed);
            canvas.alpha = currentAlpha;
            yield return null;
        }

        canvas.alpha = 0f;
        canvas.blocksRaycasts = false;
    }

    #region 当选择框按钮按下

    public void ChoiceBtn0Click() {
        _ordinalNumber = 0;
    }
    
    public void ChoiceBtn1Click() {
        _ordinalNumber = 1;
    }
    
    public void ChoiceBtn2Click() {
        _ordinalNumber = 2;
    }
    
    public void ChoiceBtn3Click() {
        _ordinalNumber = 3;
    }
    
    public void ChoiceBtn4Click() {
        _ordinalNumber = 4;
    }

    #endregion
}