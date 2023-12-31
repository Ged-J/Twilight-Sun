using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowFillThing : MonoBehaviour
{
    public Image _image;
    public RectTransform _edgeRect;
    public RectTransform _imgRect;
 
    //You can edit this on inspector, to set up the margin.
    public float EdgeMargin = 20;
 
    /// <summary>
    /// If <c>true</c> then the image edge will be hidden when the fill amount comes to 0
    /// </summary>
    public bool HideWhenNotFilled = true;
 
    void Awake()
    {
        //_image = GetComponent<Image>();
        //_imgRect = GetComponent<RectTransform>();
       // _edgeRect = transform.GetChild(0).GetComponent<RectTransform>();
    }
 
    void Update()
    {
        if (_image.type != Image.Type.Filled) return;
 
        _edgeRect.gameObject.SetActive(!(_image.fillAmount == 0  && HideWhenNotFilled));
        _edgeRect.localPosition = new Vector2(_image.fillAmount * _imgRect.rect.width - EdgeMargin,
            _edgeRect.localPosition.y);
    }
}
