using UnityEngine;
using System.Collections.Generic;

namespace BeetrootLab.Features
{

    public class AnchorVisualiser : MonoBehaviour
    {
        private enum AnchorVisualiserStatus { Locked = 0, Available = 1, Completed = 2 };

        [SerializeField]
        private AnchorVisualiserStatus anchorStatus = AnchorVisualiserStatus.Locked;

        private MeshRenderer _meshRenderer;
        private SplineMap _splineMap;
        private Camera _camera;

        [SerializeField]
        private bool isSelected;

        private AnchorVisualiser[] _otherAnchorVisualisers;

        public bool Selected
        {
            get { return isSelected; }
            set
            {
                if (anchorStatus == AnchorVisualiserStatus.Locked) return;

                if (value)
                {
                    if(_otherAnchorVisualisers == null)
                    {
                        //Anchor[] anchors = 
                        AnchorVisualiser[] anchorVisualisers = FindObjectsOfType<AnchorVisualiser>();
                        List<AnchorVisualiser> anchorVisualisersList = new List<AnchorVisualiser>(anchorVisualisers);
                        anchorVisualisersList.RemoveAll(item => item == this);

                        _otherAnchorVisualisers = anchorVisualisersList.ToArray();
                    }

                    foreach (AnchorVisualiser visualiser in _otherAnchorVisualisers)
                        visualiser.Selected = false;

                }
                isSelected = value;
            } 
        }

        void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _splineMap = transform.root.GetComponent<SplineMap>();
            _camera = _splineMap.transform.GetComponentInChildren<Camera>();
        }

        void Update()
        {
            if (CanAssignMaterial())
            {
                TryToSelectMe();
            }

            AssignMaterial();
        }

        private void TryToSelectMe()
        {
            if (anchorStatus != AnchorVisualiserStatus.Locked)
            {
                Selected = true;
            }
        }

        /// <summary>
        /// This determines if player is pressing anchor visualiser
        /// </summary>
        /// <returns></returns>
        private bool CanAssignMaterial()
        {
            bool result = false;

            Vector2 touchPos = Vector2.zero;

            if (Application.isMobilePlatform)
            {
                if (Input.touchCount > 0)
                    touchPos = Input.GetTouch(0).position;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                    touchPos = Input.mousePosition;
            }

            if (touchPos != Vector2.zero)
            {
                Ray ray = _camera.ScreenPointToRay(touchPos);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                    result = hit.collider.gameObject == gameObject;
            }

            return result;
        }

        /// <summary>
        /// Assigns material depending on current AnchorVisualiserStatus or Selection
        /// </summary>
        private void AssignMaterial()
        {
            if (Selected)
            {
                ApplySharedMaterial(_splineMap.AnchorSelectedMaterial);
            }
            else
            {
                switch (anchorStatus)
                {
                    case AnchorVisualiserStatus.Locked:
                        ApplySharedMaterial(_splineMap.AnchorLockedMaterial);
                        break;
                    case AnchorVisualiserStatus.Available:
                        ApplySharedMaterial(_splineMap.AnchorAvailableMaterial);
                        break;
                    case AnchorVisualiserStatus.Completed:
                        ApplySharedMaterial(_splineMap.AnchorAvailableMaterial);
                        break;
                }
            }
        }

        private void ApplySharedMaterial(Material mat)
        {
            if(_meshRenderer.sharedMaterial != mat)
                _meshRenderer.sharedMaterial = mat;
        }    
    }
}
