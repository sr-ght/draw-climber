using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Arm
{
    [SerializeField] private Transform _transform;
    private List<GameObject> _bones = new List<GameObject>();

    public Transform Transform => _transform;
    public List<GameObject> Bones { get => _bones; set => _bones = value; }

    public GameObject CutArm(GameObject bone)
    {
        if (_bones.Any(b => b == bone))
        {
            var lastBone = _bones.Last(b => b.activeSelf);
            lastBone.SetActive(false);
            return lastBone;
        }
        return null;
    }
}
