namespace OpenScience.Services.Users.Enums
{
	public enum UserErrorCode
	{
		User_InvalidCredentials = 201,
		User_EmailTaken = 202,
		User_ActivationTokenAlreadyUsed = 203,
		User_ActivationTokenExpired = 204,
		User_UserAlreadyUnlocked = 205,
		User_UserAlreadyDeactivated = 206,
		User_CannotRestoreUserPassword = 207,
		User_PasswordRecoveryTokenAlreadyUsed = 208,
		User_PasswordRecoveryTokenExpired = 209,
		User_ChangePasswordNewPasswordMismatch = 210,
		User_ChangePasswordOldPasswordMismatch = 211
	}
}
