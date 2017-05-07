using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DungeoneerAssets
{
    public abstract class PaginationBase<T> : MonoBehaviour
    {
        public int itemsPerPage = 8;
        public Transform stockPanel;
        protected int page;
        public GameObject btnNextPage;
        public GameObject btnPrevPage;
        public GameObject itemTemplatePrefab;
        protected List<T> content;

        protected virtual void AddItemsToPanel(List<T> items)
        {
            foreach (T def in items)
            {
                GameObject itemSlot = GameObject.Instantiate(this.itemTemplatePrefab) as GameObject;
                if (itemSlot != null)
                {
                    itemSlot.name = this.GetName(def) + "panel";
                    this.TemplateInitialize(itemSlot, def);
                    itemSlot.transform.SetParent(this.stockPanel);
                    itemSlot.transform.localEulerAngles = Vector3.zero;
                    ((RectTransform)itemSlot.transform).anchoredPosition3D = new Vector3(((RectTransform)itemSlot.transform).anchoredPosition3D.x,
                        ((RectTransform)itemSlot.transform).anchoredPosition3D.y, 0);
                    itemSlot.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        protected virtual void TemplateInitialize(GameObject templateInstance, T item)
        { }

        protected abstract void LoadContent();

        protected virtual List<T> GetCurrentPageContent()
        {
            return this.content
                .Skip(itemsPerPage * (this.page - 1))
                .Take(itemsPerPage)
                .ToList();
        }

        protected abstract void DisposeItem(Transform item);


        protected abstract string GetName(T item);

        protected virtual bool NextPageAvailable()
        {
            return this.content.Count > itemsPerPage * this.page;
        }

        protected virtual bool PrevPageAvailable()
        {
            return this.page > 1;
        }

        public void OnNextPage()
        {
            if (this.NextPageAvailable())
            {
                this.page++;
                this.LoadPage();
                this.btnNextPage.SetActive(this.NextPageAvailable());
                this.btnPrevPage.SetActive(this.PrevPageAvailable());
            }
        }

        public void OnPreviousPage()
        {
            if (this.PrevPageAvailable())
            {
                if (this.page > 1)
                {
                    this.page--;
                    this.LoadPage();
                    this.btnNextPage.SetActive(this.NextPageAvailable());
                    this.btnPrevPage.SetActive(this.PrevPageAvailable());
                }
            }
        }

        protected void LoadPage()
        {
            this.Clear();
            List<T> defs = this.GetCurrentPageContent();
            if (defs != null && defs.Count > 0)
                this.AddItemsToPanel(defs);
        }

        protected void Clear()
        {
            foreach (Transform child in this.stockPanel)
            {
                this.DisposeItem(child);
                GameObject.Destroy(child.gameObject);
            }
        }


        protected virtual void OnDisable()
        {
            this.Clear();
        }

        protected virtual void OnEnable()
        {
            this.page = 1;
            this.StartCoroutine("Init");
        }

        protected void UpdatePaginationButtonsVisibility()
        {
            this.btnNextPage.SetActive(this.NextPageAvailable());
            this.btnPrevPage.SetActive(this.PrevPageAvailable());
        }

        private IEnumerator Init()
        {
            this.LoadContent();
            if (this.content != null && this.content.Count > 0)
            {
                yield return this.StartCoroutine("MockItems");
                this.UpdatePaginationButtonsVisibility();
                this.LoadPage();
            }
        }

        private int GetNumPages()
        {
            double val = (double)this.content.Count() / (double)this.itemsPerPage;
            val = Math.Ceiling(val);

            return (int)val;
        }

        protected virtual void Refresh()
        {
            this.LoadContent();
            this.UpdatePaginationButtonsVisibility();
            this.LoadPage();
        }

        protected void GoToLastPage()
        {
            this.page = this.GetNumPages();
            this.UpdatePaginationButtonsVisibility();
            this.LoadPage();
        }

        private IEnumerator MockItems()
        {

            this.AddItemsToPanel(new List<T> { this.content.First() });
            yield return new WaitForEndOfFrame();

            float containerHeight = ((RectTransform)this.stockPanel).GetHeight();

            float childHeight = 0;
            RectTransform childPanel = this.stockPanel.GetChild(0) as RectTransform;
            if (childPanel != null)
                childHeight = childPanel.GetHeight();

            int numCols = 2;
            GridLayoutGroup gridLayout = this.stockPanel.GetComponent<GridLayoutGroup>();
            if (gridLayout != null)
            {
                containerHeight -= (gridLayout.padding.top + gridLayout.padding.bottom);
                childHeight += gridLayout.spacing.y;
                if (gridLayout.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
                    numCols = gridLayout.constraintCount;

                // #items ignoring spacing between items
                int tmpVertItems = (int)(containerHeight / childHeight);

                this.itemsPerPage = tmpVertItems * numCols;
            }
            this.Clear();
        }
    }
}