using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Framework.ECS.Components
{
	/// <summary>
	/// Requires:<br></br>
	/// Framework.ECS.Components.DrawableComponent;<br></br>
	/// </summary>
	class AnimationComponent : Base, IUpdateable
	{	
        public DrawableComponent dc = null;
		Dictionary<string,Animation> animations = new Dictionary<string, Animation>();
		public float frame = 0;
		public bool isPlaying = false;
		public bool once = false;
		public Animation currentAnim;

		public AnimationComponent() { base.Type = typeof(AnimationComponent); }

        public void AddAnimation(string animationName, Rectangle firstFrame, int rows, int columns, int frameCount, float frameTime)
        {
            animations.Add(animationName, new Animation(){firstFrame = firstFrame, rows = rows, columns = columns, frameCount = frameCount, frameTime = frameTime});
        }

		public void Play(string animName)
		{
			VerifyRequiredComponents();

			if (animations.TryGetValue(animName, out currentAnim))
			{
                isPlaying = true;
			}
			else throw new Exception($"There is no such animation: {animName}");
		}

		public void PlayOnce(string animName)
		{
			VerifyRequiredComponents();

			if (animations.TryGetValue(animName, out currentAnim))
			{
				frame = 0;
                isPlaying = true;
				once = true;
			}
			else throw new Exception($"There is no such animation: {animName}");
		}

		public void Stop()
		{
			isPlaying = false;
			once = false;
		}

		public void Update()
		{
			VerifyRequiredComponents();

			if (!dc.UsingSourceRect) dc.UsingSourceRect = true;

			if (isPlaying)
			{
				frame += currentAnim.frameTime;


				if ((int)frame >= currentAnim.frameCount-1)
				{
                    if (once) Stop();
                    if ((int)frame >= currentAnim.frameCount)
					{
                        if (!once) frame = 0;
                    }
				}

				Rectangle source = new Rectangle(currentAnim.firstFrame.X + ((int)frame - ((int)(frame / currentAnim.columns) * currentAnim.columns)) * currentAnim.firstFrame.Width,
												currentAnim.firstFrame.Y + (int)frame / currentAnim.columns * currentAnim.firstFrame.Height,
												currentAnim.firstFrame.Width, currentAnim.firstFrame.Height);

				dc.sourceRect = source;
			}
		}

        protected override void VerifyRequiredComponents()
		{
			if (dc == null)
			{
				if (!Owner.TryGetComponent<DrawableComponent>(out dc))
					throw new Exception($"{this.GetType()}: Entity does not has required component for system correct execution. Missing component - {typeof(DrawableComponent)}");
			}
		}
	}

	struct Animation
	{
		public Rectangle firstFrame;
		public int rows; 
		public int columns; 
		public int frameCount;
		public float frameTime;
	}
}
