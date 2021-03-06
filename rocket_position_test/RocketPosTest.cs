using rocket_position;
using System;
using Xunit;
using static rocket_position.Helper;

namespace rocket_position_test
{
    public class RocketPosTest
    {
        private readonly RocketPositioning _rocketPos;
        public RocketPosTest()
        {
            _rocketPos = new RocketPositioning();
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(-20)]
        public void rocketPosPlatformIsNegativeThrowsError(int platformSize)
        {
            ArgumentOutOfRangeException actualException = Assert.Throws<ArgumentOutOfRangeException>(() => new RocketPositioning(platformSize: platformSize));
            Assert.Equal("platform Size cannot be negative", actualException.ParamName);
        }


        [Theory]
        [InlineData(100)]
        [InlineData(120)]
        public void rocketPosPlatformOutOfBoundsonAxisThrowsError(int platformSize)
        {
            Exception actualException = Assert.Throws<Exception>(() => new RocketPositioning(platformSize: platformSize));
            Assert.Equal("Platform size out of bounds on X or Y coordinate", actualException.Message);
        }


        [Theory]
        [InlineData(0, 0)]
        [InlineData(100, 100)]
        [InlineData(0, 100)]
        [InlineData(100, 0)]
        [InlineData(50, 50)]
        [InlineData(70, 70)]
        public void checkLandingPositionOutOfPlatformShouldPass(int X, int Y)
        {

            var result = _rocketPos.CheckLandingPlatform(X, Y);
            Assert.Equal(ePositionStatus.OutOfPlatform, result);
        }

        [Theory]
        [InlineData(5, 5)]
        [InlineData(15, 15)]
        [InlineData(5, 15)]
        [InlineData(15, 5)]
        [InlineData(10, 10)]
        public void checkLandingInPlatformShouldPass(int X, int Y)
        {
            var result = _rocketPos.CheckLandingPlatform(X, Y);
            Assert.Equal(ePositionStatus.OkForLanding, result);
        }


        [Theory]
        [InlineData(50, 50)]
        [InlineData(60, 60)]
        public void checkLandingOutsidePlatformWithFixedCollisionShouldPass(int X, int Y)
        {
            Assert.Equal(ePositionStatus.OutOfPlatform, _rocketPos.CheckLandingPlatform(X, Y));
            Assert.Equal(ePositionStatus.Clash, _rocketPos.CheckLandingPlatform(X, Y));
            Assert.Equal(ePositionStatus.Clash, _rocketPos.CheckLandingPlatform(X, Y));
        }

        [Theory]
        [InlineData(50, 50)]
        [InlineData(49, 50)]
        [InlineData(49, 49)]
        [InlineData(50, 49)]
        [InlineData(51, 49)]
        [InlineData(51, 50)]
        [InlineData(51, 51)]
        [InlineData(50, 51)]
        [InlineData(49, 51)]
        public void checkLandingWithFixedCollisionLocationWithinOneBlockShouldPass(int X, int Y)
        {
            Assert.Equal(ePositionStatus.OutOfPlatform, _rocketPos.CheckLandingPlatform(50, 50));
            Assert.Equal(ePositionStatus.Clash, _rocketPos.CheckLandingPlatform(X, Y));
        }

        [Fact]
        public void checkLandingInPlatformMulipleRocketsShouldPass()
        {
            Assert.Equal(ePositionStatus.OkForLanding, _rocketPos.CheckLandingPlatform(10, 10));
            Assert.Equal(ePositionStatus.OkForLanding, _rocketPos.CheckLandingPlatform(5, 5));
            Assert.Equal(ePositionStatus.OkForLanding, _rocketPos.CheckLandingPlatform(15, 15));
            Assert.Equal(ePositionStatus.OutOfPlatform, _rocketPos.CheckLandingPlatform(20, 20));
            Assert.Equal(ePositionStatus.OutOfPlatform, _rocketPos.CheckLandingPlatform(0, 0));
            Assert.Equal(3, _rocketPos.LandedRocketsCoordinates.Count);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(9, 10)]
        [InlineData(9, 9)]
        [InlineData(10, 9)]
        [InlineData(11, 9)]
        [InlineData(11, 10)]
        [InlineData(11, 11)]
        [InlineData(10, 11)]
        [InlineData(9, 11)]
        public void checkLandingInPlatformMultipleRocketsWithCollisionShouldPass(int X, int Y)
        {
            Assert.Equal(ePositionStatus.OkForLanding, _rocketPos.CheckLandingPlatform(10, 10));
            Assert.Equal(ePositionStatus.Clash, _rocketPos.CheckLandingPlatform(X, Y));
        }


        [Fact]
        public void checkLandingEnlargedPlatformShouldPass()
        {
            var rocketPos = new RocketPositioning(platformSize:50);

            Assert.Equal(ePositionStatus.OkForLanding, rocketPos.CheckLandingPlatform(20,9));
            Assert.Equal(ePositionStatus.OkForLanding, rocketPos.CheckLandingPlatform(30, 40));
            Assert.Equal(ePositionStatus.OkForLanding, rocketPos.CheckLandingPlatform(50, 50));
            Assert.Equal(ePositionStatus.OkForLanding, rocketPos.CheckLandingPlatform(9, 40));
            Assert.Equal(ePositionStatus.OutOfPlatform, rocketPos.CheckLandingPlatform(60, 60));
            Assert.Equal(ePositionStatus.OutOfPlatform, rocketPos.CheckLandingPlatform(70, 9));
            Assert.Equal(ePositionStatus.OutOfPlatform, rocketPos.CheckLandingPlatform(0, 0));
            Assert.Equal(ePositionStatus.Clash, rocketPos.CheckLandingPlatform(0, 0));
            Assert.Equal(4, rocketPos.LandedRocketsCoordinates.Count);
        }
    }
}