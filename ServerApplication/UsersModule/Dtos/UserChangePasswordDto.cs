namespace ServerApplication.UsersModule.Dtos
{
	public class UserChangePasswordDto
	{
		public string OldPassword { get; set; }

		public string NewPassword { get; set; }
		public string NewPasswordAgain { get; set; }
	}
}
