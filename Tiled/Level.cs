using System;
using System.IO;
using System.Collections.Generic;

using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Framework.Tiled;

namespace Framework
{
    class Level
    {
        public Map Map { get; private set; }
        public Dictionary<string, Tileset> Tilesets { get; private set; }
        public Layer ObstacleTileLayer { get; private set; } = null;

        private Texture2D stampTexture = null;

        public Level(string levelFilePath)
        {
            // Deserialise level xml and all tileset files that get referenced in to LevelFile
            using (StreamReader reader = new StreamReader(levelFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Map));
                Map = (Map)serializer.Deserialize(reader);

                Tilesets = new Dictionary<string, Tileset>();

                if (Map.Tileset_refs != null)
                {
                    foreach (var tileset_ref in Map.Tileset_refs)
                    {

                        using (StreamReader reader1 = new StreamReader(Path.Join("Content", tileset_ref.Source_tsx)))
                        {
                            XmlSerializer serializer1 = new XmlSerializer(typeof(Tileset));
                            Tileset newts = (Tileset)serializer1.Deserialize(reader1);
                            newts.this_ref = tileset_ref;
                            Tilesets.Add(newts.Name, newts);
                        }
                    }
                }
            }

            // Map data can be only read as a string, so
            // converting long ass string ("..0, 0, 0, 1, 1, 0,\n..") into 2D array that Layer objects will own.
            // Also assigning obstacle layer var
            if (Map.Layers != null)
            {
                foreach (var layer in Map.Layers)
                {
                    List<string> rows = new List<string>();
                    rows.AddRange(layer.Data_str.Split("\n"));
                    rows.RemoveAt(0);
                    rows.RemoveAt(rows.Count - 1);

                    layer.Data = new uint[layer.Height, layer.Width];
                    for (int i = 0; i < rows.Count; i++)
                    {
                        List<string> row = new List<string>();
                        row.AddRange(rows[i].Split(","));
                        row.RemoveAt(row.Count - 1);

                        for (int j = 0; j < row.Count; j++)
                        {
                            layer.Data[i, j] = uint.Parse(row[j]);
                        }
                    }

                    if (layer.Name == "Obstacles") ObstacleTileLayer = layer;
                }
            }

            // Convert string with object point coordinates into Vector2 list
            foreach (ObjectGroup group in Map.ObjectGroups)
            {
                foreach (Framework.Tiled.Object obj in group.Objects)
                {
                    obj.Polygon.Points = new List<Vector2>();

                    string[] vec_str = obj.Polygon.Points_str.Split(' ');
                    foreach (string s in vec_str)
                    {
                        string[] xy = s.Split(',');

                        obj.Polygon.Points.Add(new Vector2((float)Convert.ToDecimal(xy[0]), 
                                                            (float)Convert.ToDecimal(xy[1])));
                    }
                }
            }
        }

        public void LoadTilesets(ContentManager content)
        {
            foreach (var tileset in Tilesets)
            {
                tileset.Value.Image.Texture = content.Load<Texture2D>(Path.GetFileNameWithoutExtension(tileset.Value.Image.Source_png));
            }
        }

        public void Draw(SpriteBatch _spriteBatch, Camera camera, GraphicsDevice device, Rectangle viewarea, Point tileSize)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.GetTransform(device.Viewport));

            foreach (var layer in Map.Layers)
            {
                for (int y = 0; y < layer.Width; y++)
                {
                    for (int x = 0; x < layer.Height; x++)
                    {
                        Vector2 pos = new Vector2(y * tileSize.X, x * tileSize.Y);

                        if (viewarea.Contains(pos))
                        {
                            uint tileValue = layer.Data[x, y];
                            Rectangle sourceRect = new Rectangle();
                            if (tileValue != 0)
                            {
                                foreach (var tileset in Tilesets)
                                {
                                    // if tile value is in the tile range of current tileset -> pick and assign texture used by this tileset
                                    if (tileValue >= tileset.Value.this_ref.FirstElementId && tileValue < tileset.Value.this_ref.FirstElementId + tileset.Value.TileCount)
                                    {
                                        stampTexture = tileset.Value.Image.Texture;

                                        int r_x = (int)(tileValue - tileset.Value.this_ref.FirstElementId) % tileset.Value.Columns;
                                        int r_y = (int)(tileValue - tileset.Value.this_ref.FirstElementId) / tileset.Value.Columns;
                                        sourceRect = new Rectangle(1 + (2 * r_x) + r_x * tileset.Value.TileWidth, 1 + (2 * r_y) + r_y * tileset.Value.TileHeight, tileset.Value.TileWidth, tileset.Value.TileHeight);
                                    }
                                }

                                _spriteBatch.Draw(stampTexture, new Rectangle((int)pos.X, (int)pos.Y, tileSize.X, tileSize.Y), sourceRect, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                            }
                        }
                    }
                }
            }


            _spriteBatch.End();
        }

        public bool IsPassable(Point tilePos)
        {
            if (ObstacleTileLayer.Data[tilePos.Y, tilePos.X] == 0) return true;
            else return false;
        }
    }

}
