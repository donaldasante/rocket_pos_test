using static rocket_position.Helper;

namespace rocket_position
{
    public class RocketPositioning
    {
        private readonly (int Xstart, int Ystart, int Xend, int Yend) _platformArea = new(5, 5, 0,0);
        private readonly (int Xstart, int Ystart, int Xend, int Yend) _area = new(0, 0, 100, 100);
        private (int X,int Y) _lastCollistionCoordinates = (-1,-1);
        private List<(int X, int Y)> _landedRocketsCoordinates = new List<(int X, int Y)>();

        public List<(int X, int Y)> LandedRocketsCoordinates { get { return _landedRocketsCoordinates; } }

        public RocketPositioning(int platformSize = 10)
        {
            if (platformSize < 0)
            {
                throw new ArgumentOutOfRangeException("platform Size cannot be negative");
            }

            if (platformSize + _platformArea.Xstart > _area.Xend || platformSize + _platformArea.Ystart > _area.Yend)
            {
                throw new Exception("Platform size out of bounds on X or Y coordinate");
            }


            _platformArea.Xend = _platformArea.Xstart + platformSize;
            _platformArea.Yend = _platformArea.Ystart + platformSize;
        }

        public ePositionStatus CheckLandingPlatform(int X, int Y)
        {
            if (IsOnPlatform(X, Y)) 
            {
                foreach (var landedCoord in _landedRocketsCoordinates)
                {
                    var landedCoordCollisionMatrix = RebuildCollisionMatrix(landedCoord.X, landedCoord.Y);
                    if (IsColliding(landedCoordCollisionMatrix,X,Y))
                    {
                        return ePositionStatus.Clash;
                    }
                }
                _landedRocketsCoordinates.Add((X, Y));
                return ePositionStatus.OkForLanding; 
            }


            if (_lastCollistionCoordinates.X == -1 && _lastCollistionCoordinates.Y == -1)
            {
                ResetLastCollision(X, Y);
                return ePositionStatus.OutOfPlatform;
            }

            var collisionMatrix = RebuildCollisionMatrix(_lastCollistionCoordinates.X, _lastCollistionCoordinates.Y);
            if (IsColliding(collisionMatrix,X,Y)) 
            {
                ResetLastCollision(X, Y);
                return ePositionStatus.Clash; 
            };


            ResetLastCollision(X,Y);
            return ePositionStatus.OutOfPlatform;

        }


        private void ResetLastCollision(int X, int Y)
        {
            _lastCollistionCoordinates.X = X;
            _lastCollistionCoordinates.Y = Y;
        }

        private bool IsOnPlatform(int X, int Y)
        {
            return  X >= _platformArea.Xstart && X <=_platformArea.Xend  && Y >= _platformArea.Ystart  && Y <= _platformArea.Yend;
        }

        private List<(int X, int Y)> RebuildCollisionMatrix(int X, int Y)
        {
            var _collistionCoordinates = new List<(int X, int Y)>();
            _collistionCoordinates.Add((X, Y));
            _collistionCoordinates.Add((X-1, Y));
            _collistionCoordinates.Add((X-1, Y-1));
            _collistionCoordinates.Add((X, Y - 1));
            _collistionCoordinates.Add((X + 1, Y - 1));
            _collistionCoordinates.Add((X + 1, Y));
            _collistionCoordinates.Add((X + 1, Y+1));
            _collistionCoordinates.Add((X, Y + 1));
            _collistionCoordinates.Add((X - 1, Y + 1));

            return _collistionCoordinates;
        }

        private bool IsColliding(List<(int X, int Y)> collisionMatrixcoords,int Xpos, int Ypos)
        {
            foreach (var coord in collisionMatrixcoords)
            {
                if (coord.X == Xpos && coord.Y == Ypos) { return true; }
            }
            return false;
        }
    }
}