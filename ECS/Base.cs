using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.ECS
{
	abstract class Base
	{
		public Type Type { get; set; }
		public Entity Owner { get; set; }

		protected abstract void VerifyRequiredComponents();
	}
}
