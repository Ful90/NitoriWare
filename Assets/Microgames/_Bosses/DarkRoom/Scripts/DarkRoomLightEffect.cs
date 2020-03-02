﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomLightEffect : MonoBehaviour
{
    public static Transform lampTransformSingleton;
    public static Transform cursorTransformSingleton;

    [SerializeField]
    private bool useRadiusBoost;
    [Header("Singleton values only necessary in one instance")]
    [SerializeField]
    private Transform lampTransform;
    [SerializeField]
    private Transform cursorTransform;

    private Renderer rend;

	void Start()
    {
        rend = GetComponent<Renderer>();
        if (lampTransform != null)
            lampTransformSingleton = lampTransform;
        if (cursorTransform != null)
            cursorTransformSingleton = cursorTransform;
    }
	
	void LateUpdate()
    {
        updateValues();
    }

    void updateValues()
    {
        var material = rend.material;
        material.SetVector("_LampPos", lampTransformSingleton.position);
        material.SetVector("_CursorPos", cursorTransformSingleton.position);
        material.SetFloat("_LampAnim", DarkRoomEffectAnimationController.instance.lampBoost);
        if (useRadiusBoost)
            material.SetFloat("_LampRadiusBoost", DarkRoomEffectAnimationController.instance.radiusBoost);
        material.SetFloat("_CursorAnim", DarkRoomEffectAnimationController.instance.cursorBoost);
    }
}
