using Infrastructure.Identity;

namespace UnitTests.Services
{
    public class UserServiceTest
    {
        [Fact]
        public void is_user_email_confirmed_should_return_true_when_input_is_true()
        {
            //Arrange
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = "Łukasz";
            applicationUser.EmailConfirmed = true;

            UserService userService = new UserService();

            //Act
            bool isEmailConfirmed = userService.IsUserEmailConfirmed(applicationUser);

            //Assert
            Assert.True(isEmailConfirmed);
        }

        [Fact]
        public void is_user_email_confirmed_should_return_false_when_input_is_false()
        {
            //Arrange
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = "Łukasz";
            applicationUser.EmailConfirmed = false;

            UserService userService = new UserService();

            //Act
            bool isEmailConfirmed = userService.IsUserEmailConfirmed(applicationUser);

            //Assert
            Assert.False(isEmailConfirmed);
        }
    }
}