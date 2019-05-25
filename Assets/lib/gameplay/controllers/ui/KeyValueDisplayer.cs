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

        public void Start()
        {
            sizeConstraints = this.GetComponent<RectTransform>();
        }

        public void Construct(IList<KeyValueDisplayElement> dataSource)
        {
            // Skip when lists are equal; this is most cases
            if (dataSource == elements) return;

            // Else we construct a new gameobject tree
            elements = dataSource;
            var len = elements.Count;
            for (int i = 0; i < len; i++)
            {
                var gameObj = Instantiate(elements[i].GenerateGameObject(sizeConstraints));
                // if this component doesnt have layout element
                var transform = gameObj.GetComponent<RectTransform>();
                if (transform == null)
                    throw new Exception("The gameobject is not a UI element");

                if (gameObj.GetComponent<LayoutElement>() == null)
                {
                    var layout = gameObj.AddComponent<LayoutElement>();
                    // layout.
                }
                gameObj.name = "Test GameObj";
                transform.SetParent(this.gameObject.GetComponent<RectTransform>(), false);

                elements[i].ApplyOtherConstraints(gameObj);
            }
        }
        public void Update()
        {
        }
    }

    public class KeyValueDisplayElement
    {
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

        public Font font;

        public virtual GameObject GenerateGameObject(RectTransform sizeConstraints)
        {
            var rootObject = new GameObject("Test object", typeof(CanvasRenderer), typeof(RectTransform), typeof(Text), typeof(LayoutElement));
            var layouter = rootObject.GetComponent<LayoutElement>();
            layouter.preferredHeight = 32;
            var text = rootObject.GetComponent<Text>();
            text.text = "Test component";
            text.font = font;
            text.fontSize = 8;
            text.color = new Color(1, 1, 1, 1);

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
