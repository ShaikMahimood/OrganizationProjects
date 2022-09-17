CREATE DATABASE OrganizationProjectDb

USE OrganizationProjectDb

CREATE TABLE [dbo].[Users](UserId INT IDENTITY(1,1), UserName NVARCHAR(50) NOT NULL, EmailId NVARCHAR(50) PRIMARY KEY,
						  Password NVARCHAR(50) NOT NULL, PhoneNumber BIGINT NOT NULL, Address NVARCHAR(50) NOT NULL);
Select * from Users
--Create Procedure to Insert User
CREATE PROCEDURE [dbo].[UserInsertProc](@UserName NVARCHAR(50), @EmailId NVARCHAR(50), @Password NVARCHAR(50), @PhoneNumber BIGINT, @Address NVARCHAR(50))
AS  BEGIN
	INSERT INTO [dbo].[Users] values(@UserName, @EmailId, @Password, @PhoneNumber, @Address)
	END

--Create Procedure to Update User
CREATE PROCEDURE [dbo].[UserUpdateProc](@UserName NVARCHAR(50), @Password NVARCHAR(50), @EmailId NVARCHAR(50), @PhoneNumber BIGINT, @Address NVARCHAR(50))
AS  BEGIN
	UPDATE [dbo].[Users] SET UserName = @UserName, Password = @Password, PhoneNumber = @PhoneNumber, Address = @Address WHERE EmailId = @EmailId
	END


--Create Procedure to Delete User
CREATE PROCEDURE [dbo].[UserDeleteProc](@EmailId NVARCHAR(50))
AS  BEGIN
	DELETE FROM [dbo].[Users] WHERE  EmailId = @EmailId
	END

--Create Procedure to Get All Users
CREATE PROCEDURE [dbo].[UserSelectProc]
AS  BEGIN
	SELECT UserId,UserName,EmailId,PhoneNumber,Address FROM [dbo].[Users]
	END

exec UserSelectProc 
--Create Procedure to Get User by Username or EmailId 
CREATE PROCEDURE [dbo].[UserGetByNameOrEmailIdProc](@UsernameOrEmailId NVARCHAR(50))
AS  BEGIN
	SELECT UserId, UserName, EmailId, PhoneNumber, Address FROM [dbo].[Users] WHERE  UserName = @UsernameOrEmailId Or EmailId = @UsernameOrEmailId
	END

--Create Procedure to login user
CREATE PROCEDURE [dbo].[UserLoginProc](@EmailId NVARCHAR(50))
AS  BEGIN
	SELECT Password FROM [dbo].[Users] WHERE  EmailId = @EmailId
	END
