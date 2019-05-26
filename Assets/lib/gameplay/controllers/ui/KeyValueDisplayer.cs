using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sesim.Game.Controllers
{
    public class KeyValueDisplayer : MonoBehaviour
    {
        public string title;

        public IList<KeyValueDisplayElement> elements;

        RectTransform sizeConstraints;

        public Font font;

        public int fontSize;
        public Color labelColor;
        public Color contentColor;

        public void Start()
        {
            sizeConstraints = this.GetComponent<RectTransform>();
        }

        public void Construct(IList<KeyValueDisplayElement> dataSource)
        {
            // Skip when lists are equal; this is most cases
            // if (dataSource == elements) return;

            DestroyAllChildren(this.GetComponent<RectTransform>());

            var titleObject = BuildTitleObject();
            titleObject
                .GetComponent<RectTransform>()
                .SetParent(this.GetComponent<RectTransform>());

            // Else we construct a new gameobject tree
            elements = dataSource;
            var len = elements.Count;
            for (int i = 0; i < len; i++)
            {
                elements[i].contentColor = elements[i].contentColor ?? contentColor;
                elements[i].labelColor = elements[i].labelColor ?? labelColor;
                elements[i].font = elements[i].font ?? font;
                if (elements[i].fontSize <= 0) elements[i].fontSize = fontSize;

                var gameObj = elements[i].ConstructGameobject(sizeConstraints);
                // if this component doesnt have layout element
                var transform = gameObj.GetComponent<RectTransform>();
                if (transform == null)
                    throw new Exception("The gameobject is not a UI element");

                gameObj.name = $"{title}_Obj";
                transform.SetParent(this.gameObject.GetComponent<RectTransform>(), false);
                elements[i].ApplyOtherConstraints(gameObj);
            }
        }

        GameObject BuildTitleObject()
        {
            var titleObject = new GameObject($"{this.title}_Key", typeof(LayoutElement), typeof(CanvasRenderer), typeof(Text));
            var text = titleObject.GetComponent<Text>();
            text.text = this.title;
            text.font = font;
            text.fontSize = fontSize;
            text.color = labelColor;
            text.fontStyle = FontStyle.Bold;
            return titleObject;
        }

        public void Update()
        {
        }

        static void DestroyAllChildren(Transform src)
        {
            var childCount = src.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var childZero = src.GetChild(i);
                childZero.SetParent(null);
                Destroy(childZero.gameObject);
            }
        }
    }

    public class KeyValueDisplayElement
    {
        public KeyValueDisplayElement(string title, string content)
        {
            this.title = title;
            this.content = content;
        }

        public bool dirty = false;

        protected string title;
        public string Title
        {
            get => title;
            set { title = value; dirty = true; }
        }

        protected string content;
        public string Content
        {
            get => content;
            set { content = value; dirty = true; }
        }

        public Font font = null;

        public int fontSize = -1;
        public Color? labelColor = null;
        public Color? contentColor = null;

        public virtual GameObject ConstructGameobject(RectTransform sizeConstraints)
        {
            var rootObject = new GameObject($"{title}_Displayer", typeof(RectTransform), typeof(LayoutElement));
            var rootLayout = rootObject.GetComponent<LayoutElement>();
            rootLayout.preferredHeight = 12;
            var rootTransform = rootObject.GetComponent<RectTransform>();

            var label = new GameObject($"{title}_Key", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            var labelText = label.GetComponent<Text>();
            labelText.text = title;
            labelText.font = font;
            labelText.fontSize = fontSize;
            labelText.color = labelColor.Value;

            var labelTransform = label.GetComponent<RectTransform>();
            labelTransform.anchorMin = new Vector2(0f, 0f);
            labelTransform.anchorMax = new Vector2(0.333f, 1f);
            labelTransform.offsetMax = new Vector2();
            labelTransform.offsetMin = new Vector2();
            labelTransform.SetParent(rootTransform, false);

            var value = new GameObject($"{title}_Val", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            var valueText = value.GetComponent<Text>();
            valueText.text = content;
            valueText.font = font;
            valueText.fontSize = fontSize;
            valueText.color = contentColor.Value;

            var valueTransform = value.GetComponent<RectTransform>();
            valueTransform.anchorMin = new Vector2(0.333f, 0f);
            valueTransform.anchorMax = new Vector2(1f, 1f);
            // valueTransform.
            valueTransform.offsetMax = new Vector2();
            valueTransform.offsetMin = new Vector2();
            valueTransform.SetParent(rootTransform, false);

            dirty = false;
            return rootObject;
        }

        public virtual void ApplyOtherConstraints(GameObject obj)
        {

        }

        public override bool Equals(object obj)
        {
            return obj is KeyValueDisplayElement element &&
                   title == element.title && content == element.content;
        }


        private int hashCode = -1;
        public override int GetHashCode()
        {
            if (!dirty) return hashCode;

            hashCode = 375585649 + EqualityComparer<string>.Default.GetHashCode(title);
            hashCode += 375585649 + EqualityComparer<string>.Default.GetHashCode(content);
            return hashCode;
        }
    }
}
