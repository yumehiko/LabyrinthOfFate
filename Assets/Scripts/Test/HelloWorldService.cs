using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko
{
	public class HelloWorldService
	{
		private int count = 0;

		public void Hello(string message = "Hello world")
		{
			UnityEngine.Debug.Log(message + count);
			count++;
		}
	}
}