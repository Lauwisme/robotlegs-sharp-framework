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
using System.Collections.Generic;
using swiftsuspenders.dependencyproviders;
using robotlegs.bender.framework.api;
using swiftsuspenders.mapping;

public class SingletonFactory
{
	/*============================================================================*/
	/* Public Properties                                                          */
	/*============================================================================*/

	public Dictionary<MappingId, object> SingletonInstances
	{
		get
		{
			return singletonInstances;
		}
	}

	/*============================================================================*/
	/* Private Properties                                                         */
	/*============================================================================*/

	private IInjector _injector;
	
	private Dictionary<DependencyProvider, MappingId> dependencyMappingIds = new Dictionary<DependencyProvider, MappingId>();
	
	private Dictionary<MappingId, object> singletonInstances = new Dictionary<MappingId, object>();

	/*============================================================================*/
	/* Constructor                                                                */
	/*============================================================================*/

	public SingletonFactory (IInjector injector)
	{
		_injector = injector;
		_injector.PostMappingChange += PostMappingChange;
	}
	
	/*============================================================================*/
	/* Public Functions                                                           */
	/*============================================================================*/

	public void Destroy()
	{
		_injector.PostMappingChange -= PostMappingChange;
		_injector = null;
		foreach (DependencyProvider dp in dependencyMappingIds.Keys) 
		{
			if (dp is SingletonProvider)
			{
				dp.PostApply -= HandlePostApply;
				dp.PreDestroy -= HandlePreDestroy;
			}
		}
		dependencyMappingIds.Clear ();
		singletonInstances.Clear ();
	}
	
	/*============================================================================*/
	/* Private Functions                                                          */
	/*============================================================================*/
	
	private void PostMappingChange (MappingId mappingId, InjectionMapping mapping)
	{
		DependencyProvider dp = mapping.GetProvider ();
		if (dp is SingletonProvider) 
		{
			dp.PostApply += HandlePostApply;
			dependencyMappingIds [dp] = mappingId;
		} 
		else if (dp is ValueProvider) 
		{
			AddSingleton(mappingId, _injector.GetInstance(mappingId.type, mappingId.key));
		}
	}

	private void HandlePostApply (DependencyProvider dp, object obj)
	{
		AddSingleton (dependencyMappingIds [dp], obj);
		dp.PostApply -= HandlePostApply;
		dp.PreDestroy += HandlePreDestroy;
	}
	
	private void HandlePreDestroy(DependencyProvider dp, object obj)
	{
		dp.PostApply -= HandlePreDestroy;
		RemoveSingleton (dependencyMappingIds [dp]);
		dependencyMappingIds.Remove (dp);
	}
	
	private void AddSingleton(MappingId mappingId, object singleton)
	{
		singletonInstances [mappingId] = singleton;
	}
	
	private void RemoveSingleton(MappingId mappingId)
	{
		singletonInstances.Remove (mappingId);
	}
}