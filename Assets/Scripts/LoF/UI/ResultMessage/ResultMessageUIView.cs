using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LoF.UI.ResultMessage
{
    public class ResultMessageUIView : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> messageUIs;

        private readonly List<string> messages = new();

        public void ViewMessage(string message)
        {
            const int margin = 6;
            const int x = 0;
            var sizeX = (int)messageUIs[0].rectTransform.sizeDelta.x;
            var top = 0;

            AddMessage(message);
            for (var i = 0; i < messages.Count; i++)
            {
                messageUIs[i].text = messages[i];
                var height = (int)messageUIs[i].preferredHeight;
                messageUIs[i].rectTransform.sizeDelta = new Vector2Int(sizeX, height);
                messageUIs[i].rectTransform.anchoredPosition = new Vector2Int(x, top);
                top = top - height - margin;
            }
        }

        private void AddMessage(string message)
        {
            messages.Insert(0, message);
            if (messages.Count > messageUIs.Count) messages.RemoveAt(messages.Count - 1);
        }
    }
}