using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CUI.UI
{
    /// <summary>
    /// UI工具箱
    /// </summary>
    public class UIScrollViewBox : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private Transform element;

        public delegate void SimpleDelegate(string _name, string _type);
        public delegate void FloatDelegate(string _name, string _type, float _value);
        public SimpleDelegate onElementClick;
        public SimpleDelegate onElementSelect;
        public FloatDelegate onElementSlider;

        public Dictionary<string, Transform> dic_elements = new Dictionary<string, Transform>();

        public void Add(UIElementInfor _elementInfor, Transform _element)
        {
            if (dic_elements.ContainsKey(_elementInfor.Name))
            {
                return;
            }

            InitElementInfor(_element, _elementInfor);
            InitElementEvent(_element);
            dic_elements.Add(_element.name, _element);
        }

        private void InitElementInfor(Transform _element, UIElementInfor _elementInfor)
        {
            foreach (var item in _elementInfor.ColorParameters)
            {
                Transform _colorChild = _element.Find(item.Key);
                if (_colorChild)
                {
                    Image _image = _colorChild.GetComponent<Image>();
                    if (_image)
                    {
                        _image.color = item.Value;
                    }
                }
            }
            foreach (var item in _elementInfor.TextParameters)
            {
                Transform _textChild = _element.Find(item.Key);
                if (_textChild)
                {
                    Text _text = _textChild.GetComponent<Text>();
                    if (_text)
                    {
                        _text.text = item.Value;
                    }
                }
            }
            foreach (var item in _elementInfor.ImageParameters)
            {
                Transform _imageChild = _element.Find(item.Key);
                if (_imageChild)
                {
                    Image _image = _imageChild.GetComponent<Image>();
                    if (_image)
                    {
                        _image.sprite = item.Value;
                    }
                }
            }
            Selectable[] selectables = _element.GetComponentsInChildren<Selectable>();
            foreach (var item in selectables)
            {
                if (_elementInfor.TriggerEnable.ContainsKey(item.name))
                {
                    item.interactable = _elementInfor.TriggerEnable[item.name];
                }
            }
        }

        private void InitElementEvent(Transform _element)
        {
            Button[] array_btn = _element.GetComponentsInChildren<Button>();
            foreach (var item in array_btn)
            {
                string _type = item.name;
                item.onClick.AddListener(delegate () { OnElementClick(_element.name, _type); });
            }

            Toggle[] array_toggle = _element.GetComponentsInChildren<Toggle>();
            foreach (var item in array_toggle)
            {
                string _type = item.name;
                item.onValueChanged.AddListener(delegate (bool isOn) { OnElementSelect(_element.name, _type, isOn); });
                if (content.GetComponent<ToggleGroup>()) { item.group = content.GetComponent<ToggleGroup>(); }
            }

            Slider[] array_slider = _element.GetComponentsInChildren<Slider>();
            foreach (var item in array_slider)
            {
                string _type = item.name;
                item.onValueChanged.AddListener(delegate (float value) { OnElementSlider(_element.name, _type, value); });
            }
        }

        public void Clear()
        {
            foreach (var item in dic_elements)
            {
                Destroy(dic_elements[item.Key].gameObject);
            }
            dic_elements.Clear();
        }

        public void UpdateElement(string _elementID, UIElementInfor _elementInfor)
        {
            if (dic_elements.ContainsKey(_elementID))
            {
                InitElementInfor(dic_elements[_elementID], _elementInfor);
            }
        }
        public void Remove(string _elementID)
        {
            if (dic_elements.ContainsKey(_elementID))
            {
                Destroy(dic_elements[_elementID].gameObject);
                dic_elements.Remove(_elementID);
            }
        }

        public void SetVisable(List<string> _list)
        {
            foreach (var item in dic_elements)
            {
                if (_list.Contains(item.Key))
                {
                    item.Value.gameObject.SetActive(true);
                }
                else
                {
                    item.Value.gameObject.SetActive(false);
                }
            }
        }

        private void OnElementClick(string _name, string _type)
        {
            if (onElementClick != null)
            {
                onElementClick(_name, _type);
            }
        }

        private void OnElementSelect(string _name, string _type, bool _isOn)
        {
            if (_isOn && onElementSelect != null)
            {
                onElementSelect(_name, _type);
            }
        }

        private void OnElementSlider(string _name, string _type, float _value)
        {
            onElementSlider(_name, _type, _value);
        }
    }

    /// <summary>
    /// UI元素信息
    /// </summary>
    public struct UIElementInfor
    {
        public string Name;
        public Dictionary<string, Color> ColorParameters;
        public Dictionary<string, string> TextParameters;
        public Dictionary<string, Sprite> ImageParameters;
        public Dictionary<string, bool> TriggerEnable;
    }
}