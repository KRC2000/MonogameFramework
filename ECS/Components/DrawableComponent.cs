using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Framework.ECS.Components
{
	/// <summary>
	/// Recomends:<br></br>
	/// Framework.ECS.Components.TransformComponent;<br></br>
	/// </summary>
	class DrawableComponent : Base
	{
		public Texture2D texture = null;
		public TransformComponent tc = null;
		public bool UsingSourceRect = false;
		public Rectangle sourceRect;

		
		public DrawableComponent() { base.Type = typeof(DrawableComponent); }

		public void Draw(SpriteBatch _batch)
		{
			if (tc == null)
				Owner.TryGetComponent<TransformComponent>(out tc);


			if (tc != null)
			{
				if (!UsingSourceRect) _batch.Draw(texture, tc.Pos, Color.White);
				else _batch.Draw(texture, tc.Pos, sourceRect ,Color.White);
			}
			else
			{
				if (!UsingSourceRect) _batch.Draw(texture, new Vector2(0, 0), Color.White);
				else _batch.Draw(texture, new Vector2(0, 0), sourceRect, Color.White);
			}
		}

		public void Draw(SpriteBatch _batch, Vector2 size)
		{
			if (tc == null)
				Owner.TryGetComponent<TransformComponent>(out tc);
			
			if (tc != null)
			{
				if (!UsingSourceRect) _batch.Draw(texture, new Rectangle((int)tc.Pos.X, (int)tc.Pos.Y, (int)size.X, (int)size.Y), Color.White);
				else _batch.Draw(texture, new Rectangle((int)tc.Pos.X, (int)tc.Pos.Y, (int)size.X, (int)size.Y), sourceRect,Color.White);
			}
			else
			{
				if (!UsingSourceRect) _batch.Draw(texture, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
				else _batch.Draw(texture, new Rectangle(0, 0, (int)size.X, (int)size.Y), sourceRect, Color.White);
			}
		}

		protected override void VerifyRequiredComponents(){}
	}
}
