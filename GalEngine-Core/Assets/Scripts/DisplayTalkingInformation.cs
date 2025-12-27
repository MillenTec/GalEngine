/*
 * Gaming页的主逻辑
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Image backgroundImage;
    public TextMeshProUGUI descriptionText;
    public MessageBoxController messageBoxController;

    private string _packPath;
    private JObject _config;
    private TextAsset _jsonData;
    private int? _ordinalNumber = null;
    private string _jsonFullPath;
    private JArray _plotJsonData;
    private bool _isInTextOutput; // 用于显示是否处于逐字输出迭代器中，为了使跳过判定迭代器拥有退出条件
    private bool _isNextOrSkipKeyDown; // 用于检测空格或向下键是否按下
    private int _tempTalkingValueOutputSpeed; // 临时存储Speed值，用于跳过逐字输出中使用

    private IEnumerator Start() {
        _packPath = GameEvents.GamingPackPath;
        GameEvents.GamingPackPath = null;
        TextMeshProUGUI[] textMashProList = GetComponentsInChildren<TextMeshProUGUI>();
        FontResourceManager fontResourceManager = new FontResourceManager();
        fontResourceManager.SetTMProFont(textMashProList);
        Debug.Log("字体已完成设置");
        yield return StartCoroutine(WaitFontSetDone(fontResourceManager));
        try {
            if (!File.Exists($"{_packPath}/config.json")) {
                throw new Exception($"ERROR: 文件{_packPath}/config.json不存在");
            }

            string configValue = File.ReadAllText($"{_packPath}/config.json");
            _config = JObject.Parse(configValue);
            if (_config["index"] == null) {
                throw new Exception("ERROR: 该config文件中不存在\"index\"键值");
            }

            string jsonPath = _config["index"].ToString();
            _jsonFullPath = $"{_packPath}/{jsonPath}";
            if (!File.Exists(_jsonFullPath)) {
                throw new Exception($"ERROR: 文件{_jsonFullPath}不存在");
            }

            string jsonData = File.ReadAllText(_jsonFullPath);
            _plotJsonData = JArray.Parse(jsonData);
            
            Debug.Log("校验完毕");
            
            StartCoroutine(BeginDialogue());
        } catch (Exception ex) {
            Debug.LogError(ex);
            messageBoxController.MessageBox("无法打开", $"打开此剧情包时遇到错误\n{ex}\n请尝试解决后再打开", MessageBoxType.OnlyOk);
        }
    }

    IEnumerator BeginDialogue() {
        yield return StartCoroutine(TraverseJson(_plotJsonData));
        SceneManager.LoadSceneAsync("PackManager");
    }

    private void OnEnable() {
        messageBoxController.OnMessageBoxButtonClick += RaiseOnMessageBoxButtonClick;
    }

    void RaiseOnMessageBoxButtonClick(Button button) {
        messageBoxController.CloseMessageBox();
        SceneManager.LoadSceneAsync("PackManager");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow)) {
            _isNextOrSkipKeyDown = true;
        } else {
            _isNextOrSkipKeyDown = false;
        }
    }

    IEnumerator WaitFontSetDone(FontResourceManager fontResourceManager) {
        while (!fontResourceManager.IsSetDone) {
            yield return null;
        }
    }

    private IEnumerator TraverseJson(JArray tempJsonData) {
        for (int i = 0; i < tempJsonData.Count; i++) {   // 遍历JSON的第一层数组，并调用显示名字及内容迭代器直到结束
            yield return StartCoroutine(DisplaySpeakerNameAndValue(tempJsonData, i));
            Debug.Log($"完成第{i}层");
        }
        Debug.Log("已退出协程");
    }

    private IEnumerator WaitNextKeyDown() {
        Debug.Log("等待按下按键");
        while (!_isNextOrSkipKeyDown) yield return null;
        while (_isNextOrSkipKeyDown) yield return null;  // 等待放开按键(防抖)
        Debug.Log("下一步");
    }

    private IEnumerator TraverseChoice(JArray choiceJson) {
        for (int i = 0; i < choiceJson.Count; i++) {
            int ordinalId = (int)choiceJson[i]["ordinal"];
            if (ordinalId == 0) {
                choicesBox0Text.text = (string)choiceJson[i]["value"];
                StartCoroutine(Animation.CanvasFadeIn(choicesBoxCanvasGroup0, 0.1f));
            }else if (ordinalId == 1) {
                choicesBox1Text.text = (string)choiceJson[i]["value"];
                StartCoroutine(Animation.CanvasFadeIn(choicesBoxCanvasGroup1, 0.1f));
            }else if (ordinalId == 2) {
                choicesBox2Text.text = (string)choiceJson[i]["value"];
                StartCoroutine(Animation.CanvasFadeIn(choicesBoxCanvasGroup2, 0.1f));
            }else if (ordinalId == 3) {
                choicesBox3Text.text = (string)choiceJson[i]["value"];
                StartCoroutine(Animation.CanvasFadeIn(choicesBoxCanvasGroup3, 0.1f));
            }else if (ordinalId == 4) {
                choicesBox4Text.text = (string)choiceJson[i]["value"];
                StartCoroutine(Animation.CanvasFadeIn(choicesBoxCanvasGroup4, 0.1f));
            }
            yield return null;
        }
    }

    // TODO: 比较早期写的代码，现在看来有一些地方处理得不好，未来可能重构
    private IEnumerator DisplaySpeakerNameAndValue(JArray jsonData, int nodeId) {
        Debug.Log($"开始播放{nodeId}");
        int[] jsonIndex = new int[2];  // 两个值的数组，索引0存储的是顶层JSON数组，索引1存储的是选择分支的编号
        jsonIndex[0] = nodeId;
        Debug.Log($"{jsonIndex[0] == (int)jsonData[jsonIndex[0]]["node"]} {nodeId} {jsonData[jsonIndex[0]]["node"]}");
        if (jsonIndex[0] == (int)jsonData[jsonIndex[0]]["node"]) {
            if (jsonData[jsonIndex[0]]["background"] != null) {
                string background = jsonData[jsonIndex[0]]["background"].ToString();
                if (background != "NULL") {
                    string backgroundPath = $"{_packPath}/{background}";
                    try {
                        if (!File.Exists(backgroundPath)) {
                            throw new WarningException($"WARNING: 文件{backgroundPath}不存在");
                        }

                        Sprite sprite = ExternalResourceLoader.LoadSpriteFromFile(backgroundPath);
                        backgroundImage.sprite = sprite;
                        StartCoroutine(Animation.ChangeColor(backgroundImage, backgroundImage.color, Color.white, 0.2f));
                    } catch (WarningException wex) {
                        Debug.LogWarning(wex);
                    } catch (Exception ex) {
                        Debug.LogError(ex);
                    }

                    yield return null;
                } else if (background == "NULL") {
                    backgroundImage.sprite = null;
                    StartCoroutine(Animation.ChangeColor(backgroundImage, backgroundImage.color, Color.black, 0.2f));
                }
            }
            string nodeType = (string)jsonData[jsonIndex[0]]["type"];
            if (nodeType == "branch") {
                JArray choiceJson = (JArray)jsonData[jsonIndex[0]]["choice"];
                yield return StartCoroutine(TraverseChoice(choiceJson));
                yield return StartCoroutine(WaitOrdinalNumberNoNull());
                #region 使所有选择框渐隐

                if (choicesBoxCanvasGroup0.alpha != 0) {
                    StartCoroutine(Animation.CanvasFadeOut(choicesBoxCanvasGroup0, 0.1f));
                }
                
                if (choicesBoxCanvasGroup1.alpha != 0) {
                    StartCoroutine(Animation.CanvasFadeOut(choicesBoxCanvasGroup1, 0.1f));
                }
                
                if (choicesBoxCanvasGroup2.alpha != 0) {
                    StartCoroutine(Animation.CanvasFadeOut(choicesBoxCanvasGroup2, 0.1f));
                }
                
                if (choicesBoxCanvasGroup3.alpha != 0) {
                    StartCoroutine(Animation.CanvasFadeOut(choicesBoxCanvasGroup3, 0.1f));
                }
                
                if (choicesBoxCanvasGroup4.alpha != 0) {
                    StartCoroutine(Animation.CanvasFadeOut(choicesBoxCanvasGroup4, 0.1f));
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
                Debug.Log("This a node");
                string nameValue = (string)jsonData[jsonIndex[0]]["character"];
                string talkValue = (string)jsonData[jsonIndex[0]]["value"];
                if (jsonData[jsonIndex[0]]["description"] != null) {
                    string description = (string)jsonData[jsonIndex[0]]["description"];
                    descriptionText.text = description;
                } else {
                    descriptionText.text = "";
                }
                Debug.Log($"Name:{nameValue}  Value:{talkValue}");
                nameText.text = nameValue;
                //OnDialogueChange?.Invoke();
                Debug.Log($"正在等待按下下一个按键");
                yield return StartCoroutine(OutputTalkingValue(talkValue, talkingValueOutputSpeed));
                yield return StartCoroutine(WaitNextKeyDown());
            }
        } else {
            Debug.Log("ERROR: JSON格式错误，请确保ID编号正确");
        }
        
        Debug.Log($"{nodeId}已完成");
    }

    IEnumerator WaitOrdinalNumberNoNull() {
        _ordinalNumber = null;
        _isNextOrSkipKeyDown = false;
        Input.ResetInputAxes();
        yield return null;
        while (_ordinalNumber == null) {
            yield return null;
        }
    }

    private IEnumerator OutputTalkingValue(string value, int speed) {
        Debug.Log("等待输出完毕...");
        _isInTextOutput = true;
        _tempTalkingValueOutputSpeed = speed;  // 重置临时速度，这个值可能会在GetSkipAndSetTempSpeed方法中被更改
        StartCoroutine(GetSkipAndSetTempSpeed());
        talkingText.text = "";
        char[] valueArray = value.ToCharArray();
        foreach (char word in valueArray) {
            talkingText.text += word;
            yield return null;
            speed = _tempTalkingValueOutputSpeed;  // 每次循环检查一次临时速度的值，如果速度变为0会被更改
            float speedSeconds = speed / 1000f;
            yield return new WaitForSeconds(speedSeconds);
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