﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Framework
{
	class Camera
	{
		public float Scale { get; private set; } = 1;
		public float Rotation { get; private set; } = 0;

		/// <summary> Center position of camera, becomes a center after "origin" matrix applyed</summary>
		public Vector2 Position { get; private set; }

		public float MovementSpeed { get; set; } = 1;

		public Camera()
		{
		}

		public void Update(KeyboardState kbState)
		{
			if (kbState.IsKeyDown(Keys.Left)) Move(-MovementSpeed, 0);
            if (kbState.IsKeyDown(Keys.Right)) Move(MovementSpeed, 0);
            if (kbState.IsKeyDown(Keys.Up)) Move(0, -MovementSpeed);
            if (kbState.IsKeyDown(Keys.Down)) Move(0, MovementSpeed);
            if (kbState.IsKeyDown(Keys.PageUp)) AddZoom(0.01f);
            if (kbState.IsKeyDown(Keys.PageDown)) AddZoom(-0.01f);
			if (kbState.IsKeyDown(Keys.Home)) AddRotation(0.1f);
            if (kbState.IsKeyDown(Keys.End)) AddRotation(-0.1f);
		}

		/// <summary>
		///Matrix o = Matrix.CreateTranslation(GetCenter(viewport).X - Position.X, GetCenter(viewport).Y - Position.Y, 0);<br></br>
		///Matrix t = Matrix.CreateTranslation(-Position.X, -Position.Y, 0);<br></br>
		///Matrix s = Matrix.CreateScale(scale);<br></br>
		///Matrix r = Matrix.CreateRotationZ(-rotation * ((float)Math.PI / 180f));<br></br>
		///return t * r * o * s;
		/// </summary>
		/// <param name="viewport"></param>
		/// <returns></returns>
		public Matrix GetTransform(Viewport viewport) => Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
				* Matrix.CreateRotationZ(-Rotation * ((float)Math.PI / 180f)) 
				* Matrix.CreateTranslation(viewport.Width / Scale / 2, viewport.Height / Scale / 2, 0)
				* Matrix.CreateScale(Scale);
		public void SetPos(Vector2 pos)
		{
			Position = pos;
		}
		public void Move(float x, float y)
		{
			Position += new Vector2(x, y);
		}
		public void Move(Vector2 vec)
		{
			Position += vec;
		}

		public void SetZoom(float zoom)
		{
			Scale = zoom;
		}
		public void AddZoom(float zoom)
		{
			Scale += zoom;
		}

		public void SetRotation(float angle)
		{
			Rotation = angle;
		}
		public void AddRotation(float angle)
		{
			Rotation += angle;
		}

		public Rectangle GetViewArea(Viewport viewport)
		{
			return new Rectangle((int)(Position.X - (viewport.Width / Scale / 2)), 
								(int)(Position.Y - (viewport.Height / Scale / 2)), 
								(int)(viewport.Width / Scale), (int)(viewport.Height / Scale));
		}
	}
}

