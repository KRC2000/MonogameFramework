namespace Collisio
{
    public static class Collision
    {
        //
        public static float GetVecLengthSquared(float vecX, float vecY)
        {
            return (vecX * vecX) + (vecY * vecY);
        }

        public static float GetDotProduct(float vec1X, float vec1Y,
                                          float vec2X, float vec2Y)
        {
            return (vec1X * vec2X) + (vec1Y * vec2Y);
        }

        /// <summary>
        /// Checks if point is located inside of a axis aligned bounding box, calculates a normalized 
        /// vector that points in the direction that point should move to stop intersection
        /// as soon as posible.
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="upperLeftX">X component of upper left corner of AABB</param>
        /// <param name="upperLeftY">Y component of upper left corner of AABB</param>
        /// <param name="width">AABB width</param>
        /// <param name="height">AABB height</param>
        /// <param name="pushVecX">X component of the vector that points in the direction
        /// where point should move to avoid collision as soon as polible</param>
        /// <param name="pushVecY">Y component of the vector that points in the direction
        /// where point should move to avoid collision as soon as polible</param>
        /// <returns>true if point is inside of the axis aligned bounding box</returns>
        public static bool IsPointInsideAABB(float pointX, float pointY,
                                             float upperLeftX, float upperLeftY,
                                             float width, float height,
                                             out float pushVecX,
                                             out float pushVecY)
        {
            pushVecX = pushVecY = 0;
            // Calculate a vector from upper-left aabb corner to the point
            float xdif = pointX - upperLeftX;
            float ydif = pointY - upperLeftY;

            // If point coordinates lie in AABB bounds, an actual collision check
            if (xdif > 0 && xdif <= width &&
                ydif > 0 && ydif <= height)
            {
                // Check distances from a point to every AABB edge, choose the shortest one
                // and assign pushVec values accordingly
                float shortest = xdif;
                pushVecX = -1;
                if (width - xdif < shortest) { shortest = width - xdif; pushVecX = 1; pushVecY = 0; }
                if (ydif < shortest) { shortest = ydif; pushVecX = 0; pushVecY = -1; }
                if (height - ydif < shortest) { shortest = height - ydif; pushVecX = 0; pushVecY = 1; }
  
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if point is located inside of a convex shape defined by points.
        /// </summary>
        /// <param name="points">Array of points that form a convex shape. Should be filled with 
        /// coordinates of points like so: [x, y, x1, y1, x2, y2...]</param>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <returns>true if point is inside of the convex shape</returns>
        public static bool IsPointInsideConvexShape(float[] points, 
                                                    float pointX,
                                                    float pointY)
        {
            bool result = true;
            float segVecX, segVecY, pointVecX, pointVecY;

            // Check if point is located on the right side of every line segment of the shape
            for (int i = 3; i < points.Length; i+=2)
            {
                segVecX = points[i - 1] - points[i - 3];
                segVecY = points[i] - points[i - 2];
                pointVecX = pointX - points[i - 3];
                pointVecY = pointY - points[i - 2];

                if (GetDotProduct(pointVecX, pointVecY, -segVecY, segVecX) < 0) result = false;
            }

            // Check if point is located on the right side of the last line segment that
            // connects last and first points in the shape
            segVecX = points[0] - points[points.Length - 2];
            segVecY = points[1] - points[points.Length - 1];
            pointVecX = pointX - points[points.Length - 2];
            pointVecY = pointY - points[points.Length - 1];
            if (GetDotProduct(pointVecX, pointVecY, -segVecY, segVecX) < 0) result = false;
            
            return result;
        }

        public static bool IsCircleIntersectsConvexShape(float[] points, 
                                                         float circleCenterX,
                                                         float circleCenterY,
                                                         float radius,
                                                         out float pushVecX,
                                                         out float pushVecY)
        {
            pushVecX = pushVecY = 0;
            return false;
        }
        /// <summary>
        /// Checks if point is located inside of a circle, calculates a vector
        /// that points in the direction that circle should move to stop intersection
        /// as soon as posible. <br/>
        /// CAUTION!!! Calculated vector will not be a unit vector! Its length will be random,
        /// you should normalize it before using!
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="circleCenterX"></param>
        /// <param name="circleCenterY"></param>
        /// <param name="radius"></param>
        /// <param name="pushVecX"> X component of the vector that points in the direction
        /// where circle should move to avoid collision as soon as polible </param>
        /// <param name="pushVecY">Y component of the vector that points in the direction
        /// where circle should move to avoid collision as soon as polible</param>
        /// <returns>true if point is inside of the circle</returns>
        public static bool IsPointInsideCircle(float pointX, float pointY,
                                               float circleCenterX,
                                               float circleCenterY,
                                               float radius,
                                               out float pushVecX,
                                               out float pushVecY)
        {
            pushVecX = pushVecY = 0;

            // Calculate vector from circle center to point
            float vx = pointX - circleCenterX;
            float vy = pointY - circleCenterY;

            // If the vector between circle center and point is longer 
            // than radius - point is located inside circle
            if (GetVecLengthSquared(vx, vy) <= radius * radius)
            {
                pushVecX = -vx; pushVecY = -vy;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if circle intersects with a triangle. Calculates a vector
        /// that points in the direction that circle should move to stop intersection
        /// as soon as posible. <br/>
        /// CAUTION!!! Calculated vector will not be a unit vector! Its length will be random,
        /// you should normalize it before using! <br/>
        /// CAUTION!!! Triangle points must be passed in clockwise order! <br/>
        /// </summary>
        /// <param name="point1X"></param>
        /// <param name="point1Y"></param>
        /// <param name="point2X"></param>
        /// <param name="point2Y"></param>
        /// <param name="point3X"></param>
        /// <param name="point3Y"></param>
        /// <param name="circleCenterX"></param>
        /// <param name="circleCenterY"></param>
        /// <param name="radius">Circle radius</param>
        /// <param name="pushVecX">X component of the vector that points in the direction
        /// where circle should move to avoid collision as soon as polible</param>
        /// <param name="pushVecY">Y component of the vector that points in the direction
        /// where circle should move to avoid collision as soon as polible</param>
        /// <param name="checkFullOverlap"> should method also check if a circle is completely 
        /// inside of a triangle? By default - false, which means that if circle is entirely inside 
        /// of a triangle - no intersection will be detected</param>
        /// <returns>true if circle is intersecting with a triangle</returns>
        public static bool IsCircleIntersectsTriangle(float point1X, float point1Y,
                                                      float point2X, float point2Y,
                                                      float point3X, float point3Y,
                                                      float circleCenterX,
                                                      float circleCenterY,
                                                      float radius,
                                                      out float pushVecX,
                                                      out float pushVecY,
                                                      bool checkFullOverlap = false)
        {
            if (IsPointInsideCircle(point1X, point1Y, circleCenterX, circleCenterY, radius,  out pushVecX, out pushVecY) ||
            IsPointInsideCircle(point2X, point2Y, circleCenterX, circleCenterY, radius,  out pushVecX, out pushVecY) ||
            IsPointInsideCircle(point3X, point3Y, circleCenterX, circleCenterY, radius,  out pushVecX, out pushVecY) ||
            IsCircleIntersectsLineSegment(point1X, point1Y, point2X, point2Y, circleCenterX, circleCenterY, radius, out pushVecX, out pushVecY) ||
            IsCircleIntersectsLineSegment(point2X, point2Y, point3X, point3Y, circleCenterX, circleCenterY, radius, out pushVecX, out pushVecY) ||
            IsCircleIntersectsLineSegment(point3X, point3Y, point1X, point1Y, circleCenterX, circleCenterY, radius, out pushVecX, out pushVecY) ||
            (checkFullOverlap && IsPointInsideConvexShape(new float[]{point1X, point1Y, point2X, point2Y, point3X, point3Y}, circleCenterX, circleCenterY)))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if circle intersects with a line segment. calculates a vector
        /// that points in the direction that circle should move to stop intersection
        /// as soon as posible. <br/>
        /// CAUTION!!! Calculated vector will not be a unit vector! Its length will be random,
        /// you should normalize it before using! Only for internal use - when calculating 
        /// collision with shapes.
        /// </summary>
        /// <param name="point1X"></param>
        /// <param name="point1Y"></param>
        /// <param name="point2X"></param>
        /// <param name="point2Y"></param>
        /// <param name="circleCenterX"></param>
        /// <param name="circleCenterY"></param>
        /// <param name="radius">Circle radius</param>
        /// <param name="pushVecX">X component of the vector that points in the direction
        /// where circle should move to avoid collision as soon as polible</param>
        /// <param name="pushVecY">Y component of the vector that points in the direction
        /// where circle should move to avoid collision as soon as polible</param>
        /// <returns>true if circle is intersecting with a line segment</returns>
        private static bool IsCircleIntersectsLineSegment(float point1X, float point1Y,
                                                  float point2X, float point2Y,
                                                  float circleCenterX,
                                                  float circleCenterY,
                                                  float radius,
                                                  out float pushVecX,
                                                  out float pushVecY)
        {
            pushVecX = pushVecY = 0;

            float segmentVecX = point2X - point1X;
            float segmentVecY = point2Y - point1Y;

            float toCenterVecX = circleCenterX - point1X;
            float toCenterVecY = circleCenterY - point1Y;

            float dot = segmentVecX * toCenterVecX + segmentVecY * toCenterVecY;

            pushVecX = segmentVecY * ((dot > 0)? 1 : -1);
            pushVecY = segmentVecX * ((dot > 0)? -1 : 1);

            if (dot > 0)
            {
                float radiusSqr = radius * radius;
                float c1sqr = GetVecLengthSquared(toCenterVecX, toCenterVecY) - radiusSqr;
                float len = GetVecLengthSquared(segmentVecX, segmentVecY);

                if (dot < len)
                {
                    if (c1sqr * len <= dot * dot)
                        return true;
                }
            }

            return false;
        }
    }
}