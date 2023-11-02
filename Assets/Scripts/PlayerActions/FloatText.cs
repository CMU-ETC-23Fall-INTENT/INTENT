using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace INTENT
{
    public class FloatText : MonoBehaviour
    {
        [SerializeField] private float verticalMove;
        [SerializeField] private float aliveTime;
        // Start is called before the first frame update
        public void StartFloat(string text)
        {
            GetComponent<TextMeshPro>().text = text;
            transform.rotation = Camera.main.transform.rotation;
            StartCoroutine(StartText());
        }
        IEnumerator StartText()
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = transform.position + Vector3.up * verticalMove;
            TextMeshPro text = GetComponent<TextMeshPro>();
            float timer = 0;
            while(timer < aliveTime)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - timer / aliveTime);
                transform.position = Vector3.Lerp(startPos, endPos, timer / aliveTime);
                timer += Time.deltaTime;
                yield return null;
            }
            transform.position = endPos;
            Destroy(this.gameObject);
        }
    }
}
