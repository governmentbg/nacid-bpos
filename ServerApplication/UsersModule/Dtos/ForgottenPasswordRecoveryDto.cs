namespace ServerApplication.UsersModule.Dtos
{
	public class ForgottenPasswordRecoveryDto
	{
		public string Token { get; set; }
		public string NewPassword { get; set; }
		public string NewPasswordAgain { get; set; }
	}
}
