/*
    ==================================================================
    Description
        Add
    ==================================================================
    History
        2018/12/11  Carter    Created.
    ==================================================================
    Step

    ==================================================================
    Result

	==================================================================
    Example
		exec NSP_PubMessage_Add
		@Content = '123'
*/

CREATE PROCEDURE [dbo].[NSP_PubMessage_Add]
	@Content NVARCHAR(255)
AS
	INSERT INTO PubMessage(Content)
	OUTPUT inserted.Content
	VALUES(@Content)

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[NSP_PubMessage_Add] TO PUBLIC
    AS [dbo];