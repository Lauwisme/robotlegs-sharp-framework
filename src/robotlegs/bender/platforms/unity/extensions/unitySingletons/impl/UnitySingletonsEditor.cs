//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEditor;
using System.Collections.Generic;
using swiftsuspenders.mapping;
using UnityEngine;
using robotlegs.bender.platforms.unity.extensions.monoscriptCache.api;

namespace robotlegs.bender.platforms.unity.extensions.unitySingletons.impl
{
	[CustomEditor(typeof(UnitySingletons))]
	public class UnitySingletonsEditor : Editor
	{
		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/
		
		private bool start;
		
		private UnitySingletons unitySingletons;
		
		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/
		
		private void OnEnable()
		{
			if (start)
				return;

			start = true;
			unitySingletons = target as UnitySingletons;
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public override void OnInspectorGUI ()
		{
			foreach (KeyValuePair<MappingId, object> kvp in unitySingletons.Factory.SingletonInstances) 
			{
				string label = kvp.Key.type.Name;
				if (kvp.Key.key != null)
					label += ": " + kvp.Key.key.ToString();

				MonoScript ms = MonoScriptCache.GetMonoScript(kvp.Value.GetType());
				if (ms != null)
				{
					EditorGUILayout.ObjectField(label, MonoScriptCache.GetMonoScript(kvp.Value.GetType()), typeof(MonoScript), false);
				}
				else
				{
					EditorGUILayout.LabelField(label, kvp.Value.GetType().Name);
				}
			}
		}
	}
}