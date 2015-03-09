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

namespace robotlegs.bender.platforms.unity.extensions.unitySingletons.impl
{
	public class SingletonFactory
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		public event Action<MappingId, object> AddedSingleton;

		public event Action<MappingId> RemovedSingleton;

		public Dictionary<MappingId, object> SingletonInstances
		{
			get
			{
				return _singletonInstances;
			}
		}

		/*============================================================================*/
		/* Private Properties                                                         */
		/*============================================================================*/

		private IInjector _injector;
		
		private Dictionary<DependencyProvider, MappingId> _dependencyMappingIds = new Dictionary<DependencyProvider, MappingId>();
		
		private Dictionary<MappingId, object> _singletonInstances = new Dictionary<MappingId, object>();

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		public SingletonFactory (IInjector injector)
		{
			_injector = injector;
			_injector.PostMappingChange += PostMappingChange;
			_injector.PostMappingRemove += PostMappingRemove;
		}
		
		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Destroy()
		{
			_injector.PostMappingChange -= PostMappingChange;
			_injector.PostMappingRemove -= PostMappingRemove;
			_injector = null;
			foreach (DependencyProvider dp in _dependencyMappingIds.Keys) 
			{
				if (dp is SingletonProvider)
				{
					dp.PostApply -= HandlePostApply;
					dp.PreDestroy -= HandlePreDestroy;
				}
			}
			_dependencyMappingIds.Clear ();
			_singletonInstances.Clear ();
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
				_dependencyMappingIds [dp] = mappingId;
			} 
			else if (dp is ValueProvider) 
			{
				AddSingleton(mappingId, _injector.GetInstance(mappingId.type, mappingId.key));
			}
		}
		
		private void PostMappingRemove (MappingId mappingId)
		{
			if (_singletonInstances.ContainsKey (mappingId)) 
			{
				RemoveSingleton(mappingId);
			}
		}

		private void HandlePostApply (DependencyProvider dp, object obj)
		{
			AddSingleton (_dependencyMappingIds [dp], obj);
			dp.PostApply -= HandlePostApply;
			dp.PreDestroy += HandlePreDestroy;
		}
		
		private void HandlePreDestroy(DependencyProvider dp, object obj)
		{
			dp.PostApply -= HandlePreDestroy;
			RemoveSingleton (_dependencyMappingIds [dp]);
			_dependencyMappingIds.Remove (dp);
		}
		
		private void AddSingleton(MappingId mappingId, object singleton)
		{
			_singletonInstances [mappingId] = singleton;
			if (AddedSingleton != null) 
			{
				AddedSingleton(mappingId, singleton);
			}
		}
		
		private void RemoveSingleton(MappingId mappingId)
		{
			_singletonInstances.Remove (mappingId);
			if (RemovedSingleton != null) 
			{
				RemovedSingleton(mappingId);
			}
		}
	}
}