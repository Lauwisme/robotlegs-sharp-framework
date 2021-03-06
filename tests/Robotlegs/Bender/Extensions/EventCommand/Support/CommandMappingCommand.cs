//------------------------------------------------------------------------------
//  Copyright (c) 2014-2016 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using Robotlegs.Bender.Extensions.EventManagement.Impl;
using Robotlegs.Bender.Extensions.EventCommand.API;
using Robotlegs.Bender.Extensions.EventManagement.API;

namespace Robotlegs.Bender.Extensions.EventCommand.Support
{
	public class CommandMappingCommand
	{
		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		[Inject]
		public IEvent evt;

		[Inject("nestedCommand")]
		public Type commandClass;

		[Inject]
		public IEventCommandMap eventCommandMap;

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		public void Execute()
		{
			eventCommandMap.Map(evt.type, typeof(IEvent)).ToCommand(commandClass);
		}
	}
}

