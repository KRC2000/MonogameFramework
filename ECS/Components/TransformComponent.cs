using Microsoft.Xna.Framework;

namespace Framework.ECS.Components
{
	class TransformComponent : Base
	{
		public Vector2 Pos { get; private set; }

		public TransformComponent() { base.Type = typeof(TransformComponent); }

		public void Move(Vector2 vec)
		{
			Pos += vec;
		}

		public void SetPos(Vector2 vec)
		{
			Pos = vec;
		}

		protected override void VerifyRequiredComponents(){}
	}
}
