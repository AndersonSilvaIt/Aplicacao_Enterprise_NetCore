﻿using MediatR;
using System;

namespace NSE.Core.Messages
{
	public class Event : Message, INotification
	{
		public DateTime TimeStamp { get; set; }

		protected Event()
		{
			TimeStamp = new DateTime();
		}
	}
}
